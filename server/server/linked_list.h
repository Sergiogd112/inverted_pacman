//
// Created by sergio on 22/03/23.
//

#ifndef SERVER_LINKED_LIST_H
#define SERVER_LINKED_LIST_H
// server.h : Include file for standard system include files,
// or project specific include files.

//#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>

// TODO: Reference additional headers your program requires here.
struct Node {
    int id;
    int sockfd;
    struct Node *prev;
    struct Node *next;
};

void insert_to_llist(struct Node **head_ref, int new_id, int new_sockfd);

void append_to_llist(struct Node **head_ref, int new_id, int new_sockfd);

int search_on_llist(struct Node *head, int target_id);

int remove_node_from_llist(struct Node** head_ref, int target_id);

#endif //SERVER_LINKED_LIST_H
