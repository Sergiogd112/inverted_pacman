#include "main.h"

void *AtenderThread(struct Node *thread_args[2]){
    int sock_conn=thread_args[1]->sockfd;
    struct Node * head=&thread_args[0];
    struct Node * node=&thread_args[1];

    MYSQL *conn;
    char request[512];
    char response[1010];
    int  ret;
    char name[20];
    conn = mysql_init(NULL);
    char user[30], email[30], password[30];
    if (!mysql_real_connect(conn, SERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    char datos[1000];
    int n;
    char id1[20];
    char id2[20];
    char id3[20];
    char id4[20];
    while (1 == 1) {
        printf("Esperando peticion\n");
        ret = read(sock_conn, request, sizeof(request));

        printf("Recibido\n");

        request[ret] = '\0';

        printf("Peticion: %s\n", request);

        char *p = strtok(request, "/");

        int code = atoi(p); //convierte el string p al entero codigo
        char user[20];
        int res;
        if (code == 0) {
            //desconectar
            break;
        }
        p = strtok(NULL, "/");
        strcpy(name, p);
        printf("Codigo: %d, Nombre: %s\n", code, name);
        switch (code) {
            case 1: //Register
                p = strtok(NULL, "*");
                strcpy(email, "patata@gmail.com");
                //p = strtok(NULL, "*");
                strcpy(password, p);
                res = register_user(conn, name, email, password);
                if (res == 1)
                    strcpy(response, "1");
                else if (res == -1)
                    strcpy(response, "0");
                else
                    strcpy(response, "2");
                break;
            case 2: //Login
                p = strtok(NULL, "/");
                strcpy(password, p);
                res = login(conn, name, password);
                if (res != 0) {
                    strcpy(response, "1");
                    node->id = res;
                }
                else if (res == -1)
                    strcpy(response, "0");
                else
                    strcpy(response, "2");
                break;

            
            case 3: //Ranking
                n = Devuelveme_Ranking(datos);
                sprintf(response, "%d/%s", n, datos);
                break;
            
            case 4: //Pedir online
                n = llist_to_string(head,datos);
                sprintf(response, "%d/%s", n, datos);
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


                if (search_on_llist(head,atoi(id1)) != -1 && search_on_llist(head,atoi(id2)) != -1 && search_on_llist(head,atoi(id3)) != -1 && search_on_llist(head,atoi(id4)) != -1)
                    strcpy(response, "1"); //todo bn
                else
                    strcpy(response, "0"); //alguno de los usuarios no est� conectado



                break;


            default:
                printf("Invalid choice!\n");
        }
        printf("Respuesta: %s\n", response);
        write(sock_conn, response, strlen(response));
    }

    close(sock_conn);
    mysql_close(conn);
}

int main() {

    int sock_conn, sock_listen;
    struct sockaddr_in server_addr;
    if ((sock_listen = socket(AF_INET, SOCK_STREAM, 0)) < 0)
        printf("socket error\n");

    memset(&server_addr, 0, sizeof(server_addr));
    server_addr.sin_family = AF_INET;

    server_addr.sin_addr.s_addr = htonl(INADDR_ANY);
    server_addr.sin_port = htons(9050);

    if (bind(sock_listen, (struct sockaddr *) &server_addr, sizeof(server_addr)) < 0)
        printf("Error al bind\n");

    if (listen(sock_listen, 3) < 0)
        printf("Error en el listen\n");
    struct Node node;
    int idx;
    pthread_t thread;
    int i = 0;
    struct Node head = node;
    struct Node * thread_args[2];
    thread_args[0]=&head;
    for (;;) {
        printf("Escuchando\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        printf("He recibido conexion\n");
        struct Node* new_node = (struct Node*)malloc(sizeof(struct Node));
        // assign data to new node
        new_node->id = -1;
        new_node->sockfd = sock_conn;
        strcpy(new_node->name, "name");

        // make next of new node as NULL and prev as last node
        new_node->next = NULL;

        idx= append_to_llist(&head,new_node);
        thread_args[1]=&new_node;
        pthread_create(&thread, NULL, AtenderThread, thread_args);
        i++;
    }


    return 0;
}
