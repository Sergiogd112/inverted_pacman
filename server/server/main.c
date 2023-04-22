#include "main.h"

void UpdateConnectedThread(struct Nodes **head_ref) {
    while (1 == 1) {
        continue;
    }
}

void *AtenderThread(ThreadArgs * threadArgs) {
    int pos=threadArgs->i;
    ConnectedList *list =threadArgs->list;
    struct  Node *node=&list->connections[pos];
    int sock_conn = node->sockfd;
    printf("%d\n", sock_conn);


    MYSQL *conn;
    char request[512];
    char response[2010];
    int ret;
    char name[20];
    conn = mysql_init(NULL);
    char user[30], email[30], password[30];
    if (!mysql_real_connect(conn, SERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
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
    while (vacios < 6) {
        printf("Esperando peticion\n");
        ret = read(sock_conn, request, sizeof(request));
        if (ret <= 0) {
            vacios++;
            continue;
        }
        printf("Recibido\n");

        request[ret] = '\0';

        printf("Peticion: %s\n", request);

        char *p = strtok(request, "/");
//        if (p==NULL)
//            vacios++;
//            continue;
        code = atoi(p); //convierte el string p al entero codigo
        char user[20];
        int res;
        if (code == 0) {
            //desconectar
            break;
        }
        snprintf(response, 2000, "%d/", code);
        switch (code) {
            case 1: //Register
                p = strtok(NULL, "*");
                snprintf(name, 20, "%s", p);
                printf("Codigo: %d, Nombre: %s\n", code, name);
                p = strtok(NULL, "*");
                snprintf(password, 20, "%s", p);
                p = strtok(NULL, "*");
                snprintf(email, 20, "%s", p);
                res = register_user(conn, name, email, password);

                if (res == 1) {
                    strcat(response, "1");
                    strcpy(node->name, name);
                    int n = connected_to_string(list, datos);
                    push_connected(list, datos, n);
                } else if (res == -1)
                    strcat(response, "0");
                else
                    strcat(response, "2");
                break;
            case 2: //Login
                p = strtok(NULL, "*");
                strcpy(name, p);
                printf("Codigo: %d, Nombre: %s\n", code, name);
                p = strtok(NULL, "*");
                strcpy(password, p);
                res = login(conn, name, password);
                if (res != 0) {
                    strcat(response, "1");
                    node->id = res;
                    strcpy(node->name, name);
                    int n = connected_to_string(list, datos);
                    push_connected(list, datos, n);
                } else if (res == -1)
                    strcat(response, "0");
                else
                    strcat(response, "2");
                break;


            case 3: //Ranking
                n = Devuelveme_Ranking(datos);
                snprintf(response, 2000, "%d/%d/%s", code, n, datos);
                break;

            case 4: //Pedir online
                n = llist_to_string(list, datos);
                snprintf(response, 2000, "%d/%d/%s", code, n, datos);
                break;

            case 5: //Crear Partida
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
                    strcat(response, "1"); //todo bn
                else
                    strcat(response, "0"); //alguno de los usuarios no est� conectado



                break;


            default:
                printf("Invalid choice!\n");
                strcat(response, "error");
        }
        if (node->sending_connected == 1) {
            printf("waiting to finish sending connected");
            while (node->sending_connected == 1)
                99999 * 99999;
        }
        strcat(response, "\x04");
        printf("Respuesta: %s\n", response);
        write(sock_conn, response, strlen(response));
    }

    close(sock_conn);
    mysql_close(conn);
    pthread_mutex_lock(&mutex); //No me interrumpas ahora
    printf("Removing: %s %d", name, node->idx);
    remove_node_from_list(list, node->idx);
    print_idx(list);
    pthread_mutex_unlock(&mutex); //ya puedes interrumpirme

}

int main() {

    int sock_conn, sock_listen;
    struct sockaddr_in server_addr;
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        printf("socket error\n");

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;

    server_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    server_addr.sin_port = htons(9060);

    if (bind(sock_listen, (struct sockaddr *) &server_addr, sizeof(server_addr)) < 0)
        printf("Error al bind\n");

    if (listen(sock_listen, 3) < 0)
        printf("Error en el listen\n");

    pthread_t thread;
    ConnectedList list;
    initialize_list(&list);
    ThreadArgs threadArgs;
    threadArgs.list=&list;
    char res[200];
    int i;
    for (;;) {
        printf("Escuchando\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        printf("He recibido conexion\n");
        i=get_empty(&list);
        list.connections[i].sockfd=sock_conn;
        threadArgs.i=i;
        printf("%d\n", sock_conn);
        printf("%d\n", list.connections[i].sockfd);
        pthread_create(&thread, NULL, (void *(*)(void *))AtenderThread, &threadArgs);
        print_idx(&list);
    }


    return 0;
}
