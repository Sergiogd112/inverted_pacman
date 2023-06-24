#ifndef SERVER_PETICIONES_H
#define SERVER_PETICIONES_H
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <mysql/mysql.h>
#include "logger.h"

char *get_partidas_string_by_name(MYSQL *conn, const char *name, int *string_length);

char *obtenerNombres(MYSQL *conn, const char *nombre, int *n);

#endif // SERVER_PETICIONES_H
