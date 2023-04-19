#include "FuncionRanking.h"

int Devuelveme_Ranking(char res[1000]) {
    //char ID[20],char Nombre[20],int Puntos,
    MYSQL *conn;
    int err;

    //Creamos una conexion al servidor MYSQL
    conn = mysql_init(NULL);
    if (conn == NULL) {
        printf("Error al crear la conexion: %u %s\n",
               mysql_errno(conn), mysql_error(conn));
        exit(1);
    }

    //Inicializar la conexion
    conn = mysql_real_connect(conn, "localhost", "root", "mysql", "partidas_usuarios", 0, NULL, 0);
    if (conn == NULL) {
        printf("Error al inicializar la conexion: %u %s\n",
               mysql_errno(conn), mysql_error(conn));
        exit(1);
    }


    //Consulta para recuperar valores de las columnas
    MYSQL_RES *result;
    err = mysql_query(conn, "SELECT TOP 10 ID, puntos, nombre FROM usuarios ORDER BY puntos DESC");

    //Compruebo si la consulta ha ido bien
    if (err != 0) {
        fprintf(stderr, "Error de consulta: %s", mysql_error(conn));
        exit(1);
    }

    //Creo listas para cada columna
    result = mysql_store_result(conn);
    MYSQL_ROW row;
    row= mysql_fetch_row(result);
    int i=0;
    if (row == NULL) {
        sprintf(res, "nada");
    }else {

        //Agregar los valores de cada columna a las listas
        for (i = 0; row!=NULL; i++) {
            //ID[] = strdup(mysqlgetvalue(result, i, 0));
            //Nombre[] = strdup(mysqlgetvalue(result, i, 1));
            //Puntos[] = strdup(mysqlgetvalue(result, i, 2));
            sprtinf(res, "%s%s*%s,", res, row[2], row[1]);
            row= mysql_fetch_row(result);
        }
    }



    mysql_close(conn);
    return i;

}