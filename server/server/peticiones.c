#include "peticiones.h"
/**
 * Retrieves a string representation of the game sessions associated with a given name from the database.
 * @param conn The MySQL connection object.
 * @param name The name to search for.
 * @param string_length Pointer to an integer to store the length of the resulting string.
 * @return A dynamically allocated string containing the game sessions information, or NULL if an error occurs.
 */
char *get_partidas_string_by_name(MYSQL *conn, const char *name, int *string_length)
{
    MYSQL_RES *res;
    MYSQL_ROW row;
    char query[2000];
    char *partidas_string = NULL;
    *string_length = 0;
    char logmsg[200];
    // Log the function call
    snprintf(logmsg, 200, "get_partidas_string_by_name: %s", name);
    logger(LOGINFO, logmsg);
    // Construct the MySQL query to retrieve the game sessions string
    snprintf(query, 2000, "SELECT GROUP_CONCAT(CONCAT(partidas.id_partida, '*', usuarios_partida.str, '*', partidas.puntuacion_global) SEPARATOR ',') AS partidas_string "
                          "FROM partidas "
                          "INNER JOIN ( "
                          "SELECT id_partida, GROUP_CONCAT(usuarios.nombre ORDER BY partidas_usuarios.id_usuario SEPARATOR '*') AS str "
                          "FROM partidas_usuarios "
                          "INNER JOIN usuarios ON usuarios.ID = partidas_usuarios.id_usuario "
                          "WHERE partidas_usuarios.id_partida IN ( "
                          "SELECT id_partida "
                          "FROM partidas_usuarios "
                          "WHERE id_usuario = ( "
                          "SELECT ID "
                          "FROM usuarios "
                          "WHERE nombre = '%s' "
                          ") "
                          ") "
                          "GROUP BY id_partida "
                          ") AS usuarios_partida ON partidas.id_partida = usuarios_partida.id_partida;",
             name);
    // Execute the MySQL query
    if (mysql_query(conn, query) != 0)
    {
        fprintf(stderr, "Error executing MySQL query: %s\n", mysql_error(conn));
        return NULL;
    }
    // Get the result set from the query
    res = mysql_use_result(conn);
    if (res == NULL)
    {
        fprintf(stderr, "Error retrieving MySQL result set\n");
        return NULL;
    }
    // Fetch the row from the result set
    if ((row = mysql_fetch_row(res)) != NULL)
    {
        if (row[0] != NULL)
        {
            // Calculate the length of the resulting string
            *string_length = snprintf(NULL, 0, "%s", row[0]);
            // Allocate memory for the resulting string
            partidas_string = malloc((*string_length + 1) * sizeof(char));
            // Copy the resulting string
            snprintf(partidas_string, *string_length + 1, "%s", row[0]);
        }
    }
    // Free the result set
    mysql_free_result(res);

    return partidas_string;
}

/**
 * Retrieves distinct names from the database based on a given name.
 * @param conn The MySQL connection object.
 * @param nombre The name to search for.
 * @param n Pointer to an integer to store the length of the resulting string.
 * @return A dynamically allocated string containing the distinct names, separated by commas, or NULL if an error occurs.
 */
char *obtenerNombres(MYSQL *conn, const char *nombre, int *n)
{
    // Create the SQL queries
    char query1[250];
    char query2[250];
    snprintf(query1, 250, "SELECT LENGTH(GROUP_CONCAT(DISTINCT u.nombre SEPARATOR ',')) AS len \
                    FROM usuarios u \
                    INNER JOIN partidas_usuarios pu1 ON u.ID = pu1.id_usuario \
                    INNER JOIN partidas_usuarios pu2 ON pu1.id_partida = pu2.id_partida \
                    INNER JOIN usuarios u2 ON u2.ID = pu2.id_usuario \
                    WHERE u2.nombre = '%s' AND u.nombre != '%s'",
            nombre, nombre);

    snprintf(query2, 250, "SELECT GROUP_CONCAT(DISTINCT u.nombre SEPARATOR ',') AS str \
                    FROM usuarios u \
                    INNER JOIN partidas_usuarios pu1 ON u.ID = pu1.id_usuario \
                    INNER JOIN partidas_usuarios pu2 ON pu1.id_partida = pu2.id_partida \
                    INNER JOIN usuarios u2 ON u2.ID = pu2.id_usuario \
                    WHERE u2.nombre = '%s' AND u.nombre != '%s'",
            nombre, nombre);

    // Execute the first query
    if (mysql_query(conn, query1))
    {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    MYSQL_RES *result1 = mysql_store_result(conn);
    MYSQL_ROW row1 = mysql_fetch_row(result1);
    if (row1 == NULL)
    {
        *n = 0;
        mysql_free_result(result1);
        return NULL;
    }
    int len = atoi(row1[0]);
    mysql_free_result(result1);

    // Execute the second query
    if (mysql_query(conn, query2))
    {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    MYSQL_RES *result2 = mysql_store_result(conn);
    MYSQL_ROW row2 = mysql_fetch_row(result2);
    if (row2 == NULL)
    {
        *n = 0;
        mysql_free_result(result2);
        return NULL;
    }
    char *str = strdup(row2[0]); // Allocate memory for the string and copy the result
    mysql_free_result(result2); // Free the memory allocated for the result set

    *n = len; // Set the length of the string
    return str; // Return the string
}