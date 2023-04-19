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
#include <string.h>
// TODO: Reference additional headers your program requires here.
struct Node {
    int id;
    int sockfd;
    char name[20];
    struct Node* prev;
    struct Node* next;
};

void insert_to_llist(struct Node** head_ref, int new_id, int new_sockfd, char name[20]);

int append_to_llist(struct Node** head_ref, int new_id, int new_sockfd, char name[20]);

int search_on_llist(struct Node* head, int target_id);

int remove_node_from_llist(struct Node** head_ref, int target_id);

int llist_to_string(struct Node* head, char res[200])
#endif //SERVER_LINKED_LIST_H