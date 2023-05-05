//
// Created by antonia on 5/05/23.
//

#ifndef SERVER_CHAT_H
#define SERVER_CHAT_H

#include "config.h"
#include <mysql/mysql.h>
#include <string.h>
void write_message(MYSQL *conn, Nombre name, char text[MAXCHATMSGLEN]);
int chat_to_string(MYSQL *conn, char *res);
#endif //SERVER_CHAT_H
