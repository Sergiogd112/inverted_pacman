#include <mysql/mysql.h>
#include <stdio.h>
#include <string.h>
#include <openssl/sha.h>

#define SERVER "localhost"
#define USER "root"
#define PASSWORD "mysql"
#define DATABASE "InvertedPacman"

void register_user(MYSQL *conn) {
	char name[30], email[30], password[30], query[200];
	unsigned char hash[SHA256_DIGEST_LENGTH];
	char hash_string[SHA256_DIGEST_LENGTH*2+1];
	
	printf("Enter your name: ");
	scanf("%s", name);
	printf("Enter your email: ");
	scanf("%s", email);
	printf("Enter your password: ");
	scanf("%s", password);
	
	SHA256_CTX sha256;
	SHA256_Init(&sha256);
	SHA256_Update(&sha256, password,strlen(password));
	SHA256_Final(hash,&sha256);
	
	for (int i = 0; i < SHA256_DIGEST_LENGTH; i++)
		sprintf(hash_string + (i * 2), "%02x", hash[i]);
	
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
	
	if(mysql_num_rows(result)==0){
		sprintf(query,"INSERT INTO usuarios(nombre,correo,contraseña) VALUES('%s','%s','%s')",name,email,hash_string);
		
		if (mysql_query(conn, query)) {
			fprintf(stderr,"%s\n", mysql_error(conn));
			exit(1);
		}
		printf("User registered successfully!\n");
	}else{
		printf("A user with this name already exists!\n");
	}
}

void login(MYSQL *conn) {
	char email[30], password[30], query[200];
	unsigned char hash[SHA256_DIGEST_LENGTH];
	char hash_string[SHA256_DIGEST_LENGTH*2+1];
	
	printf("Enter your email: ");
	scanf("%s", email);
	printf("Enter your password: ");
	scanf("%s", password);
	
	SHA256_CTX sha256;
	SHA256_Init(&sha256);
	SHA256_Update(&sha256, password,strlen(password));
	SHA256_Final(hash,&sha256);
	
	for (int i = 0; i < SHA256_DIGEST_LENGTH; i++)
		sprintf(hash_string + (i * 2), "%02x", hash[i]);
	
	sprintf(query,"SELECT * FROM usuarios WHERE correo='%s' AND contraseña='%s'",email,hash_string);
	
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
	}else{
		printf("Login successful!\n");
	}
}

int main() {
	MYSQL *conn;
	conn = mysql_init(NULL);
	
	if (!mysql_real_connect(conn, SERVER, USER, PASSWORD, DATABASE, 0, NULL, 0)) {
		fprintf(stderr,"%s\n", mysql_error(conn));
		exit(1);
	}
	
	int choice;
	while(1){
		printf("1. Register\n2. Login\n3. Exit\nEnter your choice: ");
		scanf("%d",&choice);
		switch(choice){
		case 1:
			register_user(conn);
			break;
		case 2:
			login(conn);
			break;
		case 3:
			exit(0);
		default:
			printf("Invalid choice!\n");
		}
	}
	
	mysql_close(conn);
	
	return 0;
}
