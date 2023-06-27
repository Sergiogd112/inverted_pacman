//
// Created by antonia on 30/04/23.
//

#include "partida.h"

//
// Created by Sergio on 22/03/23.
//

int sum(int *arr, int len)
{
    int total = 0;
    for (int i = 0; i < len; i++)
        total += arr[i];
    return total;
}

/**
 * initialize_partidas_list - A function that initializes a ListaPartidas structure.
 * @param list: Pointer to a ListaPartidas structure to be initialized.
 */
void initialize_partidas_list(ListaPartidas *list)
{
    list->idx = 0;
    list->used = 0;
    list->update_connecetions = 0;
    for (int i = 0; i < MAXPARTIDAS; i++)
    {
        for (int j = 0; j < NJUGADORESPARTIDA; j++)
        {
            list->partidas[i].player_pos[j].x = 0.0f;
            list->partidas[i].player_pos[j].y = 0.0f;
            list->partidas[i].enemys[j].pos.x = 0.0f;
            list->partidas[i].enemys[j].pos.y = 0.0f;
            list->partidas[i].sockets[j] = -1;
            list->partidas[i].vidas[j] = 3;
            list->partidas[i].answer[j] = 0;
            list->partidas[i].listos[j] = 0;
            list->partidas[i].disconnected[j] = 0;
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
 * @param partida: Pointer to a Connection structure to be reset.
 */
void reset_partida(Partida *partida)
{
    for (int j = 0; j < NJUGADORESPARTIDA; j++)
    {
        partida->player_pos[j].x = 0.0f;
        partida->player_pos[j].y = 0.0f;
        partida->enemys[j].pos.x = 0.0f;
        partida->enemys[j].pos.y = 0.0f;
        partida->sockets[j] = -1;
        partida->vidas[j] = 3;
        partida->answer[j] = 0;
        partida->listos[j] = 0;
        partida->disconnected[j] = 0;
    }
    partida->jugando = 0;
    partida->usando = 0;
    partida->idx = 0;
    partida->kill = 0;
}

/**
 * get_empty_from_partidas_list - A function that returns the index of an empty slot in the ListaPartidas.
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
        if (list->partidas[i].usando == 0) // Check if connection is not in use
        {
            list->partidas[i].usando = 1;      // Set using of the connection to 1, indicating it's in use
            list->partidas[i].idx = list->idx; // Set idx of the connection to list->idx
            list->idx++;                       // Increment idx in the list
            list->used++;                      // Increment used in the list
            return i;                          // Return the index of the empty slot in the list
        }
    }
    return -1; // Return -1 if no empty slot found in the list
}

/**
 * search_on_partidas_llist - A function that searches for a target_id in the ListaPartidas.
 * @param list: Pointer to a ListaPartidas structure.
 * @param target_id: The target_id to search for in the list.
 * @return: Returns the index of the connection with matching target_id, or -1 if not found.
 */
int search_on_partidas_llist(ListaPartidas *list, int target_idx)
{
    for (int i = 0; i < MAXPARTIDAS; i++) // Iterate through the connections array in the list
    {
        if (list->partidas[i].usando == 1 &&
            list->partidas[i].idx == target_idx) // Check if connection is in use and has matching target_id
            return i;                            // Return the index of the connection with matching target_id
    }

    return -1; // Return -1 if target_id not found in the list
}

/**
 * remove_node_from_connected_list - A function that removes a connection from the ListaPartidas.
 * @param list: Pointer to a ListaPartidas structure.
 * @param target_i: The index of the connection to be removed.
 * @return: Returns -1 if target_i is out of range, -2 if the connection at target_i is not in use,and 0 if the connection was successfully removed.
 */
int remove_node_from_partidas_list(ListaPartidas *list, int target_idx)
{
    if (target_idx > MAXPARTIDAS) // Check if target_i is out of range
        return -1;                // Return -1 if target_i is out of range

    if (list->partidas[target_idx].usando == 0) // Check if the connection at target_i is not in use
        return -2;                              // Return -2 if the connection at target_i is not in use
    int i = search_on_partidas_llist(list, target_idx);
    reset_partida(&list->partidas[i]); // Reset the connection at target_i
    list->used--;                      // Decrement the used count in the list

    return 0; // Return 0 to indicate successful removal
}

/**
 * partidas_llist_to_string - A function that converts a ListaPartidas to a string representation.
 * @param list: Pointer to a ListaPartidas structure.
 * @param res: The buffer to store the resulting string.
 * @return: Returns the total number of matches processed.
 */
int partidas_llist_to_string(ListaPartidas *list, char res[200])
{
    int i;
    for (i = 0; i < MAXPARTIDAS; i++)
    {
        if (list->partidas[i].usando == 1)
            snprintf(res, 2000, "%s%d,", res, list->partidas[i].idx);
    }

    res[strlen(res) - 1] = '\0'; // Remove trailing comma
    return i;                    // Return the total number of connections processed
}

/**
 * print_connected_idx - A function that prints the indices of active matches in a ListaPartidas.
 * @param list: Pointer to a ListaPartidas structure.
 */
void print_partidas_idx(ListaPartidas *list)
{
    char res[2000]; // Buffer to store the resulting string
    for (int i = 0; i < MAXPARTIDAS; i++)
    {
        if (list->partidas[i].jugando == 1)
        {
            snprintf(res, 2000, "%s%d,", res,
                     list->partidas[i].idx); // Convert index to string and append to result buffer
        }
    }

    res[strlen(res) - 2] = '\0'; // Remove trailing comma
    logger(LOGINFO, res);        // Print the resulting string
}

/**
 * Finds the index of a player in the game session based on their name.
 * @param partida A pointer to the Partida object.
 * @param nombre The name of the player.
 * @return The index of the player if found, or -1 if not found.
 */
int i_player_partida(Partida *partida, Nombre nombre)
{
    char logmsg[100];
    snprintf(logmsg, 100, "Buscando jugador %s en partida %d", nombre, partida->idx);
    logger(LOGINFO, logmsg);
    for (int i = 0; i < 4; i++)
    {
        if (strcmp(partida->nombres[i], nombre) == 0)
            return i;
    }
    return -1;
}

/**
 * Processes a message for the players in the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param mesg The message to be processed.
 * @return 0 on success, or 1 if the message is invalid.
 */
int message_to_players(Partida *partida, char mesg[512])
{
    char *p;
    for (int i = 0; i < NJUGADORESPARTIDA; i++)
    {
        p = strtok(mesg, "*"); // Extract name
        if (strcmp(partida->nombres[i], p) != 0)
            return 1;
        p = strtok(mesg, "*"); // Extract x
        partida->player_pos[i].x = atof(p);
        p = strtok(mesg, "*"); // Extract y
        partida->player_pos[i].y = atof(p);
        p = strtok(mesg, "*"); // Extract points
        partida->puntos[i] = atoi(p);
        p = strtok(mesg, "*"); // Extract lifes
        partida->vidas[i] = atoi(p);
    }
    return 0;
}

/**
 * Processes a message for the enemies in the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param mesg The message to be processed.
 * @return 0 on success.
 */
int message_to_enemies(Partida *partida, char mesg[512])
{
    char *p;
    for (int i = 0; i < 4; i++)
    {
        p = strtok(mesg, "*"); // Extract id
        partida->enemys[i].id = atoi(p);
        p = strtok(mesg, "*"); // Extract x
        partida->enemys[i].pos.x = atof(p);
        p = strtok(mesg, "*"); // Extract y
        partida->enemys[i].pos.y = atof(p);
    }
    return 0;
}

/**
 * Processes a message for the game session, including players and enemies. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param mesg The message to be processed.
 * @return 0 on success, or a combination of error codes if the message is invalid.
 */
int message_to_partida(Partida *partida, char mesg[512])
{
    char *p = strtok(mesg, "|");
    int n = message_to_players(partida, p);
    p = strtok(mesg, "|");
    int m = message_to_enemies(partida, p);
    return n + m * 2;
}

/**
 * Finds the index of an enemy in the game session based on its ID.
 * @param partida A pointer to the Partida object.
 * @param id The ID of the enemy.
 * @return The index of the enemy if found, or -1 if not found
 */
int get_enemy_with_id(Partida *partida, int id)
{
    for (int i = 0; i < 4; i++)
        if (partida->enemys[i].id == id)
            return i;
    return -1;
}

/**
 * Handles the client's requests in the game session.
 * @param partida A pointer to the Partida object.
 * @param nombre The name of the client.
 */
void Atender_Cliente_Partida(Partida *partida, Nombre nombre, MYSQL *conn)
{
    int ret;
    int ij = i_player_partida(partida, nombre); // Get the index of the player in the game session.
    int sock_conn = partida->sockets[ij];       // Get the socket connection for the player.
    char request[512];
    int vacios = 0;
    int code;
    int scode;
    int sscode;
    char logmsg[2000];
    snprintf(logmsg, 2000, "Atendiendo cliente %s en partida %d", nombre, partida->idx);
    logger(LOGINFO, logmsg);
    while (sum(partida->vidas, NJUGADORESPARTIDA) > 0)
    { // Continue processing while there are still lives left in the game.
        ret = read(sock_conn, request, sizeof(request));
        if (ret <= 0)
        {
            vacios++;
            continue;
        }
        request[ret] = '\0';
        // Extract codes from the request.
        char *p = strtok(request, "/");

        code = atoi(p);
        if (code != 8)
            continue;
        p = strtok(NULL, "/");

        scode = atoi(p);
        p = strtok(NULL, "/");

        sscode = atoi(p);
        switch (scode)
        {
        case 0:
            if (sscode == 1)
            {
                pthread_mutex_lock(&partida->mutex);
                partida->listos[ij] = 1;
                pthread_mutex_unlock(&partida->mutex);
            }
            else if (sscode == 0 && ij == 0)
            {
                p = strtok(NULL, "/");
                message_to_partida(partida, p);
                pthread_mutex_lock(&partida->mutex);
                partida->listos[ij] = 1;
                pthread_mutex_unlock(&partida->mutex);
            }

            break;
        case 1:
            switch (sscode)
            {
            case 0:
                pthread_mutex_lock(&partida->mutex);
                p = strtok(NULL, "*");
                if (strcmp(nombre, p) == 0)
                {
                    p = strtok(p, "*"); // extract x
                    partida->player_pos[ij].x = atof(p);
                    p = strtok(p, "*"); // extract y
                    partida->player_pos[ij].y = atof(p);
                }
                pthread_mutex_unlock(&partida->mutex);
                break;
            case 1:
                pthread_mutex_lock(&partida->mutex);
                p = strtok(NULL, "*");
                if (strcmp(nombre, p) == 0)
                {
                    p = strtok(p, "*"); // extract id
                    partida->enemys[get_enemy_with_id(partida, atoi(p))].id = -1;
                    p = strtok(p, "*"); // extract p
                    partida->puntos[ij] = atof(p);
                }
                pthread_mutex_unlock(&partida->mutex);
                for (int i = 0; i < NJUGADORESPARTIDA; i++)
                    if (i != ij)
                        write(sock_conn, request, strlen(request));
                break;
            case 2:
                pthread_mutex_lock(&partida->mutex);
                p = strtok(NULL, "*");
                if (strcmp(nombre, p) == 0)
                {
                    p = strtok(p, "*"); // extract id
                    partida->vidas[ij]--;
                }
                pthread_mutex_unlock(&partida->mutex);
                for (int i = 0; i < NJUGADORESPARTIDA; i++)
                    if (i != ij)
                        write(sock_conn, request, strlen(request));
                break;
            case 3:
                pthread_mutex_lock(&partida->mutex);
                p = strtok(NULL, "*");
                if (strcmp(nombre, p) == 0)
                {
                    p = strtok(p, "*"); // extract id
                    partida->vidas[ij] = 0;
                }
                pthread_mutex_unlock(&partida->mutex);
                for (int i = 0; i < NJUGADORESPARTIDA; i++)
                    if (i != ij)
                        write(sock_conn, request, strlen(request));
                break;
            default:
                break;
            }
            break;
        case 2:
            if (ij == 0)
            {
                pthread_mutex_lock(&partida->mutex);
                if (sscode == 0)
                {
                    p = strtok(NULL, "/");
                    message_to_enemies(partida, p);
                    pthread_mutex_unlock(&partida->mutex);
                }
                else
                {
                    int ie = get_enemy_with_id(partida, -1);

                    if (ie >= 0 && ie < 4)
                    {
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
        case 3:
            if (ij == 0)
            {
                for (int i = 0; i < NJUGADORESPARTIDA; i++)
                    if (i != ij)
                        write(sock_conn, request, strlen(request));
                int ag = add_game_to_db(conn, partida);
            }
            return;
            break;
        default:
            break;
        }
    }
    snprintf(logmsg, 300, "El jugador %s ha abandonado la partida", nombre);
    logger(LOGINFO, logmsg);
}

/**
 * Generates a server message containing player information. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param res The buffer to store the generated message.
 * @return 0 indicating success.
 */
int server_msg_1(Partida *partida, char res[300])
{

    pthread_mutex_lock(&partida->mutex);
    // Iterate over each player in the partida and generate a formatted string.
    for (int i = 0; i < NJUGADORESPARTIDA; i++)
    {
        snprintf(res, 300, "%s*%f*%f*%d*%d,", partida->nombres[i], partida->player_pos[i].x, partida->player_pos[i].y,
                 partida->puntos[i], partida->vidas[i]);
    }
    pthread_mutex_unlock(&partida->mutex);
    res[strlen(res) - 2] = '\0'; // Remove the trailing comma from the generated string.
    return 0;
}

/**
 * Generates a server message containing enemy information. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param res The buffer to store the generated message.
 * @return 0 indicating success.
 */
int server_msg_2(Partida *partida, char res[300])
{

    pthread_mutex_lock(&partida->mutex);
    // Iterate over each enemy in the partida and generate a formatted string.
    for (int i = 0; i < NJUGADORESPARTIDA; i++)
    {
        snprintf(res, 300, "%d*%f*%f,", partida->enemys[i].id, partida->enemys[i].pos.x, partida->enemys[i].pos.y);
    }
    pthread_mutex_unlock(&partida->mutex);
    res[strlen(res) - 2] = '\0'; // Remove the trailing comma from the generated string.
    return 0;
}

/**
 * Generates a combined server message containing player and enemy information. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param res The buffer to store the generated message.
 * @return The sum of return values from server_msg_1 and server_msg_2.
 */
int server_msg_0(Partida *partida, char res[600])
{
    char res1[300];
    char res2[300];
    // Generate player and enemy messages separately.
    int n = server_msg_1(partida, res1);
    int m = server_msg_2(partida, res2);
    // Combine the player and enemy messages.
    snprintf(res, 600, "%s|%s", res1, res2);
    return n + m * 2;
}

/**
 * Sends a message to all players in the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @param msg The message to be sent.
 * @param len The length of the message.
 */
void send_to_all(Partida *partida, char *msg, int len)
{
    for (int i = 0; i < NJUGADORESPARTIDA; i++)
        write(partida->sockets[i], msg, len);
}

/**
 * Sends a server message 0 to all players in the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @return 0 indicating success.
 */
int send_0(Partida *partida)
{
    char data0[600];
    server_msg_0(partida, data0); // Get data for signal 0 from the server.
    char msg0[620];
    snprintf(msg0, 620, "8/0/%s", data0);     // Format the message to be sent.
    send_to_all(partida, msg0, strlen(msg0)); // Send the message to all players.
    return 0;
}

/**
 * Sends signal 1 to the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @return 0 on success.
 */
int send_1(Partida *partida)
{
    char data1[300];
    server_msg_1(partida, data1); // Get data for signal 1 from the server.
    char msg1[320];
    snprintf(msg1, 320, "8/0/%s", data1);     // Format the message to be sent.
    send_to_all(partida, msg1, strlen(msg1)); // Send the message to all players.
    return 0;
}

/**
 * Sends signal 2 to the game session. The format of the message is in the file: Mensajes.md.
 * @param partida A pointer to the Partida object.
 * @return 0 on success.
 */
int send_2(Partida *partida)
{
    char data2[300];
    server_msg_0(partida, data2); // Get data for signal 2 from the server.
    char msg2[320];
    snprintf(msg2, 320, "8/0/%s", data2);     // Format the message to be sent.
    send_to_all(partida, msg2, strlen(msg2)); // Send the message to all players.
    return 0;
}

/**
 * Runs a game session in a separate thread.
 * @param args A pointer to the PartidaArgs struct containing necessary arguments.
 */
void Partida_Thread(void *args)
{
    PartidaArgs *partidaArgs = (PartidaArgs *)args;            // Cast the void pointer to a PartidaArgs pointer.
    Partida *partida = partidaArgs->partida;                   // Extract the Partida object from partidaArgs.
    ListaPartidas *listaPartidas = partidaArgs->listaPartidas; // Extract the ListaPartidas object from partidaArgs.
    int sumres = 0;
    while (sumres < 4)
    {
        pthread_mutex_lock(&partida->mutex);
        usleep(100000); // Pause the execution for 100 milliseconds.
        sumres = sum(partida->listos, 4);
        pthread_mutex_unlock(&partida->mutex);
    }

    send_0(partida); // Send a signal indicating that the sumres condition is met.
    pthread_mutex_lock(&partida->mutex);
    sumres = sum(partida->vidas, 4);
    pthread_mutex_unlock(&partida->mutex);

    while (sumres > 0)
    {
        send_1(partida); // Send signal 1 to the game session.
        send_2(partida); // Send signal 2 to the game session.
        usleep(1000);    // Pause the execution for 1 millisecond.
    }
}

/**
 * Stores a game in the database. There are two tables in the database: partidas and partidas_usuarios.
 * The partidas table stores the game sessions(id and global points), and the partidas_usuarios table stores the users that play in each game session.
 * Each row of the partidas_usuarios table has an id_partida and an id_usuario, which are foreign keys to the partidas and usuarios tables, respectively and puntuacion
 * which contains the points of the user in that game session.
 * @param conn The MySQL connection object.
 * @param partida a Partida struct containing the game session data.
 * @return 0 if the game was stored successfully, -1 otherwise.
 */
int add_game_to_db(MYSQL *conn, Partida *partida)
{
    char query[2000];
    char logmsg[200];
    // Log the function call
    snprintf(logmsg, 200, "add_game_to_db: %s", partida->idx);
    logger(LOGINFO, logmsg);
    // Construct the MySQL query to insert the game session into the partidas table
    snprintf(query, 2000, "INSERT INTO partidas (, puntuacion_global) VALUES ( %d);", sum(partida->puntos, 4));
    // Execute the MySQL query
    if (mysql_query(conn, query) != 0)
    {
        fprintf(stderr, "Error executing MySQL query: %s\n", mysql_error(conn));
        return -1;
    }
    // Obtain the id of the partida that was just inserted
    int id = mysql_insert_id(conn);

    // iterate through the users in the partida struct and insert them into the partidas_usuarios table
    for (int i = 0; i < 4; i++)
    {
        // Construct the MySQL query to insert the user into the partidas_usuarios table
        snprintf(query, 2000, "INSERT INTO partidas_usuarios (id_partida, id_usuario, puntuacion) VALUES (%d, (SELECT ID FROM usuarios WHERE nombre = '%s'), %d);",
                 id, partida->nombres[i], partida->puntos[i]);
        // Execute the MySQL query
        if (mysql_query(conn, query) != 0)
        {
            fprintf(stderr, "Error executing MySQL query: %s\n", mysql_error(conn));
            return -1;
        }
    }

    return 0;
}