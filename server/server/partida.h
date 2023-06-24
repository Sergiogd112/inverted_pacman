//
// Created by antonia on 30/04/23.
//

#ifndef SERVER_PARTIDA_H
#define SERVER_PARTIDA_H
#define MAXPARTIDAS 20

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <string.h>
#include <unistd.h>
#include "config.h"
#include <pthread.h>
#include "logger.h"
typedef struct
{
    float x;
    float y;
} Position;
typedef struct
{
    Position pos;
    int id;
} Enemy;

typedef struct
{
    Nombre nombres[4];
    Position player_pos[4];
    Enemy enemys[4];
    int sockets[4];
    int puntos[4];
    int vidas[4];
    int usando;
    int jugando;
    int idx;
    int answer[4];
    int listos[4];
    pthread_mutex_t mutex;
    int kill;
} Partida;

typedef struct
{
    Partida partidas[MAXPARTIDAS];
    int used;
    int idx;
    int update_connecetions;
} ListaPartidas;

typedef struct
{
    Partida *partida;
    ListaPartidas *listaPartidas;

} PartidaArgs;

void initialize_partidas_list(ListaPartidas *list);

void reset_partida(Partida *partida);

int get_empty_from_partidas_list(ListaPartidas *list);

int search_on_partidas_llist(ListaPartidas *list, int target_idx);

int remove_node_from_partidas_list(ListaPartidas *list, int target_i);

int partidas_llist_to_string(ListaPartidas *list, char res[200]);

void print_partidas_idx(ListaPartidas *list);

void Atender_Cliente_Partida(Partida *partida, Nombre nombre, MYSQL *conn);

#endif // SERVER_PARTIDA_H
