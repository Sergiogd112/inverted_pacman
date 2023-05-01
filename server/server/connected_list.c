//
// Created by Sergio on 22/03/23.
//

#include "connected_list.h"

/**
 * initialize_connected_list - A function that initializes a ConnectedList structure.
 *
 * @param list: Pointer to a ConnectedList structure to be initialized.
 */
void initialize_connected_list(ConnectedList *list)
{
    list->idx = 0;                 // Initialize idx to 0
    list->used = 0;                // Initialize used to 0
    list->update_connecetions = 0; // Initialize update_connecetions to 0
    for (int i = 0; i < MAXUSERS; i++)
    {
        list->connections[i].id = -1;               // Initialize id to -1
        list->connections[i].sockfd = 0;            // Initialize sockfd to 0
        list->connections[i].using = 0;             // Initialize using to 0
        list->connections[i].jugando = 0;             // Initialize jugando to 0
        list->connections[i].invitando = 0;             // Initialize invitando to 0
        list->connections[i].sending_connected = 0; // Initialize sending_connected to 0
        strcpy(list->connections[i].name, "name");  // Initialize name to "name"
        list->connections[i].idx = -1;              // Initialize idx to -1
    }
}

/**
 * reset_node - A function that resets the values of a Connection structure.
 *
 * @param node: Pointer to a Connection structure to be reset.
 */
void reset_node(struct Connection *node)
{
    node->id = -1;               // Reset id to -1
    node->sockfd = 0;            // Reset sockfd to 0
    node->using = 0;             // Reset using to 0
    node->jugando = 0;             // Reset jugando to 0
    node->invitando = 0;             // Reset invitando to 0
    node->sending_connected = 0; // Reset sending_connected to 0
    strcpy(node->name, "name");  // Reset name to "name"
    node->idx = -1;              // Reset idx to -1
}

/**
 * insert_to_connected_llist - A function that inserts a new node with given values into a ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param new_id: Integer value of the new node's id.
 * @param new_sockfd: Integer value of the new node's sockfd.
 * @param name: Character array containing the name for the new node.
 * @return: Returns the index of the newly inserted node in the list, or -1 if the list is full.
 */
int insert_to_connected_llist(ConnectedList *list, int new_id, int new_sockfd, char name[20])
{
    if (list->used == MAXUSERS) // Check if list is full
    {
        return -1; // Return -1 if list is full
    }
    for (int i = 0; i < MAXUSERS; i++) // Iterate through the connections array in the list
    {
        if (list->connections[i].using == 0) // Check if connection is not in use
        {
            list->connections[i].id = new_id;         // Set id of the connection to new_id
            list->connections[i].sockfd = new_sockfd; // Set sockfd of the connection to new_sockfd
            strcpy(list->connections[i].name, name);  // Copy name to the name field of the connection
            list->connections[i].idx = list->idx;     // Set idx of the connection to list->idx
            list->connections[i].using = 1;           // Set using of the connection to 1, indicating it's in use
            list->idx++;                              // Increment idx in the list
            list->used++;                             // Increment used in the list
            return i;                                 // Return the index of the newly inserted node in the list
        }
    }
    return -1; // Return -1 if no free slot found in the list
}

/**
 * get_empty_from_connected_list - A function that returns the index of an empty slot in the ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @return: Returns the index of an empty slot in the list, or -1 if the list is full.
 */
int get_empty_from_connected_list(ConnectedList *list)
{
    if (list->used == MAXUSERS) // Check if list is full
    {
        return -1; // Return -1 if list is full
    }
    for (int i = 0; i < MAXUSERS; i++) // Iterate through the connections array in the list
    {
        if (list->connections[i].using == 0) // Check if connection is not in use
        {
            list->connections[i].using = 1;       // Set using of the connection to 1, indicating it's in use
            list->connections[i].idx = list->idx; // Set idx of the connection to list->idx
            list->idx++;                          // Increment idx in the list
            list->used++;                         // Increment used in the list
            return i;                             // Return the index of the empty slot in the list
        }
    }
    return -1; // Return -1 if no empty slot found in the list
}
//// append a node at the end of the list
// void append_to_llist(struct Connection **head_ref, struct Connection **new_node_ref) {
//     // allocate memory for new node
//     struct Connection *head = *head_ref;
//
//     struct Connection *last = *head_ref;
//     struct Connection *new_node = *new_node_ref;
//     if (last != NULL) {
//         while (last->next != NULL)
//             last = last->next;
//         last->next = new_node;
//         new_node->prev = last;
//     } else {
//         // if the list is empty, make new node as head and prev as NULL
//         head = new_node;
//         new_node->prev = NULL;
//     }
// }

/**
 * search_id_on_connected_llist - A function that searches for a target_id in the ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param target_id: The target_id to search for in the list.
 * @return: Returns the index of the connection with matching target_id, or -1 if not found.
 */
int search_id_on_connected_llist(ConnectedList *list, int target_id)
{
    for (int i = 0; i < MAXUSERS; i++) // Iterate through the connections array in the list
    {
        if (list->connections[i].using == 1 && list->connections[i].id == target_id) // Check if connection is in use and has matching target_id
            return i;                                                                // Return the index of the connection with matching target_id
    }

    return -1; // Return -1 if target_id not found in the list
}
/**
 * search_name_on_connected_llist - A function that searches for a target_id in the ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param nombre: The name to search for in the list.
 * @return: Returns the index of the connection with matching target_id, or -1 if not found.
 */
int search_name_on_connected_llist(ConnectedList *list, Nombre nombre)
{
    for (int i = 0; i < MAXUSERS; i++) // Iterate through the connections array in the list
    {
        if (list->connections[i].using == 1 && strcmp(list->connections[i].name,nombre)==0) // Check if connection is in use and has matching target_id
            return i;                                                                // Return the index of the connection with matching target_id
    }

    return -1; // Return -1 if target_id not found in the list
}

/**
 * remove_node_from_connected_list - A function that removes a connection from the ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param target_i: The index of the connection to be removed.
 * @return: Returns -1 if target_i is out of range, -2 if the connection at target_i is not in use,
 *          and 0 if the connection was successfully removed.
 */
int remove_node_from_connected_list(ConnectedList *list, int target_i)
{
    if (target_i > MAXUSERS) // Check if target_i is out of range
        return -1;           // Return -1 if target_i is out of range

    if (list->connections[target_i].using == 0) // Check if the connection at target_i is not in use
        return -2;                              // Return -2 if the connection at target_i is not in use

    int err = write(list->connections[target_i].sockfd, "checking connection", strlen("checking connection"));
    // Write to the sockfd of the connection at target_i to check if it's still valid

    if (err != -1)                                 // Check if write was successful
        close(list->connections[target_i].sockfd); // Close the sockfd of the connection at target_i

    reset_node(&list->connections[target_i]); // Reset the connection at target_i
    list->used--;                             // Decrement the used count in the list

    return 0; // Return 0 to indicate successful removal
}

/**
 * connected_llist_to_string - A function that converts a ConnectedList to a string representation.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param res: The buffer to store the resulting string.
 * @return: Returns the total number of connections processed.
 */
int connected_llist_to_string(ConnectedList *list, char res[200])
{
    int i;
    for (i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1)
            snprintf(res, 2000, "%s%d,", res, list->connections[i].idx);
    }

    res[strlen(res) - 1] = '\0'; // Remove trailing comma
    return i;                    // Return the total number of connections processed
}

/**
 * print_connected_idx - A function that prints the indices of active connections in a ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 */
void print_connected_idx(ConnectedList *list)
{
    char res[2000]; // Buffer to store the resulting string
    for (int i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1)
        {
            print_Node(&list->connections[i]);                           // Assuming print_Node is a valid function to print a Node
            snprintf(res, 2000, "%s%d,", res, list->connections[i].idx); // Convert index to string and append to result buffer
        }
    }

    res[strlen(res) - 2] = '\0'; // Remove trailing comma
    printf("%s\n", res);         // Print the resulting string
}

/**
 * connected_to_string - A function that converts active connections in a ConnectedList to a string.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param res: Pointer to a character buffer to store the resulting string.
 * @param maxlen: Maximum length of the resulting string buffer.
 * @return: Number of active connections converted to string.
 */
int connected_to_string(ConnectedList *list, char *res, size_t maxlen)
{
    int i;
    int connected = 0;            // Counter for number of active connections
    size_t count = 0;             // Counter for total characters written to the resulting string buffer
    size_t available = maxlen;    // Remaining available space in the resulting string buffer
    memset(res, '\0', available); // Initialize the resulting string buffer with null characters

    for (i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1 && list->connections[i].id != -1)
        {
            size_t written = snprintf(&(res[count]), available, "%s,", list->connections[i].name); // Convert name to string and append to result buffer
            available -= written;                                                                  // Update remaining available space in the resulting string buffer
            if (available < 1)
            {
                break; // Break loop if the resulting string buffer is full
            }
            count += written; // Update total characters written
            connected++;      // Increment counter for active connections
        }
    }

    res[count - 1] = '\0'; // Remove trailing comma by null-terminating the resulting string buffer
    return connected;      // Return number of active connections converted to string
}

/**
 * push_connected - A function that pushes a list of connected users as a string to all active connections in a ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 * @param res: Pointer to a character buffer containing the list of connected users as a string.
 * @param n: Number of connected users.
 */
void push_connected(ConnectedList *list, char res[2000], int n)
{
    char response[2010];                             // Buffer to hold the formatted response string
    snprintf(response, 2010, "4/%d/%s\x04", n, res); // Format the response string

    printf("%s\n", response); // Print the response string

    for (int i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1 && list->connections[i].id != -1)
        {
            list->connections[i].sending_connected = 1;                     // Set sending_connected flag to indicate a message is being sent
            write(list->connections[i].sockfd, response, strlen(response)); // Write the response string to the socket
            list->connections[i].sending_connected = 0;                     // Reset sending_connected flag after sending the message
        }

        snprintf(res, 2000, "%s%s,", res, list->connections[i].name); // Update the list of connected users string
    }
}

/**
 * print_Node - A function that prints the details of a Connection structure.
 *
 * @param node: Pointer to a Connection structure.
 */
void print_Node(struct Connection *node)
{
    printf("---\n"); // Print a separator line
    if (node->using == 0)
        printf("Not using"); // Print a message if the connection is not in use
    else
        printf("id: %d\nsocketfd: %d\nname: %s\nidx: %d\nsending_connected: %d\njugando %d\n",
               node->id,                 // Print the 'id' field of the Connection structure
               node->sockfd,             // Print the 'sockfd' field of the Connection structure
               node->name,               // Print the 'name' field of the Connection structure
               node->idx,                // Print the 'idx' field of the Connection structure
               node->sending_connected,  // Print the 'sending_connected' field of the Connection structure
               node->jugando);
    printf("---\n");                     // Print a separator line
}

/**
 * print_Connections - A function that prints the details of all active Connection structures in a ConnectedList.
 *
 * @param list: Pointer to a ConnectedList structure.
 */
void print_Connections(ConnectedList *list)
{
    for (int i = 0; i < MAXUSERS; i++)
    {
        if (list->connections[i].using == 1 && list->connections[i].id != -1)
        {
            print_Node(&list->connections[i]); // Call print_Node to print the details of each active Connection structure
        }
    }
}
