#include "main.h"

void UpdateThreads(ConnectedList *list) {
    //    ConnectedList *list=threadArgs->list;
    //    LogQueue *queue=threadArgs->queue;
    int i = 0;
    const size_t RES_LEN = 2000;
    char res[RES_LEN];
    MYSQL *conn;
    conn = mysql_init(NULL);
    if (!mysql_real_connect(conn, DBSERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    while (1 == 1) {
        if (list->global_message == 1 || i >= 10000) {
            strcpy(res, "");
            int n = connected_to_string(list, res, RES_LEN);
            pthread_mutex_lock(&glubal_update_mutex); // No me interrumpas ahora

            push_connected(list, res, n);

            pthread_mutex_unlock(&glubal_update_mutex); // No me interrumpas ahora

            char mensage[2000];
            snprintf(mensage, 2000, "4/%d/%s\x04", n, res);
            logger(LOGINFO, mensage);
            sleep(1);
            //            enqueue(queue, get_iso8601_datetime(), LOGINFO, mensage, __FILE__, __FUNCTION__, __LINE__);
            pthread_mutex_lock(&glubal_update_mutex); // No me interrumpas ahora

            char *msg;
            int m;
            msg = chat_to_string(conn, &m);
            if (m != 0 && msg != NULL) {
                push_chat(list, msg, n);
            }

            list->global_message = 0;
            pthread_mutex_unlock(&glubal_update_mutex); // No me interrumpas ahora

            i = -1;

        }
        usleep(100000);
        i++;
    }
}

void GestionarInvitaciones(ListaPartidas *listaPartidas, Partida *partida, ConnectedList *list) {
    int denegado = 0;
    int sum = 0;
    char msg[200];
    sum = 0;
    for (int i = 0; i < 4; i++) {
        if (partida->answer[i] == -1) {
            denegado = 1;
            break;
        }
        sum += partida->answer[i];
    }

    if (denegado == 1) {
        snprintf(msg, 200, "7/0/%d", partida->idx);
        pthread_mutex_lock(&crear_partida_mutex);

        for (int i = 1; i < 4; i++) {
            write(partida->sockets[i], msg, strlen(msg));
            int pos_jugador = search_name_on_connected_llist(list, partida->nombres[i]);
            list->connections[pos_jugador].jugando = 0;
        }
        remove_node_from_partidas_list(listaPartidas, partida->idx);
        pthread_mutex_unlock(&crear_partida_mutex);
    } else if (sum == 4) {
        snprintf(msg, 200, "7/1/%d", partida->idx);
        for (int i = 1; i < 4; i++) {
            write(partida->sockets[i], msg, strlen(msg));
        }
    }
}

int GestionarCrearPartida(int pos, ConnectedList *list, ListaPartidas *listaPartidas, Nombre name1, Nombre name2,
                          Nombre name3, char res[200]) {
    char logmsg[2000];

    int i1 = search_name_on_connected_llist(list, name1);
    int i2 = search_name_on_connected_llist(list, name2);
    int i3 = search_name_on_connected_llist(list, name3);
    int is[] = {pos, i1, i2, i3};
    pthread_mutex_lock(&crear_partida_mutex);
    if (i1 == -1 || i2 == -1 || i3 == -1) {
        snprintf(res, 2000, "0/");

        return -1;
    }
    if (list->connections[i1].jugando + list->connections[i2].jugando + list->connections[i3].jugando != 0) {
        snprintf(res, 2000, "0/");

        return -1;
    }
    int i_partida = get_empty_from_partidas_list(listaPartidas);
    listaPartidas->partidas[i_partida].answer[0] = 1;
    for (int i = 0; i < 4; i++) {
        list->connections[is[i]].jugando = 1;
    }
    pthread_mutex_unlock(&crear_partida_mutex);
    for (int i = 0; i < 4; i++) {
        listaPartidas->partidas[i_partida].sockets[i] = list->connections[is[i]].sockfd;
        snprintf(listaPartidas->partidas[i_partida].nombres[i], 20, "%s", list->connections[is[i]].name);
    }

    char invitacion[200];
    snprintf(invitacion, 200, "6/%d/%s,%s*%s*%s", i_partida,
             list->connections[pos].name, list->connections[i1].name,
             list->connections[i2].name, list->connections[i3].name);
    for (int i = 1; i < sizeof(is) / sizeof(is[0]); i++) {

        snprintf(logmsg,2000,"%d: %s\n", is[i], invitacion);
        logger(LOGINFO,logmsg);
        pthread_mutex_lock(&invitation_mutex);
        list->connections[is[i]].invitando = 1;
        write(list->connections[is[i]].sockfd, invitacion, strlen(invitacion));
        list->connections[is[i]].invitando = 0;
        pthread_mutex_unlock(&invitation_mutex);
    }
    snprintf(res, 2000, "1/%d", listaPartidas->partidas[i_partida].idx);

    return listaPartidas->partidas[i_partida].idx;
}

void *AtenderThread(ThreadArgs *threadArgs) {
    int pos = threadArgs->i;
    ConnectedList *list = threadArgs->list;
    int sock_conn = list->connections[pos].sockfd;
    ListaPartidas *listaPartidas = threadArgs->lista_partidas;
    char soctext[20];
    snprintf(soctext, 20, "%d", sock_conn);
    logger(LOGINFO, soctext);

    MYSQL *conn;
    char request[512];
    ssize_t ret;
    char name[20];
    conn = mysql_init(NULL);
    char email[30], password[30];
    if (!mysql_real_connect(conn, DBSERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    char datos[2000];
    int n;
    Nombre name1;
    Nombre name2;
    Nombre name3;
    int vacios = 0;
    int code;
    char logmsg[2000];
    int send_awr = 1;
    while (vacios < 6) {
        char *response;

        send_awr = 1;
        logger(LOGINFO, "Esperando peticion\n");
        ret = read(sock_conn, request, sizeof(request));
        if (ret <= 0) {
            vacios++;

            continue;
        }
        logger(LOGINFO, "Recibido\n");
        request[ret] = '\0';
        snprintf(logmsg, 2000, "Conexion %d ha mandado: %s", list->connections[pos].idx, request);
        logger(LOGINFO, logmsg);
        char *p = strtok(request, "/");

        code = atoi(p); // convierte el string p al entero codigo
        int res;
        if (code == 0) {
            // desconectar
            break;
        }

        switch (code) {
            case 1: // Register
                p = strtok(NULL, "*");
                snprintf(name, 20, "%s", p);
                snprintf(logmsg, 2000, "Conexion %d ha intentado registrarse con usuario: %s",
                         list->connections[pos].idx, name);
                logger(LOGINFO, logmsg);
                p = strtok(NULL, "*");
                snprintf(password, 20, "%s", p);
                p = strtok(NULL, "*");
                snprintf(email, 20, "%s", p);
                res = register_user(conn, name, email, password);
                response = malloc(sizeof(char) * 4);
                if (res == 1) {

                    snprintf(response, sizeof(response), "%d/1", code);
                    strcpy(list->connections[pos].name, name);
                    pthread_mutex_lock(&glubal_update_mutex);

                    list->global_message = 1;
                    pthread_mutex_unlock(&glubal_update_mutex);
                    snprintf(logmsg, 2000, "Conexion %s se ha registrado exitosamente", name);
                    logger(LOGINFO, logmsg);
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
                logger(LOGINFO, logmsg);
                p = strtok(NULL, "*");
                strcpy(password, p);
                res = login(conn, name, password);
                response = malloc(sizeof(char) * 4);

                if (res > 0) {
                    snprintf(response, sizeof(response), "%d/1", code);
                    list->connections[pos].id = res;
                    pthread_mutex_lock(&glubal_update_mutex);
                    strcpy(list->connections[pos].name, name);
                    list->global_message = 1;
                    pthread_mutex_unlock(&glubal_update_mutex);
                    snprintf(logmsg, 2000, "Conexion %s se ha logeado exitosamente", name);
                    logger(LOGINFO, logmsg);
                } else if (res == -1)
                    snprintf(response, sizeof(response), "%d/0", code);
                else
                    snprintf(response, sizeof(response), "%d/2", code);
                break;

            case 3: // Ranking
                n = Devuelveme_Ranking(conn, datos);
                response = malloc(sizeof(datos) + sizeof(char) * 12);
                snprintf(response, sizeof(response), "%d/%d/%s", code, n, datos);
                break;

            case 4: // Pedir online
                n = connected_llist_to_string(list, datos);
                response = malloc(sizeof(datos) + sizeof(char) * 12);
                snprintf(response, sizeof(response), "%d/%d/%s", code, n, datos);
                break;

            case 5: // Crear Partida
                p = strtok(NULL, "*");
                strcpy(name1, p);
                p = strtok(NULL, "*");
                strcpy(name2, p);
                p = strtok(NULL, "*");
                strcpy(name3, p);
                logger(LOGINFO, "Creando Partida");
                GestionarCrearPartida(pos, list, listaPartidas, name1, name2, name3, datos);
                response = malloc(sizeof(datos) + sizeof(char) * 12);

                snprintf(response, sizeof(response), "%d/%s", code, datos);
                break;
            case 6:
                p = strtok(NULL, "/");
                int idx_partida = atoi(p);
                p = strtok(NULL, "/");
                logger(LOGINFO, "Gestionando invitaciones");
                int i_partida = search_on_partidas_llist(listaPartidas, idx_partida);
                for (int i = 0; i < 4; i++) {
                    if (strcmp(list->connections[pos].name, listaPartidas->partidas[i_partida].nombres[i]) == 0) {
                        listaPartidas->partidas[i_partida].answer[i] = 2 * atoi(p) - 1;
                        break;
                    }
                }
                GestionarInvitaciones(listaPartidas, &listaPartidas->partidas[i_partida], list);
                send_awr = 0;
                break;
            case 7:
                Atender_Cliente_Partida();
                break;

            case 9:
                logger(LOGINFO, "Se ha recibido un mensage en el chat");
                p = strtok(NULL, "/");
                n = write_message(conn, list->connections[pos].name, p);
                response = malloc(sizeof(char) * 4);
                if (n == 0)
                    snprintf(response, 4, "9/1");
                else if (n == 1)
                    snprintf(response, 4, "9/0");
                else
                    snprintf(response, 4, "9/2");
                break;


            default:
                snprintf(logmsg, 2000, "Conexion %d ha intentado hacer una conexion no definida %d", sock_conn, code);
                logger(LOGWARNING, logmsg);
                response = malloc(sizeof("error") + 1);
                snprintf(response, sizeof(response), "error");
        }
        if (send_awr == 1) {
            if (list->connections[pos].sending_connected == 1) {
                snprintf(logmsg, 2000, "Conexion %d esperando a que se mande la lista de connectados", code);
                logger(LOGINFO, logmsg);

                while (list->connections[pos].sending_connected == 1)
                    usleep(100000);
            }
            strcat(response, "\x04");

            logger(LOGINFO, response);
            write(sock_conn, response, strlen(response));
            free(response);
        }
    }

    close(sock_conn);
    mysql_close(conn);
    pthread_mutex_lock(&main_mutex); // No me interrumpas ahora
    snprintf(logmsg, 2000, "Removing: %s %d", name, pos);
    logger(LOGINFO, logmsg);
    remove_node_from_connected_list(list, pos);

    pthread_mutex_unlock(&main_mutex); // ya puedes interrumpirme
    pthread_mutex_lock(&glubal_update_mutex);
    logger(LOGINFO, "Actualizando lista de conectados a los clientes");
    list->global_message = 1;
    pthread_mutex_unlock(&glubal_update_mutex);
}

int main() {
    logger_init("logger.txt");
    char buf[1024];
    getlogin_r(buf, sizeof(buf));
    logger(LOGINFO, "Identificando tipo de servidor");
    if (strcmp(buf, "antonia") == 0) {
        logger(LOGINFO, "Se ha detectado como maquina virtual");
        snprintf(DBSERVER, 30, LDBSERVER);
    } else {
        logger(LOGINFO, "Se ha detectado como servidor de producción");
        snprintf(DBSERVER, 30, PDBSERVER);
    }
    int sock_conn, sock_listen;
    struct sockaddr_in server_addr;
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        logger(LOGERROR,"Error en el socket\n");

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;

    server_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    server_addr.sin_port = htons(PORT);

    if (bind(sock_listen, (struct sockaddr *) &server_addr, sizeof(server_addr)) < 0) {
        logger(LOGERROR,"Error en el bind\n");
        exit(1);
    }

    if (listen(sock_listen, 3) < 0)
        logger(LOGERROR,"Error en el listent\n");

    pthread_t thread;
    pthread_t update_thread;
    ConnectedList *list = (ConnectedList *) malloc(sizeof(ConnectedList));
    ListaPartidas *listaPartidas = (ListaPartidas *) malloc(sizeof(ListaPartidas));
    list->global_message = 0;
    initialize_connected_list(list);


    pthread_create(&update_thread, NULL, (void *(*)(void *)) UpdateThreads, list);
    int i = 0;
    logger(LOGINFO,"Empezando bucle principal");
    for (;;) {
        ThreadArgs *threadArgs = (ThreadArgs *) malloc(sizeof(ThreadArgs));
        threadArgs->list = list;
        threadArgs->lista_partidas = listaPartidas;
        logger(LOGINFO,"Escuchando\n");


        sock_conn = accept(sock_listen, NULL, NULL);
        logger(LOGINFO,"Conexion recibida\n");
        i = get_empty_from_connected_list(list);
        if (i < 0) {
            close(sock_conn);
            logger(LOGWARNING,"La lista de conectados está llena");
            continue;
        }
        char message[2000];
        snprintf(message, 2000, "%d", i);
        logger(LOGINFO,message);
        list->connections[i].sockfd = sock_conn;
        threadArgs->i = i;
        //        threadArgs->queue = &queue;
        pthread_create(&thread, NULL, (void *(*)(void *)) AtenderThread, threadArgs);

    }

    return 0;
}
