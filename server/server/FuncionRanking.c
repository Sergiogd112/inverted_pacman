#include "FuncionRanking.h"

/**
 * Retrieves the ranking data from a MySQL database.
 * @param conn A pointer to a MySQL connection object.
 * @param res A character array to store the ranking data.
 * @return The number of rows retrieved from the ranking query.
 */
int Get_Ranking(MYSQL *conn , char res[1000])
{
    int err;

    // Query to retrieve values from columns
    MYSQL_RES *result;
    err = mysql_query(conn, "SELECT ID, puntos, nombre FROM usuarios ORDER BY puntos DESC LIMIT 10");

    // Check if the query executed successfully
    if (err != 0)
    {
        fprintf(stderr, "The query failed is: %s", mysql_error(conn));
        exit(1);
    }

    // Create lists for each column
    result = mysql_store_result(conn);
    MYSQL_ROW row;
    row = mysql_fetch_row(result);
    int i = 0;
    if (row == NULL)
    {
        sprintf(res, "Nothing");
    }
    else
    {

        // Add the values of each column to the lists
        for (i = 0; row != NULL; i++)
        {
            // ID[] = strdup(mysqlgetvalue(result, i, 0));
            // Name[] = strdup(mysqlgetvalue(result, i, 1));
            // Points[] = strdup(mysqlgetvalue(result, i, 2));
            sprintf(res, "%s%s*%s,", res, row[2], row[1]); // Append the name and points to the response string
            row = mysql_fetch_row(result);
        }
    }
    res[strlen(res) - 2] = '\0'; // Remove the trailing comma from the response string

    mysql_close(conn);
    return i;  // Return the number of rows retrieved from the ranking query
}
