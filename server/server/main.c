#include "main.h"

int main() {
    MYSQL *conn;
    conn = mysql_init(NULL);
    char name[30], email[30], password[30];
    if (!mysql_real_connect(conn, SERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }
    int sock_conn, sock_listen, ret;
    struct sockaddr_in server_addr;
    char request[512];
    char response[512];
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

    for (int i = 0; i < 5; i++) {
        printf("Escuchando\n");

        sock_conn = accept(sock_listen, NULL, NULL);
        printf("He recibido conexion\n");
        while (1 == 1) {
            printf("Esperando peticion\n");
            ret = read(sock_conn, request, sizeof(request));

            printf("Recibido\n");

            request[ret] = '\0';

            printf("Peticion: %s\n", request);

            char *p = strtok(request, "/");

            int code = atoi(p);
            char name[20];
            int res;
            if (code == 0) {
                break;
            }
            p = strtok(NULL, "/");
            strcpy(name, p);
            printf("Codigo: %d, Nombre: %s\n", code, name);
            switch (code) {
                case 1:
                    p = strtok(NULL, "/");
                    strcpy(email, p);
                    p = strtok(NULL, "/");
                    strcpy(password, p);
                    res = register_user(conn, name, email, password);
                    if (res == 1)
                        strcpy(response, "OK");
                    else if (res == -1)
                        strcpy(response, "Existe");
                    else
                        strcpy(response, "Error");
                    break;
                case 2:
                    p = strtok(NULL, "/");
                    strcpy(password, p);
                    res = login(conn, name, password);
                    if (res == 1)
                        strcpy(response, "OK");
                    else if (res == -1)
                        strcpy(response, "Incorrecto");
                    else
                        strcpy(response, "Error");
                    break;

                default:
                    printf("Invalid choice!\n");
            }
            printf("Respuesta: %s\n", response);
            write(sock_conn, response, strlen(response));
        }

        close(sock_conn);
    }

    mysql_close(conn);

    return 0;
}
