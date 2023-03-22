#include "auth.h"


char* to_sha256(char* string) {
    unsigned char digest[SHA256_DIGEST_LENGTH];
    SHA256(string, strlen(string), digest);
    char* result = malloc(SHA256_DIGEST_LENGTH * 2 + 1);
    for(int i = 0; i < SHA256_DIGEST_LENGTH; i++) {
        sprintf(&result[i * 2], "%02x", (unsigned int)digest[i]);
    }
    return result;
}


int register_user(MYSQL *conn, char name[30], char email[30], char password[30]) {
	char query[200];
	unsigned char hash[SHA256_DIGEST_LENGTH];


    char* hash_string = to_sha256(password);
	
	sprintf(query,"SELECT * FROM usuarios WHERE nombre='%s'",name);
	
	if (mysql_query(conn, query)) {
		fprintf(stderr,"%s\n", mysql_error(conn));
		exit(1);
	}
	
	MYSQL_RES *result = mysql_store_result(conn);
	
	if (result == NULL) {
		fprintf(stderr,"%s\n", mysql_error(conn));
		exit(1);
	}
	
	int num_fields = mysql_num_fields(result);
	printf("Comprobando si ya esta registrado ese usuario");
	if(mysql_num_rows(result)==0){
		sprintf(query,"INSERT INTO usuarios(nombre,correo,password) VALUES('%s','%s','%s')",name,email,hash_string);
		
		if (mysql_query(conn, query)) {
			fprintf(stderr,"%s\n", mysql_error(conn));
			exit(1);
		}
		printf("User registered successfully!\n");
		return 1;
	}else{
		printf("A user with this name already exists!\n");
		return -1;
	}
}

int login(MYSQL *conn,char name[30], char password[30]) {
	char query[200];
	unsigned char hash[SHA256_DIGEST_LENGTH];



    char* hash_string = to_sha256(password);
	sprintf(query,"SELECT * FROM usuarios WHERE nombre='%s' AND password='%s'",name,hash_string);
	
	if (mysql_query(conn, query)) {
		fprintf(stderr,"%s\n", mysql_error(conn));
		exit(1);
	}
	
	MYSQL_RES *result = mysql_store_result(conn);
	
	if (result == NULL) {
		fprintf(stderr,"%s\n", mysql_error(conn));
		exit(1);
	}
	
	int num_fields = mysql_num_fields(result);
	
	if(mysql_num_rows(result)==0){
		printf("Invalid email or password\n");
		return 0;
	}else{
		printf("Login successful!\n");
		return 1;
	}
}

