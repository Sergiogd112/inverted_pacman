#include "main.h"

void UpdateConnectedThread(ConnectedList *list)
{
    int i = 0;
    const size_t RES_LEN = 2000;
    char res[RES_LEN];
    while (1 == 1)
    {
        if (list->update_connecetions == 1 || i > 10000)
        {
            strcpy(res, "");
            int n = connected_to_string(list, res, RES_LEN);
            pthread_mutex_lock(&update_connected_mutex); // No me interrumpas ahora

            push_connected(list, res, n);

            list->update_connecetions = 0;
            pthread_mutex_unlock(&update_connected_mutex); // No me interrumpas ahora
            char message[MAXLOGMSGLEN];
            snprintf(message, MAXLOGMSGLEN, "4/%d/%s\x04", n, res);

            enqueue(&queue, get_iso8601_datetime(), LOGINFO, message, __FILE__, __FUNCTIONW__, __LINE__);

            i = -1;
        }
        sleep(0.1);
        i++;
    }
}

void *AtenderThread(ThreadArgs *threadArgs)
{
    int pos = threadArgs->i;
    ConnectedList *list = threadArgs->list;
    int sock_conn = list->connections[pos].sockfd;
    LogQueue *queue=threadArgs->queue;
    printf("%d\n", sock_conn);

    MYSQL *conn;
    char request[512];
    char response[2010];
    int ret;
    char name[20];
    conn = mysql_init(NULL);
    char user[30], email[30], password[30];
    if (!mysql_real_connect(conn, DBSERVER, USER, PASSWORD, DATABASE, 0, NULL, 0))
    {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    char datos[2000];
    int n;
    char id1[20];
    char id2[20];
    char id3[20];
    char id4[20];
    int vacios = 0;
    int code;
    char logmsg[MAXLOGMSGLEN];

    while (vacios < 6)
    {
        enqueue(queue, get_iso8601_datetime(), LOGINFO, "Esperando peticion", __FILE__, __FUNCTIONW__, __LINE__);

        ret = read(sock_conn, request, sizeof(request));
        if (ret <= 0)
        {
            vacios++;
            continue;
        }
        enqueue(queue, get_iso8601_datetime(), LOGINFO, "Recibido", __FILE__, __FUNCTIONW__, __LINE__);

        request[ret] = '\0';
        snprintf(logmsg, MAXLOGMSGLEN, "Conexion %d ha mandado: %s", list->connections[pos].idx, request);
        enqueue(queue, get_iso8601_datetime(), LOGINFO, logmsg, __FILE__, __FUNCTIONW__, __LINE__);

        char *p = strtok(request, "/");

        code = atoi(p); // convierte el string p al entero codigo
        char user[20];
        int res;
        if (code == 0)
        {
            // desconectar
            break;
        }
        snprintf(response, 2000, "%d/", code);
        switch (code)
        {
        case 1: // Register
            p = strtok(NULL, "*");
            snprintf(name, 20, "%s", p);
            snprintf(logmsg, MAXLOGMSGLEN, "Conexion %d ha intentado registrarse con usuario: %s", list->connections[pos].idx, name);
            enqueue(queue, get_iso8601_datetime(), LOGINFO, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            printf("Codigo: %d, Nombre: %s\n", code, name);
            p = strtok(NULL, "*");
            snprintf(password, 20, "%s", p);
            p = strtok(NULL, "*");
            snprintf(email, 20, "%s", p);
            res = register_user(conn, name, email, password);

            if (res == 1)
            {
                strcat(response, "1");
                strcpy(list->connections[pos].name, name);
                pthread_mutex_lock(&update_connected_mutex);

                list->update_connecetions = 1;
                pthread_mutex_unlock(&update_connected_mutex);
                snprintf(logmsg, MAXLOGMSGLEN, "Conexion %s se ha registrado exitosamente", name);
                enqueue(queue, get_iso8601_datetime(), LOGINFO, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            }
            else if (res == -1)
                strcat(response, "0");
            else
                strcat(response, "2");
            break;
        case 2: // Login
            p = strtok(NULL, "*");
            strcpy(name, p);
            snprintf(logmsg, MAXLOGMSGLEN, "Conexion %d ha intentado logearse con usuario: %s", list->connections[pos].idx, name);
            enqueue(queue, get_iso8601_datetime(), LOGINFO, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            p = strtok(NULL, "*");
            strcpy(password, p);
            res = login(conn, name, password);
            if (res > 0)
            {
                strcat(response, "1");
                list->connections[pos].id = res;
                pthread_mutex_lock(&update_connected_mutex);
                strcpy(list->connections[pos].name, name);
                list->update_connecetions = 1;
                pthread_mutex_unlock(&update_connected_mutex);
                snprintf(logmsg, MAXLOGMSGLEN, "Conexion %s se ha logeado exitosamente", name);
                enqueue(queue, get_iso8601_datetime(), LOGINFO, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            }
            else if (res == -1)
                strcat(response, "0");
            else
                strcat(response, "2");
            break;

        case 3: // Ranking
            n = Devuelveme_Ranking(datos);
            snprintf(response, 2000, "%d/%d/%s", code, n, datos);
            break;

        case 4: // Pedir online
            n = llist_to_string(list, datos);
            snprintf(response, 2000, "%d/%d/%s", code, n, datos);
            break;

        case 5: // Crear Partida
            p = strtok(NULL, "*");
            strcpy(id1, p);
            p = strtok(NULL, "*");
            strcpy(id2, p);
            p = strtok(NULL, "*");
            strcpy(id3, p);
            p = strtok(NULL, "*");
            strcpy(id4, p);

            if (search_on_llist(list, atoi(id1)) != -1 && search_on_llist(list, atoi(id2)) != -1 &&
                search_on_llist(list, atoi(id3)) != -1 && search_on_llist(list, atoi(id4)) != -1)
                strcat(response, "1"); // todo bn
            else
                strcat(response, "0"); // alguno de los usuarios no est� conectado

            break;

        default:
            snprintf(logmsg, MAXLOGMSGLEN, "Conexion %d ha intentado hacer una conexion no definida %d", code);
            enqueue(queue, get_iso8601_datetime(), LOGERROR, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            strcat(response, "error");
        }
        if (list->connections[pos].sending_connected == 1)
        {
            snprintf(logmsg, MAXLOGMSGLEN, "Conexion %d esperando a que se mande la lista de connectados", code);
            enqueue(queue, get_iso8601_datetime(), LOGERROR, logmsg, __FILE__, __FUNCTIONW__, __LINE__);
            printf("waiting to finish sending connected");
            while (list->connections[pos].sending_connected == 1)
                sleep(.1);
        }
        strcat(response, "\x04");
        printf("Respuesta: %s\n", response);
        write(sock_conn, response, strlen(response));
    }

    close(sock_conn);
    mysql_close(conn);
    pthread_mutex_lock(&main_mutex); // No me interrumpas ahora
    printf("Removing: %s %d", name, pos);
    remove_node_from_list(list, pos);
    print_idx(list);
    pthread_mutex_unlock(&main_mutex); // ya puedes interrumpirme
}

int main()
{
    LogQueue queue;
    queue->count = 0;
    pthread_t log_thread;
    pthread_create(&log_thread, NULL, (void *(*)(void *))logthreadboth, &queue);

    int sock_conn, sock_listen;
    struct sockaddr_in server_addr;
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        enqueue(&queue, get_iso8601_datetime(), LOGERROR, "socket error", __FILE__, __FUNCTIONW__, __LINE__);

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;

    server_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    server_addr.sin_port = htons(PORT);

    if (bind(sock_listen, (struct sockaddr *)&server_addr, sizeof(server_addr)) < 0)
        enqueue(&queue, get_iso8601_datetime(), LOGERROR, "Error al bind", __FILE__, __FUNCTIONW__, __LINE__);

    if (listen(sock_listen, 3) < 0)
        enqueue(&queue, get_iso8601_datetime(), LOGERROR, "Error en el listen", __FILE__, __FUNCTIONW__, __LINE__);

    pthread_t thread;
    pthread_t update_thread;
    ConnectedList list;
    initialize_list(&list);
    pthread_create(&update_thread, NULL, (void *(*)(void *))UpdateConnectedThread, &list);
    char res[200];
    int i = 0;
    for (;;)
    {
        ThreadArgs *threadArgs = (ThreadArgs *)malloc(sizeof(ThreadArgs));
        threadArgs->list = &list;
        enqueue(&queue, get_iso8601_datetime(), LOGINFO, "Escuchando", __FILE__, __FUNCTIONW__, __LINE__);

        sock_conn = accept(sock_listen, NULL, NULL);
        enqueue(&queue, get_iso8601_datetime(), LOGINFO, "Conexion recibida", __FILE__, __FUNCTIONW__, __LINE__);
        i = get_empty(&list);
        if (i < 0)
        {
            close(sock_conn);
            enqueue(&queue, get_iso8601_datetime(), LOGWARNING, "Lista de conectados llena", __FILE__, __FUNCTIONW__, __LINE__);
            continue;
        }
        char message[MAXLOGMSGLEN];
        snprintf(message, MAXLOGMSGLEN, "%d", i)
            enqueue(&queue, get_iso8601_datetime(), LOGINFO, message, __FILE__, __FUNCTIONW__, __LINE__);

        list.connections[i].sockfd = sock_conn;
        threadArgs->i = i;
        threadArgs->queue = &queue;
        pthread_create(&thread, NULL, (void *(*)(void *))AtenderThread, threadArgs);
        llist_to_string(&list, message);
        enqueue(&queue, get_iso8601_datetime(), LOGINFO, message, __FILE__, __FUNCTIONW__, __LINE__);
    }

    return 0;
}
