//
// Created by antonia on 5/05/23.
//

#include "chat.h"

void write_message(MYSQL *conn, Nombre name, char *text) {
    char query[strlen(text) + 200];
    snprintf(query, strlen(text) + 200,
             "INSERT INTO chat (id_usuario, mensage) VALUES ((SELECT ID FROM usuarios WHERE nombre = '%s'), '%s');",
             name,
             text);
    if (mysql_query(conn,
                    query)) { // Execute the query using the mysql_query function and check if it returns an error.
        fprintf(stderr, "%s\n", mysql_error(conn));
        return 2;
    }
    printf("Chat writen successfully");
    return 0;
}

int chat_to_string(MYSQL *conn, char *res) {

    char query[] = "SELECT LENGTH(GROUP_CONCAT(CONCAT(usuarios.nombre, '*', chat.time, '*', chat.mensage) SEPARATOR ',')) AS len,\n"
                   "       GROUP_CONCAT(CONCAT(usuarios.nombre, '*', chat.time, '*', chat.mensage) SEPARATOR ',') AS str,\n"
                   "       COUNT(usuarios.nombre)\n"
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
    if (row == NULL) {
        res=malloc(sizeof(char));
        memset(res,0,sizeof(char));
        return 0;
    } else {

        res= malloc(sizeof(char)*(atoi(row[0])+1));
        snprintf(res,atoi(row[0])+1,"%s",row[1]);
        res[strlen(res) - 2] = '\0';
    }
    return atoi(row[2]);
}
void push_chat(ConnectedList *list, char *res, int n)
{
    char *response= malloc(sizeof(char)*(strlen(res)+13));                             // Buffer to hold the formatted response string
    snprintf(response, (strlen(res)+13), "10/%d/%s\x04", n, res); // Format the response string

    printf("%s\n", response); // Print the response string

    for (int i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1 && list->connections[i].id != -1)
        {
            list->connections[i].sending_connected = 1;                     // Set sending_connected flag to indicate a message is being sent
            write(list->connections[i].sockfd, response, strlen(response)); // Write the response string to the socket
            list->connections[i].sending_connected = 0;                     // Reset sending_connected flag after sending the message
        }

    }
}