#include "main.h"

/**
 * Updates the threads with the latest information.
 * @param list A pointer to the ConnectedList object.
 */
void UpdateThreads(ConnectedList *list) {
    // ConnectedList *list=threadArgs->list;
    // LogQueue *queue=threadArgs->queue;
    int i = 0;
    const size_t RES_LEN = 2000;
    char res[RES_LEN];
    MYSQL *conn;
    // Initialize the MySQL connection
    conn = mysql_init(NULL);
    if (!mysql_real_connect(conn, DBSERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    while (1 == 1) {
        if (list->global_message == 1 || i >= 10000) {
            strcpy(res, "");
            int n = connected_to_string(list, res, RES_LEN);
            pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption

            push_connected(list, res, n);

            pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex

            char mensage[2000];
            snprintf(mensage, 2000, "4/%d/%s\x04", n, res);
            logger(LOGINFO, mensage);
            sleep(1);
            //            enqueue(queue, get_iso8601_datetime(), LOGINFO, mensage, __FILE__, __FUNCTION__, __LINE__);
            pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption

            char *msg;
            int m;
            msg = chat_to_string(conn, &m);
            if (m != 0 && msg != NULL) {
                push_chat(list, msg, n);
            }

            list->global_message = 0;
            pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex

            i = -1;
        }
        usleep(100000);
        i++;
    }
}

/**
 * Manages invitations for a game session.
 * @param listaPartidas A pointer to the ListaPartidas struct.
 * @param partida A pointer to the Partida struct.
 * @param list A pointer to the ConnectedList struct.
 */
void ManageInvitation(ListaPartidas *listaPartidas, Partida *partida, ConnectedList *list) {
    int denegado = 0; // Flag to check if the invitation is denied
    int sum = 0;      // Variable to store the sum of answers
    char msg[200];    // Buffer to store messages
    sum = 0;
    int pos_jugadores[4];
    for (int i = 0; i < 4; i++) {
        pos_jugadores[i] = search_name_on_connected_llist(list, partida->nombres[i]);

        if (pos_jugadores[i] == -1) {
            denegado = 1;
            break;
        }
    }

    for (int i = 0; i < 4; i++) {
        //        if (list->connections[pos_jugadores[i]].using == 0)
        //        {
        //            denegado = 1;
        //            break;
        //        }
        if (partida->answer[i] == -1) {
            denegado = 1;
            break;
        }

        sum += partida->answer[i];
    }

    if (denegado == 1) {
        snprintf(msg, 200, "7/0/%d", partida->idx); // Format the denial message
        pthread_mutex_lock(&crear_partida_mutex);   // Lock the mutex before modifying the partida list
        // Notify other players and update their status
        for (int i = 0; i < 4; i++) {
            write(partida->sockets[i], msg, strlen(msg));
            int pos_jugador = search_name_on_connected_llist(list, partida->nombres[i]);
            if (pos_jugador != -1)
                continue;
            list->connections[pos_jugador].jugando = 0; // Set the player's status to not playing
        }
        remove_node_from_partidas_list(listaPartidas, partida->idx); // Remove the partida from the list
        pthread_mutex_unlock(
                &crear_partida_mutex); // Unlock the mutex after modifying the partida list
    } else if (sum == 4) {
        snprintf(msg, 200, "7/1/%d", partida->idx); // Format the acceptance message
        // Notify all players about the acceptance message
        for (int i = 0; i < 4; i++) {
            write(partida->sockets[i], msg, strlen(msg));
        }
    }
}

/**
 * Manages the creation of a game session.
 * @param pos The position of the player initiating the game session.
 * @param list A pointer to the ConnectedList struct.
 * @param listaPartidas A pointer to the ListaPartidas struct.
 * @param name1 The name of the first player to invite.
 * @param name2 The name of the second player to invite.
 * @param name3 The name of the third player to invite.
 * @param res The buffer to store the result message.
 * @return The index of the created partida, or -1 if the creation fails.
 */
int ManageCrearPartida(int pos, ConnectedList *list, ListaPartidas *listaPartidas, Nombre name1, Nombre name2,
                       Nombre name3, char res[200]) {
    char logmsg[2000]; // Buffer to store log messages

    int i1 = search_name_on_connected_llist(list, name1); // Get the index of the first invited player
    int i2 = search_name_on_connected_llist(list, name2); // Get the index of the second invited player
    int i3 = search_name_on_connected_llist(list, name3); // Get the index of the third invited player
    int is[] = {pos, i1, i2, i3};                         // Array to store player indices
    pthread_mutex_lock(&crear_partida_mutex);             // Lock the mutex before modifying the partida list
    // Check if any of the players are not found or already playing
    if (i1 == -1 || i2 == -1 || i3 == -1) {
        snprintf(res, 2000, "0/");
        // Set the result message indicating failure
        return -1;
    }
    if (list->connections[i1].jugando + list->connections[i2].jugando + list->connections[i3].jugando != 0) {
        snprintf(res, 2000, "0/");
        // Set the result message indicating failure
        return -1;
    }
    int i_partida = get_empty_from_partidas_list(listaPartidas); // Get an empty partida index from the list
    listaPartidas->partidas[i_partida].answer[0] = 1;            // Set the answer for the initiating player
    for (int i = 0; i < 4; i++) {
        list->connections[is[i]].jugando = 1; // Set the playing status for all players
    }
    pthread_mutex_unlock(&crear_partida_mutex); // Unlock the mutex after modifying the partida list
    // Store the socket connections and player names in the partida
    for (int i = 0; i < 4; i++) {
        listaPartidas->partidas[i_partida].sockets[i] = list->connections[is[i]].sockfd;
        snprintf(listaPartidas->partidas[i_partida].nombres[i], 20, "%s", list->connections[is[i]].name);
    }

    char invitacion[200];
    snprintf(invitacion, 200, "6/%d/%s,%s*%s*%s", i_partida,
             list->connections[pos].name, list->connections[i1].name,
             list->connections[i2].name, list->connections[i3].name); // Format the invitation message
    for (int i = 1; i < 4; i++) {

        snprintf(logmsg, 2000, "%d: %s\n", is[i], invitacion); // Create a log message
        logger(LOGINFO, logmsg);                               // Log the invitation
        pthread_mutex_lock(
                &invitation_mutex);                                                 // Lock the mutex before modifying the invitation status
        list->connections[is[i]].invitando = 1;                                 // Set the player's invitation status to inviting
        write(list->connections[is[i]].sockfd, invitacion, strlen(invitacion)); // Send the invitation
        list->connections[is[i]].invitando = 0;                                 // Set the player's invitation status to not inviting
        pthread_mutex_unlock(
                &invitation_mutex); // Unlock the mutex after modifying the invitation status
    }
    snprintf(res, 2000, "1/%d", listaPartidas->partidas[i_partida].idx); // Set the result message indicating success

    return listaPartidas->partidas[i_partida].idx; // Return the index of the created partida
}

/**
 * Handles client requests in a separate thread.
 * @param threadArgs A pointer to the ThreadArgs struct containing necessary arguments.
 * @return void pointer
 */
void *AtenderThread(ThreadArgs *threadArgs) {
    int pos = threadArgs->i;                                   // Get the position from threadArgs
    ConnectedList *list = threadArgs->list;                    // Get the ConnectedList from threadArgs
    int sock_conn = list->connections[pos].sockfd;             // Get the socket connection from ConnectedList
    ListaPartidas *listaPartidas = threadArgs->lista_partidas; // Get the ListaPartidas from threadArgs
    char soctext[20];
    snprintf(soctext, 20, "%d", sock_conn);
    logger(LOGINFO, soctext); // Log the socket connection

    MYSQL *conn;                  // MySQL connection object
    char request[512];            // Buffer to store the received request
    ssize_t ret;                  // Return value of the read operation
    char name[20];                // Buffer to store a name
    conn = mysql_init(NULL);      // Initialize the MySQL connection object
    char email[30], password[30]; // Buffers to store an email and password
    if (!mysql_real_connect(conn, DBSERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        // Connect to the MySQL database
        // If the connection fails, print the error and exit
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    char datos[2000]; // Buffer to store data
    int n;            // Variable to store an integer value
    Nombre name1;     // Variable of type Nombre
    Nombre name2;
    Nombre name3;
    int vacios = 0;    // Counter for empty requests
    int code;          // Variable to store a code
    char logmsg[2000]; // Buffer to store log messages
    int send_awr = 1;  // Flag to control sending a response
    int i_partida;
    while (vacios < 6) {
        char *response;

        send_awr = 1;
        logger(LOGINFO, "Esperando peticion\n");         // Log a message indicating that a request is being awaited
        ret = read(sock_conn, request, sizeof(request)); // Read the client request from the socket connection
        if (ret <= 0) {
            vacios++;

            continue;
        }
        logger(LOGINFO, "Recibido\n"); // Log a message indicating that the request has been received
        request[ret] = '\0';           // Null-terminate the received request string
        snprintf(logmsg, 2000, "Conexion %d ha mandado: %s", list->connections[pos].idx, request);
        logger(LOGINFO, logmsg); // Log the received request with connection information
        char *p = strtok(request, "/");

        code = atoi(p); // Convert the string p to an integer code
        int res;
        if (code == 0) {
            // Disconnect
            break;
        }

        switch (code) {
            case 1: // Register
                p = strtok(NULL, "*");
                snprintf(name, 20, "%s", p);
                snprintf(logmsg, 2000, "Conexion %d ha intentado registrarse con usuario: %s",
                         list->connections[pos].idx, name);
                logger(LOGINFO, logmsg); // Log a message indicating the registration attempt with the username
                p = strtok(NULL, "*");
                snprintf(password, 20, "%s", p);
                p = strtok(NULL, "*");
                snprintf(email, 20, "%s", p);
                res = register_user(conn, name, email, password); // Call register_user function
                response = malloc(sizeof(char) * 4);
                if (res == 1) {

                    snprintf(response, sizeof(response), "%d/1", code);
                    strcpy(list->connections[pos].name, name);
                    pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption

                    list->global_message = 1;
                    pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex
                    snprintf(logmsg, 2000, "Conexion %s se ha registrado exitosamente", name);
                    logger(LOGINFO, logmsg); // Log a message indicating a successful registration
                } else if (res == -1)
                    snprintf(response, sizeof(response), "%d/0", code);
                else
                    snprintf(response, sizeof(response), "%d/2", code);
                break;
            case 2: // Login
                p = strtok(NULL, "*");
                strcpy(name, p);
                snprintf(logmsg, 2000, "Conexion %d ha intentado logearse con usuario: %s",
                         list->connections[pos].idx, name);
                logger(LOGINFO, logmsg); // Log a message indicating the login attempt with the username
                p = strtok(NULL, "*");
                strcpy(password, p);
                res = login(conn, name, password); // Call login function
                response = malloc(sizeof(char) * 4);

                if (res > 0) {
                    snprintf(response, sizeof(response), "%d/1", code);
                    list->connections[pos].id = res;
                    pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption
                    strcpy(list->connections[pos].name, name);
                    list->global_message = 1;
                    pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex
                    snprintf(logmsg, 2000, "Conexion %s se ha logeado exitosamente", name);
                    logger(LOGINFO, logmsg);
                } else if (res == -1)
                    snprintf(response, sizeof(response), "%d/0", code);
                else
                    snprintf(response, sizeof(response), "%d/2", code);
                break;

            case 3:                           // Ranking
                n = Get_Ranking(conn, datos); // Call Devuelveme_Ranking function
                response = malloc(sizeof(datos) + sizeof(char) * 12);
                snprintf(response, sizeof(response), "%d/%d/%s", code, n, datos);
                break;

            case 4:                                         // Get Online
                n = connected_llist_to_string(list, datos); // Call connected_llist_to_string function
                response = malloc(sizeof(datos) + sizeof(char) * 12);
                snprintf(response, sizeof(response), "%d/%d/%s", code, n, datos);
                break;

            case 5: // Create game
                p = strtok(NULL, "*");
                strcpy(name1, p);
                p = strtok(NULL, "*");
                strcpy(name2, p);
                p = strtok(NULL, "*");
                strcpy(name3, p);
                logger(LOGINFO, "Creando Partida");
                ManageCrearPartida(pos, list, listaPartidas, name1, name2, name3,
                                   datos); // Call GestionarCrearPartida function
                response = malloc(sizeof(datos) + sizeof(char) * 12);

                snprintf(response, sizeof(response), "%d/%s", code, datos);
                break;
            case 6:
                p = strtok(NULL, "/");
                int invres = atoi(p);
                p = strtok(NULL, "/");
                int idx_partida = atoi(p);
                logger(LOGINFO,
                       "Gestionando invitaciones"); // Log a message indicating that invitations are being managed
                i_partida = search_on_partidas_llist(listaPartidas,
                                                     idx_partida); // Search for the index of the partida in the listaPartidas
                for (int i = 0; i < 4; i++) {
                    if (strcmp(list->connections[pos].name, listaPartidas->partidas[i_partida].nombres[i]) == 0) {
                        // Check if the name of the connection matches the name in the partida
                        listaPartidas->partidas[i_partida].answer[i] = 2 * invres - 1;
                        // Update the answer for the corresponding player in the partida
                        break;
                    }
                }
                ManageInvitation(listaPartidas, &listaPartidas->partidas[i_partida],
                                 list); // Call GestionarInvitaciones function
                send_awr = 0;
                break;
            case 7:
                p = strtok(NULL, "/");
                int v = atoi(p);
                if (v == 1) {
                    logger(LOGINFO, "Empezando Partida");
                    Atender_Cliente_Partida(&listaPartidas->partidas[i_partida], name,
                                            conn); // Call the Atender_Cliente_Partida function
                } else {
                    logger(LOGINFO, "Rechazando Partida");
                }
                send_awr = 0;

                break;

            case 9:
                logger(LOGINFO,
                       "Se ha recibido un mensage en el chat"); // Log a message indicating that a chat message has been received
                p = strtok(NULL, "/");
                n = write_message(conn, list->connections[pos].name, p); // Call write_message function
                response = malloc(sizeof(char) * 4);
                if (n == 0) {
                    snprintf(response, 4, "9/1");
                    int l;
                    char *res = chat_to_string(conn, &l);
                    push_chat(list, res, l);
                } else if (n == 1)
                    snprintf(response, 4, "9/0");
                else
                    snprintf(response, 4, "9/2");
                break;
            case 11:
                logger(LOGINFO, "Se va a eliminar el usuario");
                int res = deleteUser(conn, list->connections[pos].name); // Call deleteUser function
                if (res == 1) {
                    response = malloc(sizeof(char) * 5);
                    snprintf(response, 5, "11/1");
                    pthread_mutex_lock(&main_mutex); // Acquire the main mutex to avoid interruption
                    snprintf(logmsg, 2000, "Removing: %s %d", name, pos);
                    logger(LOGINFO, logmsg);
                    remove_node_from_connected_list(list, pos);

                    pthread_mutex_unlock(&main_mutex);        // Release the main mutex
                    pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption
                    logger(LOGINFO, "Actualizando lista de conectados a los clientes");
                    list->global_message = 1;
                    pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex
                } else {
                    response = malloc(sizeof(char) * 5);
                    snprintf(response, 5, "11/0");
                }
                break;
            case 12:
                logger(LOGINFO, "Se ha pedido la lista usuarios con los que he jugado");
                int n12;
                char *res12 = obtenerNombres(conn, list->connections[pos].name, &n12); // Call obtenerNombres function
                response = malloc(sizeof(char) * (n12 + 4));
                snprintf(response, n12 + 4, "12/%s", res12);
                break;
            case 14:
                logger(LOGINFO, "Se ha pedido la lista de partidas");
                int n14;
                p = strtok(NULL, "/");
                char *res14 = get_partidas_string_by_name(conn, p, &n14); // Call get_partidas_string_by_name function
                response = malloc(sizeof(char) * (n14 + 4));
                snprintf(response, n14 + 4, "14/%s", res14);
                break;
            default:
                snprintf(logmsg, 2000, "Conexion %d ha intentado hacer una conexion no definida %d", sock_conn, code);
                logger(LOGWARNING, logmsg); // Log a warning message indicating an undefined connection attempt
                response = malloc(sizeof("error") + 1);
                snprintf(response, sizeof(response), "error");
        }
        if (send_awr == 1) {
            if (list->connections[pos].sending_connected == 1) {
                snprintf(logmsg, 2000, "Conexion %d esperando a que se mande la lista de connectados", code);
                logger(LOGINFO,
                       logmsg); // Log a message indicating that the connection is waiting for the list of connected clients to be sent

                while (list->connections[pos].sending_connected == 1)
                    usleep(100000); // Sleep for a short time while waiting for the list to be sent
            }
            strcat(response, "\x04");

            logger(LOGINFO, response);                    // Log the response message
            write(sock_conn, response, strlen(response)); // Write the response message back to the client
            free(response);
        }
    }

    close(sock_conn);
    mysql_close(conn);
    pthread_mutex_lock(&main_mutex); // Acquire the main mutex to avoid interruption
    snprintf(logmsg, 2000, "Removing: %s %d", name, pos);
    logger(LOGINFO, logmsg);
    remove_node_from_connected_list(list, pos);

    pthread_mutex_unlock(&main_mutex);        // Release the main mutex
    pthread_mutex_lock(&glubal_update_mutex); // Acquire the global update mutex to avoid interruption
    logger(LOGINFO, "Actualizando lista de conectados a los clientes");
    list->global_message = 1;
    pthread_mutex_unlock(&glubal_update_mutex); // Release the global update mutex
}

int main() {
    logger_init("logger.txt"); // Initialize the logger with a log file
    char buf[1024];
    getlogin_r(buf, sizeof(buf));                      // Get the username of the current user
    logger(LOGINFO, "Identificando tipo de servidor"); // Log a message indicating the identification of the server type
    if (strcmp(buf, "antonia") == 0) {
        logger(LOGINFO,
               "Se ha detectado como maquina virtual"); // Log a message indicating that the server has been detected as a virtual machine
        snprintf(DBSERVER, 30,
                 LDBSERVER); // Set the database server address to the virtual machine address
    } else {
        logger(LOGINFO,
               "Se ha detectado como servidor de producción"); // Log a message indicating that the server has been detected as a production server
        snprintf(DBSERVER, 30,
                 PDBSERVER); // Set the database server address to the production server address
    }
    int sock_conn, sock_listen;
    struct sockaddr_in server_addr;
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        logger(LOGERROR, "Error en el socket\n"); // Log an error message if there is an issue creating the socket

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;

    server_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    server_addr.sin_port = htons(PORT);

    if (bind(sock_listen, (struct sockaddr *) &server_addr, sizeof(server_addr)) < 0) {
        logger(LOGERROR,
               "Error en el bind\n"); // Log an error message if there is an issue binding the socket to the address
        exit(1);
    }

    if (listen(sock_listen, 3) < 0)
        logger(LOGERROR,
               "Error en el listent\n"); // Log an error message if there is an issue with listening for connections

    pthread_t thread;
    pthread_t update_thread;
    ConnectedList *list = (ConnectedList *) malloc(
            sizeof(ConnectedList));                                                    // Allocate memory for the connected client list
    ListaPartidas *listaPartidas = (ListaPartidas *) malloc(sizeof(ListaPartidas)); // Allocate memory for the game list
    list->global_message = 0;
    initialize_connected_list(list); // Initialize the connected client list

    pthread_create(&update_thread, NULL, (void *(*)(void *)) UpdateThreads,
                   list); // Create a thread for updating the client connections
    int i = 0;
    logger(LOGINFO, "Empezando bucle principal");
    for (;;) {
        ThreadArgs *threadArgs = (ThreadArgs *) malloc(sizeof(ThreadArgs)); // Allocate memory for the thread arguments
        threadArgs->list = list;
        threadArgs->lista_partidas = listaPartidas;
        logger(LOGINFO, "Escuchando\n"); // Log a message indicating that the server is listening for connections

        sock_conn = accept(sock_listen, NULL, NULL); // Accept a new connection
        logger(LOGINFO, "Conexion recibida\n");      // Log a message indicating that a connection has been received
        i = get_empty_from_connected_list(list);     // Get an empty slot from the connected client list
        if (i < 0) {
            close(sock_conn);
            logger(LOGWARNING, "La lista de conectados está llena");
            continue;
        }
        char message[2000];
        snprintf(message, 2000, "%d", i);
        logger(LOGINFO, message);
        list->connections[i].sockfd = sock_conn;
        threadArgs->i = i;
        //        threadArgs->queue = &queue;
        pthread_create(&thread, NULL, (void *(*)(void *)) AtenderThread, threadArgs);
    }

    return 0;
}
