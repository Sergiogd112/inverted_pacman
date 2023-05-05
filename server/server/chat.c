//
// Created by antonia on 5/05/23.
//

#include "chat.h"

void write_message(MYSQL *conn, Nombre name, char text[MAXCHATMSGLEN]) {
    char query[MAXCHATMSGLEN + 200];
    snprintf(query, MAXCHATMSGLEN + 200,
             "INSERT INTO chat (id_usuario, mensage) VALUES ((SELECT ID FROM usuarios WHERE nombre = '%s'), '%s');",
             name,
             text);
    if (mysql_query(conn,
                    query)) { // Execute the query using the mysql_query function and check if it returns an error.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    printf("Chat writen successfully");
}

int chat_to_string(MYSQL *conn, char *res) {

    char query[] = "SELECT LENGTH(GROUP_CONCAT(CONCAT(usuarios.nombre, '*', chat.time, '*', chat.mensage) SEPARATOR ',')) AS len,\n"
                   "       GROUP_CONCAT(CONCAT(usuarios.nombre, '*', chat.time, '*', chat.mensage) SEPARATOR ',') AS str\n"
                   "FROM usuarios, chat\n"
                   "WHERE usuarios.id = chat.id_usuario;";
    if (mysql_query(conn,
                    query)) { // Execute the query using the mysql_query function and check if it returns an error.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    MYSQL_RES *result;

    result = mysql_store_result(conn);
    MYSQL_ROW row;
    row = mysql_fetch_row(result);
    int i = 0;
    if (row == NULL) {
        res=malloc(sizeof(char));
        snprintf(res,sizeof(res), "");
        return 0;
    } else {

        res= malloc(sizeof(char)*atoi(row[0]));
        snprintf(res,sizeof(res),"%s",row[1]);
        res[strlen(res) - 2] = '\0';
    }
    return i;
}