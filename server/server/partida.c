//
// Created by antonia on 30/04/23.
//

#include "partida.h"

//
// Created by Sergio on 22/03/23.
//


/**
 * initialize_partidas_list - A function that initializes a ListaPartidas structure.
 *
 * @param list: Pointer to a ListaPartidas structure to be initialized.
 */
void initialize_partidas_list(ListaPartidas *list)
{
    list->idx = 0;
    list->used = 0;
    list->update_connecetions = 0;
    for (int i = 0; i < MAXPARTIDAS; i++)
    {
        for (int j=0;j<4;j++){
            list->partidas[i].player_pos[j].x=0.0f;
            list->partidas[i].player_pos[j].y=0.0f;
            list->partidas[i].enemy_pos[j].x=0.0f;
            list->partidas[i].enemy_pos[j].y=0.0f;
            list->partidas[i].sockets[j]=-1;
            list->partidas[i].vidas[j]=3;
            list->partidas[i].answer[j]=0;

        }
        list->partidas[i].jugando=0;
        list->partidas[i].idx=0;
    }
}

/**
 * reset_partida - A function that resets the values of a Partida structure.
 *
 * @param partida: Pointer to a Connection structure to be reset.
 */
void reset_partida(Partida *partida)
{
    for (int j=0;j<4;j++){
        partida->player_pos[j].x=0.0f;
        partida->player_pos[j].y=0.0f;
        partida->enemy_pos[j].x=0.0f;
        partida->enemy_pos[j].y=0.0f;
        partida->sockets[j]=-1;
        partida->vidas[j]=3;
        partida->answer[j]=0;
    }
    partida->jugando=0;
    partida->idx=0;
}



/**
 * get_empty_from_partidas_list - A function that returns the index of an empty slot in the ListaPartidas.
 *
 * @param list: Pointer to a ListaPartidas structure.
 * @return: Returns the index of an empty slot in the list, or -1 if the list is full.
 */
int get_empty_from_partidas_list(ListaPartidas *list)
{
    if (list->used == MAXPARTIDAS) // Check if list is full
    {
        return -1; // Return -1 if list is full
    }
    for (int i = 0; i < MAXPARTIDAS; i++) // Iterate through the connections array in the list
    {
        if (list->partidas[i].jugando == 0) // Check if connection is not in use
        {
            list->partidas[i].jugando = 1;       // Set using of the connection to 1, indicating it's in use
            list->partidas[i].idx = list->idx; // Set idx of the connection to list->idx
            list->idx++;                          // Increment idx in the list
            list->used++;                         // Increment used in the list
            return i;                             // Return the index of the empty slot in the list
        }
    }
    return -1; // Return -1 if no empty slot found in the list
}

/**
 * search_on_partidas_llist - A function that searches for a target_id in the ListaPartidas.
 *
 * @param list: Pointer to a ListaPartidas structure.
 * @param target_id: The target_id to search for in the list.
 * @return: Returns the index of the connection with matching target_id, or -1 if not found.
 */
int search_on_partidas_llist(ListaPartidas *list, int target_idx)
{
    for (int i = 0; i < MAXPARTIDAS; i++) // Iterate through the connections array in the list
    {
        if (list->partidas[i].jugando == 1 && list->partidas[i].idx == target_idx) // Check if connection is in use and has matching target_id
            return i;                                                                // Return the index of the connection with matching target_id
    }

    return -1; // Return -1 if target_id not found in the list
}

/**
 * remove_node_from_connected_list - A function that removes a connection from the ListaPartidas.
 *
 * @param list: Pointer to a ListaPartidas structure.
 * @param target_i: The index of the connection to be removed.
 * @return: Returns -1 if target_i is out of range, -2 if the connection at target_i is not in use,
 *          and 0 if the connection was successfully removed.
 */
int remove_node_from_partidas_list(ListaPartidas *list, int target_idx)
{
    if (target_idx > MAXPARTIDAS) // Check if target_i is out of range
        return -1;           // Return -1 if target_i is out of range

    if (list->partidas[target_idx].jugando == 0) // Check if the connection at target_i is not in use
        return -2;                              // Return -2 if the connection at target_i is not in use
    int i=search_on_partidas_llist(list,target_idx);
    reset_partida(&list->partidas[i]); // Reset the connection at target_i
    list->used--;                             // Decrement the used count in the list

    return 0; // Return 0 to indicate successful removal
}

/**
 * partidas_llist_to_string - A function that converts a ListaPartidas to a string representation.
 *
 * @param list: Pointer to a ListaPartidas structure.
 * @param res: The buffer to store the resulting string.
 * @return: Returns the total number of matches processed.
 */
int partidas_llist_to_string(ListaPartidas *list, char res[200])
{
    int i;
    for (i = 0; i < MAXPARTIDAS; i++)
    {
        if (list->partidas[i].jugando == 1)
            snprintf(res, 2000, "%s%d,", res, list->partidas[i].idx);
    }

    res[strlen(res) - 1] = '\0'; // Remove trailing comma
    return i;                    // Return the total number of connections processed
}

/**
 * print_connected_idx - A function that prints the indices of active matches in a ListaPartidas.
 *
 * @param list: Pointer to a ListaPartidas structure.
 */
void print_partidas_idx(ListaPartidas *list)
{
    char res[2000]; // Buffer to store the resulting string
    for (int i = 0; i < MAXPARTIDAS; i++)
    {
        if (list->partidas[i].jugando == 1)
        {
            snprintf(res, 2000, "%s%d,", res, list->partidas[i].idx); // Convert index to string and append to result buffer
        }
    }

    res[strlen(res) - 2] = '\0'; // Remove trailing comma
    printf("%s\n", res);         // Print the resulting string
}

void Thread_Partida(void * PartidasArgs){

}
