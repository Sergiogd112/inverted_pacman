//
// Created by antonia on 5/05/23.
//

#ifndef SERVER_CHAT_H
#define SERVER_CHAT_H

#include "config.h"
#include "connected_list.h"
#include <mysql/mysql.h>
#include <string.h>
int write_message(MYSQL *conn, Nombre name, char *text);
char * chat_to_string(MYSQL *conn, int *n);
void push_chat(ConnectedList *list, char *res, int n);
#endif //SERVER_CHAT_H
