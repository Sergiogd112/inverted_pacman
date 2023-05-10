//
// Created by antonia on 22/03/23.
//

#ifndef SERVER_MAIN_H
#define SERVER_MAIN_H 1
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
#include "config.h"
#include "partida.h"
#include "chat.h"
#include "logger.h"
pthread_mutex_t main_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t glubal_update_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t crear_partida_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t invitation_mutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t chat_mutex = PTHREAD_MUTEX_INITIALIZER;
char DBSERVER[30];

typedef struct
{
    int i;
    ConnectedList *list;
    ListaPartidas *lista_partidas;
//    LogQueue *queue;
} ThreadArgs;
typedef struct {
    ListaPartidas *listaPartidas;
    Partida *partida;
    ConnectedList *list;
}InvitationArgs;

//typedef struct
//{
//    ConnectedList *list;
//    LogQueue *queue;
//
//} UpdateConnectedThreadArgs;

#endif // SERVER_MAIN_H
