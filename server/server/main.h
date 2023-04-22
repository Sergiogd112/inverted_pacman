//
// Created by antonia on 22/03/23.
//

#ifndef SERVER_MAIN_H
#define SERVER_MAIN_H
#include "auth.h"
#include "FuncionRanking.h"
#include "connected_list.h"
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <string.h>
#include <mysql/mysql.h>
#include <pthread.h>
#include <unistd.h>


pthread_mutex_t main_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t update_connected_mutex = PTHREAD_MUTEX_INITIALIZER;
typedef struct {
    int i;
    ConnectedList *list;
}ThreadArgs;

#endif //SERVER_MAIN_H
