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
 * @param conexion The MySQL connection object.
 * @param nombre The name to search for.
 * @param longitud Pointer to an integer to store the length of the resulting string.
 * @return A dynamically allocated string containing the distinct names, separated by commas, or NULL if an error occurs.
 */
char *obtenerNombres(MYSQL *conexion, const char *nombre, int *longitud)
{
    // Create the SQL query
    char consulta[200];
    char logmsg[200];
    snprintf(logmsg, 200, "obtenerNombres: %s", nombre);
    logger(LOGINFO, logmsg);
    sprintf(consulta, "SELECT DISTINCT u.nombre \
                    FROM usuarios u \
                    INNER JOIN partidas_usuarios pu1 ON u.ID = pu1.id_usuario \
                    INNER JOIN partidas_usuarios pu2 ON pu1.id_partida = pu2.id_partida \
                    INNER JOIN usuarios u2 ON u2.ID = pu2.id_usuario \
                    WHERE u2.nombre = '%s' AND u.nombre != '%s'",
            nombre, nombre);
    // Run the query
    if (mysql_query(conexion, consulta))
    {
        fprintf(stderr, "Error al ejecutar la consulta: %s\n", mysql_error(conexion));
        return NULL;
    }

    // Obtain the results
    MYSQL_RES *resultado = mysql_store_result(conexion);
    if (resultado == NULL)
    {
        fprintf(stderr, "Error al obtener el resultado: %s\n", mysql_error(conexion));
        return NULL;
    }
    snprintf(logmsg, 200, "nombres obtenidos: %d", mysql_num_rows(resultado));
    logger(LOGINFO, logmsg);

    //Obtain the number of rows
    int numFilas = mysql_num_rows(resultado);

    char *nombres = NULL;
    int nombresSize = 0;
    // Calculate the total length of the resulting string
    int longitudTotal = numFilas;// Include commas between names
    MYSQL_ROW fila;
    while ((fila = mysql_fetch_row(resultado)))
    {
        int filaSize = strlen(fila[0]) + 1; // +1 for the comma
        nombres = realloc(nombres, (nombresSize + filaSize) * sizeof(char));
        snprintf(nombres + nombresSize, filaSize, "%s,", fila[0]);
        nombresSize += filaSize;
    }

    nombres[nombresSize - 1] = '\0';// Remove last comma

    // Free the result and assign the resulting length
    mysql_free_result(resultado);
    *longitud = longitudTotal - 1; // Exclude the length of the last comma

    return nombres;
}
