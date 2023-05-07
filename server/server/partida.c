//
// Created by antonia on 30/04/23.
//

#include "partida.h"

//
// Created by Sergio on 22/03/23.
//

int sum(int *arr, int len) {
    int total = 0;
    for (int i = 0; i < len; i++)
        total += arr[i];
    return total;
}

/**
 * initialize_partidas_list - A function that initializes a ListaPartidas structure.
 *
 * @param list: Pointer to a ListaPartidas structure to be initialized.
 */
void initialize_partidas_list(ListaPartidas *list) {
    list->idx = 0;
    list->used = 0;
    list->update_connecetions = 0;
    for (int i = 0; i < MAXPARTIDAS; i++) {
        for (int j = 0; j < 4; j++) {
            list->partidas[i].player_pos[j].x = 0.0f;
            list->partidas[i].player_pos[j].y = 0.0f;
            list->partidas[i].enemys[j].pos.x = 0.0f;
            list->partidas[i].enemys[j].pos.y = 0.0f;
            list->partidas[i].sockets[j] = -1;
            list->partidas[i].vidas[j] = 3;
            list->partidas[i].answer[j] = 0;
            list->partidas[i].listos[j] = 0;

        }
        list->partidas[i].jugando = 0;
        pthread_mutex_init(&list->partidas[i].mutex, NULL);
        list->partidas[i].usando = 0;
        list->partidas[i].idx = 0;
        list->partidas[i].kill = 0;
    }
}

/**
 * reset_partida - A function that resets the values of a Partida structure.
 *
 * @param partida: Pointer to a Connection structure to be reset.
 */
void reset_partida(Partida *partida) {
    for (int j = 0; j < 4; j++) {
        partida->player_pos[j].x = 0.0f;
        partida->player_pos[j].y = 0.0f;
        partida->enemys[j].pos.x = 0.0f;
        partida->enemys[j].pos.y = 0.0f;
        partida->sockets[j] = -1;
        partida->vidas[j] = 3;
        partida->answer[j] = 0;
        partida->listos[j] = 0;
    }
    partida->jugando = 0;
    partida->usando = 0;
    partida->idx = 0;
    partida->kill = 0;
}


/**
 * get_empty_from_partidas_list - A function that returns the index of an empty slot in the ListaPartidas.
 *
 * @param list: Pointer to a ListaPartidas structure.
 * @return: Returns the index of an empty slot in the list, or -1 if the list is full.
 */
int get_empty_from_partidas_list(ListaPartidas *list) {
    if (list->used == MAXPARTIDAS) // Check if list is full
    {
        return -1; // Return -1 if list is full
    }
    for (int i = 0; i < MAXPARTIDAS; i++) // Iterate through the connections array in the list
    {
        if (list->partidas[i].usando == 0) // Check if connection is not in use
        {
            list->partidas[i].usando = 1;       // Set using of the connection to 1, indicating it's in use
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
int search_on_partidas_llist(ListaPartidas *list, int target_idx) {
    for (int i = 0; i < MAXPARTIDAS; i++) // Iterate through the connections array in the list
    {
        if (list->partidas[i].usando == 1 &&
            list->partidas[i].idx == target_idx) // Check if connection is in use and has matching target_id
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
int remove_node_from_partidas_list(ListaPartidas *list, int target_idx) {
    if (target_idx > MAXPARTIDAS) // Check if target_i is out of range
        return -1;           // Return -1 if target_i is out of range

    if (list->partidas[target_idx].usando == 0) // Check if the connection at target_i is not in use
        return -2;                              // Return -2 if the connection at target_i is not in use
    int i = search_on_partidas_llist(list, target_idx);
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
int partidas_llist_to_string(ListaPartidas *list, char res[200]) {
    int i;
    for (i = 0; i < MAXPARTIDAS; i++) {
        if (list->partidas[i].usando == 1)
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
void print_partidas_idx(ListaPartidas *list) {
    char res[2000]; // Buffer to store the resulting string
    for (int i = 0; i < MAXPARTIDAS; i++) {
        if (list->partidas[i].jugando == 1) {
            snprintf(res, 2000, "%s%d,", res,
                     list->partidas[i].idx); // Convert index to string and append to result buffer
        }
    }

    res[strlen(res) - 2] = '\0'; // Remove trailing comma
    printf("%s\n", res);         // Print the resulting string
}

int i_jugador_partida(Partida *partida, Nombre nombre) {
    for (int i = 0; i < 4; i++) {
        if (strcmp(partida->nombres[i], nombre) == 0)
            return i;
    }
    return -1;
}

int mensage_to_jugadores(Partida *partida, char mesg[512]) {
    char *p;
    for (int i = 0; i < 4; i++) {
        p = strtok(mesg, "*");//extract name
        if (strcmp(partida->nombres[i], p) != 0)
            return 1;
        p = strtok(mesg, "*");//extract x
        partida->player_pos[i].x = atof(p);
        p = strtok(mesg, "*");//extract y
        partida->player_pos[i].y = atof(p);
        p = strtok(mesg, "*");//extract points
        partida->puntos[i] = atoi(p);
        p = strtok(mesg, "*");//extract lifes
        partida->vidas[i] = atoi(p);

    }
    return 0;
}

int mensage_to_enemigos(Partida *partida, char mesg[512]) {
    char *p;
    for (int i = 0; i < 4; i++) {
        p = strtok(mesg, "*");//extract id
        partida->enemys[i].id = atoi(p);
        p = strtok(mesg, "*");//extract x
        partida->enemys[i].pos.x = atof(p);
        p = strtok(mesg, "*");//extract y
        partida->enemys[i].pos.y = atof(p);

    }
    return 0;
}

int mesage_to_partida(Partida *partida, char mesg[512]) {
    char *p = strtok(mesg, "|");
    int n = mensage_to_jugadores(partida, p);
    p = strtok(mesg, "|");
    int m = mensage_to_enemigos(partida, p);
    return n + m * 2;
}

int get_enemy_with_id(Partida *partida, int id) {
    for (int i = 0; i < 4; i++)
        if (partida->enemys[i].id == id)
            return i;
    return -1;
}

void Atender_Cliente_Partida(Partida *partida, Nombre nombre) {
    int ret;
    int ij = i_jugador_partida(partida, nombre);
    int sock_conn = partida->sockets[ij];
    char request[512];
    int vacios = 0;
    int code;
    int scode;
    int sscode;
    while (sum(partida->vidas, 4) > 0) {
        ret = read(sock_conn, request, sizeof(request));
        if (ret <= 0) {
            vacios++;
            continue;
        }
        request[ret] = '\0';
        printf("Conexion %s ha mandado: %s\n", nombre, request);
        char *p = strtok(request, "/");

        code = atoi(p);
        if (code != 8)
            continue;
        p = strtok(NULL, "/");

        scode = atoi(p);
        p = strtok(NULL, "/");

        sscode = atoi(p);
        switch (scode) {
            case 0:
                if (sscode == 1) {
                    pthread_mutex_lock(&partida->mutex);
                    partida->listos[ij] = 1;
                    pthread_mutex_unlock(&partida->mutex);
                } else if (sscode == 1 && ij == 0) {
                    p = strtok(NULL, "/");
                    mesage_to_partida(partida, p);
                    pthread_mutex_lock(&partida->mutex);
                    partida->listos[ij] = 1;
                    pthread_mutex_unlock(&partida->mutex);
                }

                break;
            case 1:
                switch (sscode) {
                    case 0:
                        pthread_mutex_lock(&partida->mutex);
                        p = strtok(NULL, "*");
                        if (strcmp(nombre, p) == 0) {
                            p = strtok(p, "*");//extract x
                            partida->player_pos[ij].x = atof(p);
                            p = strtok(p, "*");//extract y
                            partida->player_pos[ij].y = atof(p);
                        }
                        pthread_mutex_unlock(&partida->mutex);
                        break;
                    case 1:
                        pthread_mutex_lock(&partida->mutex);
                        p = strtok(NULL, "*");
                        if (strcmp(nombre, p) == 0) {
                            p = strtok(p, "*");//extract id
                            partida->enemys[get_enemy_with_id(partida, atoi(p))].id = -1;
                            p = strtok(p, "*");//extract p
                            partida->puntos[ij] = atof(p);
                        }
                        pthread_mutex_unlock(&partida->mutex);
                        for (int i = 0; i < 4; i++)
                            if (i != ij)
                                write(sock_conn, request, strlen(request));
                        break;
                    case 2:
                        pthread_mutex_lock(&partida->mutex);
                        p = strtok(NULL, "*");
                        if (strcmp(nombre, p) == 0) {
                            p = strtok(p, "*");//extract id
                            partida->vidas[ij]--;
                        }
                        pthread_mutex_unlock(&partida->mutex);
                        for (int i = 0; i < 4; i++)
                            if (i != ij)
                                write(sock_conn, request, strlen(request));
                        break;
                    case 3:
                        pthread_mutex_lock(&partida->mutex);
                        p = strtok(NULL, "*");
                        if (strcmp(nombre, p) == 0) {
                            p = strtok(p, "*");//extract id
                            partida->vidas[ij] = 0;
                        }
                        pthread_mutex_unlock(&partida->mutex);
                        for (int i = 0; i < 4; i++)
                            if (i != ij)
                                write(sock_conn, request, strlen(request));
                        break;
                    default:
                        break;
                }
                break;
            case 2:
                if (ij == 0) {
                    pthread_mutex_lock(&partida->mutex);
                    if (sscode == 0) {
                        p = strtok(NULL, "/");
                        mensage_to_enemigos(partida, p);
                        pthread_mutex_unlock(&partida->mutex);

                    } else {
                        int ie = get_enemy_with_id(partida, -1);

                        if (ie >= 0 && ie < 4) {
                            p = strtok(NULL, "*");
                            partida->enemys[ie].pos.x = atof(p);
                            p = strtok(NULL, "*");
                            partida->enemys[ie].pos.y = atof(p);
                            p = strtok(NULL, "*");
                            partida->enemys[ie].id = atoi(p);
                        }
                        pthread_mutex_unlock(&partida->mutex);
                        for (int i = 0; i < 4; i++)
                            if (i != ij)
                                write(sock_conn, request, strlen(request));
                    }

                }
                break;
            default:
                break;
        }
    }
}
char * server_mesg_1(Partida *partida){
    char res[300]
}
void Partida_Thread(void *args) {
    PartidaArgs *partidaArgs = (PartidaArgs *) args;
    Partida *partida = partidaArgs->partida;
    ListaPartidas *listaPartidas = partidaArgs->listaPartidas;
    int sumres=0;
    while (sumres < 4) {
        pthread_mutex_lock(&partida->mutex);
        usleep(100000);
        sumres=sum(partida->listos, 4);
        pthread_mutex_unlock(&partida->mutex);
    }
    pthread_mutex_lock(&partida->mutex);
    sumres=sum(partida->vidas, 4);
    pthread_mutex_unlock(&partida->mutex);

    while (sumres > 0) {
        pthread_mutex_lock(&partida->mutex);
        for (int i = 0; i < 4; i++) {
            char msg;
            write(partida->sockets[i],);
        }
        pthread_mutex_unlock(&partida->mutex);

    }
}