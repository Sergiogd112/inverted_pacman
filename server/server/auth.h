#ifndef AUTH_H
#define AUTH_H
#include <mysql/mysql.h>
#include <stdio.h>
#include <string.h>
#include <openssl/sha.h>


#define SERVER "localhost"
#define USER "root"
#define PASSWORD "mysql"
#define DATABASE "InvertedPacman"
int register_user(MYSQL *conn,char name[30], char email[30], char password[30]);
int login(MYSQL *conn,char name[30], char password[30]);
#endif
