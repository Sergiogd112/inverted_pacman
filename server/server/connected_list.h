//
// Created by sergio on 22/03/23.
//

#ifndef SERVER_CONNECTED_LIST_H
#define SERVER_CONNECTED_LIST_H
// server.h : Include file for standard system include files,
// or project specific include files.

#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <string.h>
#include "config.h"
#include "logger.h"
#define MAXUSERS 200
// TODO: Reference additional headers your program requires here.
struct Connection
{
    int id;
    int sockfd;
    Nombre name;
    int idx;
    int sending_connected;
    int using;
    int jugando;
    int invitando;
};

typedef struct
{
    struct Connection connections[MAXUSERS];
    int used;
    int idx;
    int global_message;
} ConnectedList;

void initialize_connected_list(ConnectedList *list);

int insert_to_connected_llist(ConnectedList *list, int new_id, int new_sockfd, char name[20]);

int get_empty_from_connected_list(ConnectedList *list);

int search_id_on_connected_llist(ConnectedList *list, int target_id);

int search_name_on_connected_llist(ConnectedList *list, Nombre nombre);

int remove_node_from_connected_list(ConnectedList *list, int target_i);

int connected_llist_to_string(ConnectedList *list, char res[200]);

void print_connected_idx(ConnectedList *list);

int connected_to_string(ConnectedList *list, char* res, size_t maxlen);

void push_connected(ConnectedList *list, char res[2000], int n);

void print_Node(struct Connection *node);

void print_Connections(ConnectedList *list);

#endif // SERVER_CONNECTED_LIST_H