#ifndef SERVER_PETICIONES_H
#define SERVER_PETICIONES_H
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <mysql/mysql.h>
#include loggear.h

char *get_partidas_string_by_name(MYSQL *conn, const char *name, int *string_length);

char *obtenerNombres(MYSQL *conexion, const char *nombre, int *longitud);

#endif // SERVER_PETICIONES_H
