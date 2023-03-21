#include "auth.h"




int register_user(MYSQL *conn, char name[30], char email[30], char password[30]) {
	char query[200];
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
		return 1;
	}else{
		printf("A user with this name already exists!\n");
		return -1;
	}
}

int login(MYSQL *conn,char name[30], char password[30]) {
	char query[200];
	unsigned char hash[SHA256_DIGEST_LENGTH];
	char hash_string[SHA256_DIGEST_LENGTH*2+1];
	
	
	
	SHA256_CTX sha256;
	SHA256_Init(&sha256);
	SHA256_Update(&sha256, password,strlen(password));
	SHA256_Final(hash,&sha256);
	
	for (int i = 0; i < SHA256_DIGEST_LENGTH; i++)
		sprintf(hash_string + (i * 2), "%02x", hash[i]);
	
	sprintf(query,"SELECT * FROM usuarios WHERE nombre='%s' AND contraseña='%s'",name,hash_string);
	
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

