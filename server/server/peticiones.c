#include "peticiones.h"

char *get_partidas_string_by_name(MYSQL *conn, const char *name, int *string_length)
{
    MYSQL_RES *res;
    MYSQL_ROW row;
    char query[2000];
    char *partidas_string = NULL;
    *string_length = 0;
    char logmsg[200];
    snprintf(logmsg, 200, "get_partidas_string_by_name: %s", name);
    logger(LOGINFO, logmsg);
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

    if (mysql_query(conn, query) != 0)
    {
        fprintf(stderr, "Error executing MySQL query: %s\n", mysql_error(conn));
        return NULL;
    }

    res = mysql_use_result(conn);
    if (res == NULL)
    {
        fprintf(stderr, "Error retrieving MySQL result set\n");
        return NULL;
    }

    if ((row = mysql_fetch_row(res)) != NULL)
    {
        if (row[0] != NULL)
        {
            *string_length = snprintf(NULL, 0, "%s", row[0]);
            partidas_string = malloc((*string_length + 1) * sizeof(char));
            snprintf(partidas_string, *string_length + 1, "%s", row[0]);
        }
    }

    mysql_free_result(res);

    return partidas_string;
}

char *obtenerNombres(MYSQL *conexion, const char *nombre, int *longitud)
{
    // Crear la consulta SQL
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
    // Ejecutar la consulta
    if (mysql_query(conexion, consulta))
    {
        fprintf(stderr, "Error al ejecutar la consulta: %s\n", mysql_error(conexion));
        return NULL;
    }

    // Obtener el resultado
    MYSQL_RES *resultado = mysql_store_result(conexion);
    if (resultado == NULL)
    {
        fprintf(stderr, "Error al obtener el resultado: %s\n", mysql_error(conexion));
        return NULL;
    }
    snprintf(logmsg, 200, "nombres obtenidos: %d", mysql_num_rows(resultado));
    logger(LOGINFO, logmsg);

    // Obtener el número de filas
    int numFilas = mysql_num_rows(resultado);

    // Calcular la longitud total de la cadena resultante
    int longitudTotal = numFilas; // Incluye las comas entre los nombres
    MYSQL_ROW fila;
    while ((fila = mysql_fetch_row(resultado)))
    {
        longitudTotal += strlen(fila[0]); // Longitud de cada nombre
    }

    // Reservar memoria para la cadena resultante
    char *nombres = (char *)malloc(longitudTotal * sizeof(char));
    if (nombres == NULL)
    {
        fprintf(stderr, "Error al reservar memoria\n");
        mysql_free_result(resultado);
        return NULL;
    }

    // Construir la cadena resultante
    nombres[0] = '\0'; // Inicializar la cadena vacía
    while ((fila = mysql_fetch_row(resultado)))
    {
        logger(LOGINFO, fila[0]);
        strcat(nombres, fila[0]);
        strcat(nombres, ",");
    }
    nombres[strlen(nombres) - 1] = '\0'; // Eliminar la última coma

    // Liberar el resultado y asignar la longitud resultante
    mysql_free_result(resultado);
    *longitud = longitudTotal - 1; // Excluir la longitud de la última coma

    return nombres;
}
