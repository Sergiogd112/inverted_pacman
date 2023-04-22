//
// Created by Sergio on 22/03/23.
//

#include "connected_list.h"

void initialize_list(ConnectedList *list) {
    list->idx = 0;
    list->used = 0;

    for (int i = 0; i < MAXUSERS; i++) {
        list->connections[i].id = -1;
        list->connections[i].sockfd = 0;
        list->connections[i].using = 0;
        list->connections[i].sending_connected = 0;
        strcpy(list->connections[i].name, "name");
        list->connections[i].idx = -1;
    }
}

void reset_node(struct Node * node){
    node->id = -1;
    node->sockfd = 0;
    node->using = 0;
    node->sending_connected = 0;
    strcpy(node->name, "name");
    node->idx = -1;
}

int insert_to_llist(ConnectedList *list, int new_id, int new_sockfd, char name[20]) {
    if (list->used == MAXUSERS) {
        return -1;
    }
    for (int i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 0) {
            list->connections[i].id = new_id;
            list->connections[i].sockfd = new_sockfd;
            strcpy(list->connections[i].name, name);
            list->connections[i].idx = list->idx;
            list->connections[i].using=1;
            list->idx++;
            list->used++;
            return i;
        }
    }
    return -1;
}
int get_empty(ConnectedList *list){
    if (list->used == MAXUSERS) {
        return -1;
    }
    for (int i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 0) {
            list->connections[i].using=1;
            list->connections[i].idx = list->idx;
            list->idx++;
            list->used++;
            return i;
        }
    }
    return -1;
}
//// append a node at the end of the list
//void append_to_llist(struct Node **head_ref, struct Node **new_node_ref) {
//    // allocate memory for new node
//    struct Node *head = *head_ref;
//
//    struct Node *last = *head_ref;
//    struct Node *new_node = *new_node_ref;
//    if (last != NULL) {
//        while (last->next != NULL)
//            last = last->next;
//        last->next = new_node;
//        new_node->prev = last;
//    } else {
//        // if the list is empty, make new node as head and prev as NULL
//        head = new_node;
//        new_node->prev = NULL;
//    }
//}

// search a node by id and return its socket file descriptor or -1 if not found
int search_on_llist(ConnectedList *list, int target_id) {
    for (int i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 1 && list->connections[i].id == target_id)
            return i;
    }

    return -1; // not found
}

// remove a node by id and return its socket file descriptor or -1 if not found
int remove_node_from_list(ConnectedList *list, int target_i) {
    if (target_i > MAXUSERS)
        return -1;
    if (list->connections[target_i].using == 0)
        return -2;
    int err = write(list->connections[target_i].sockfd, "checking connection", strlen("checking connection"));
    if (err != -1)
        close(list->connections[target_i].sockfd);
    reset_node(&list->connections[target_i]);
    list->used--;
}

int llist_to_string(ConnectedList *list, char res[2000]) {
    int i;
    for (i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 1)
            snprintf(res, 2000, "%s%s,", res, list->connections[i].name);
    }
    res[strlen(res) - 1] = '\0';
    return i;
}

void print_idx(ConnectedList *list) {
    char res[2000];
    for (int i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 1) {
            print_Node(&list->connections[i]);
            snprintf(res, 2000, "%s%d,", res, list->connections[i].idx);
        }
    }

    res[strlen(res) - 1] = '\0';
    printf("%s\n", res);
}

int connected_to_string(ConnectedList *list, char res[2000]) {
    int i;
    for (i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 1 && list->connections[i].id == -1)
            snprintf(res, 2000, "%s%s,", res, list->connections[i].name);
    }
    res[strlen(res) - 1] = '\0';
    return i;
}

void push_connected(ConnectedList *list, char res[2000], int n) {
    char response[2010];
    snprintf(response, 2010, "4/%d/%s\x04", n, res);
    printf("%s\n", response);
    for (int i = 0; i < MAXUSERS; i++) {
        if (list->connections[i].using == 1 && list->connections[i].id == -1) {
            list->connections[i].sending_connected = 1;
            write(list->connections[i].sockfd, response, strlen(response));
            list->connections[i].sending_connected = 0;
        }
        snprintf(res, 2000, "%s%s,", res, list->connections[i].name);
    }

}

void print_Node(struct Node *node) {
    printf("---\n");
    if (node->using == 0)
        printf("Not using");
    else
        printf("id: %d\nsocketfd: %d\nname: %s\nidx: %d\nsending_connected: %d\n",
               node->id,
               node->sockfd,
               node->name,
               node->idx,
               node->sending_connected
        );
    printf("---\n");
}

