#ifndef AUTH_H
#define AUTH_H
#include <mysql/mysql.h>
#include <string.h>
#include <openssl/sha.h>
#include "config.h"

int register_user(MYSQL *conn, char name[30], char email[30], char password[30]);
int login(MYSQL *conn, char name[30], char password[30]);
#endif
