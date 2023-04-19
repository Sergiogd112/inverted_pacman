//
// Created by Sergio on 22/03/23.
//

#include "linked_list.h"


void insert_to_llist(struct Node** head_ref, int new_id, int new_sockfd, char name[20]) {
    // allocate memory for new node
    struct Node* new_node = (struct Node*)malloc(sizeof(struct Node));
    // assign data to new node
    new_node->id = new_id;
    new_node->sockfd = new_sockfd;
    strcpy(new_node->name, name);
    // make new node as head and previous as NULL
    new_node->next = (*head_ref);
    new_node->prev = NULL;
    // change prev of head node to new node
    if ((*head_ref) != NULL)
        (*head_ref)->prev = new_node;
    // move the head to point to the new node
    (*head_ref) = new_node;
}
// append a node at the end of the list
int append_to_llist(struct Node** head_ref, struct Node * new_node) {
    // allocate memory for new node
    
    struct Node* last = *head_ref;
    int idx=0;
    if (last != NULL) {
        while (last->next != NULL)
            last = last->next;
        idx++;
        last->next = new_node;
        new_node->prev = last;
    }
    else {
        // if the list is empty, make new node as head and prev as NULL
        (*head_ref) = new_node;
        new_node->prev = NULL;
    }
    return idx;
}

// search a node by id and return its socket file descriptor or -1 if not found
int search_on_llist(struct Node* head, int target_id) {
    struct Node* current = head;
    while (current != NULL) {
        if (current->id == target_id)
            return current->sockfd; // found the node
        current = current->next; // move to next node
    }

    return -1; // not found
}

// remove a node by id and return its socket file descriptor or -1 if not found
int remove_node_from_llist(struct Node** head_ref, int target_id) {
    struct Node* current = *head_ref;
    while (current != NULL) {
        if (current->id == target_id) {
            // found the node to be deleted
            int sockfd = current->sockfd; // store the socket file descriptor
            // unlink the node from the list
            if (current == *head_ref) {
                // deleting the head node
                *head_ref = current->next;
                if (*head_ref != NULL)
                    (*head_ref)->prev = NULL;
            }
            else {
                // deleting an intermediate or tail node
                if (current->next != NULL)
                    current->next->prev = current->prev;
                if (current->prev != NULL)
                    current->prev->next = current->next;
            }
            free(current); // free the memory of the node
            return sockfd; // return the socket file descriptor of deleted node
        }
        current = current->next; // move to next node
    }
    return -1; // not found
}
int llist_to_string(struct Node* head, char res[200]) {
    struct Node* current = head;
    while (current != NULL) {
        sprintf(res, "%s%s,", current->name);
        current = current->next; // move to next node
    }
    return -1; // not found
}