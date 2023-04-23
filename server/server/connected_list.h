//
// Created by sergio on 22/03/23.
//

#ifndef SERVER_CONNECTED_LIST_H
#define SERVER_CONNECTED_LIST_H
// server.h : Include file for standard system include files,
// or project specific include files.

// #pragma once

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <string.h>
#include "config.h"

#define MAXUSERS 200
// TODO: Reference additional headers your program requires here.
struct Node
{
    int id;
    int sockfd;
    char name[20];
    int idx;
    int sending_connected;
    int using;
};

typedef struct
{
    struct Node connections[MAXUSERS];
    int used;
    int idx;
    int update_connecetions;
} ConnectedList;

void initialize_list(ConnectedList *list);

int insert_to_llist(ConnectedList *list, int new_id, int new_sockfd, char name[20]);

int get_empty(ConnectedList *list);

int search_on_llist(ConnectedList *list, int target_id);

int remove_node_from_list(ConnectedList *list, int target_i);

int llist_to_string(ConnectedList *list, char res[200]);

void print_idx(ConnectedList *list);

int connected_to_string(ConnectedList *list, char* res, size_t maxlen);

void push_connected(ConnectedList *list, char res[200], int n);

void print_Node(struct Node *node);

void print_Connections(ConnectedList *list);

#endif // SERVER_CONNECTED_LIST_H