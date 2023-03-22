#include "auth.h"


/**
 * Computes the SHA256 hash of a string.
 * @param string The input string to hash.
 * @return A pointer to a string that represents the SHA256 hash of the input string.
 */
char *to_sha256(char *string) {
    unsigned char digest[SHA256_DIGEST_LENGTH]; // Initialize an array of unsigned characters called digest with a length of SHA256_DIGEST_LENGTH.
    SHA256(string, strlen(string),
           digest); // Call the SHA256 function from the OpenSSL library to compute the SHA256 hash of the input string and store it in the digest array.
    char *result = malloc(SHA256_DIGEST_LENGTH * 2 +
                          1); // Allocate memory for a new string called result with a length of SHA256_DIGEST_LENGTH * 2 + 1.
    for (int i = 0; i < SHA256_DIGEST_LENGTH; i++) { // Iterate over each byte in the digest array.
        sprintf(&result[i * 2], "%02x",
                (unsigned int) digest[i]); // Convert each byte to a two-digit hexadecimal string representation using the sprintf function and store it in the result string.
    }
    return result; // Return a pointer to the result string.
}

/**
 * Registers a user in a MySQL database.
 * @param conn A pointer to a MySQL connection object.
 * @param name The user's name.
 * @param email The user's email.
 * @param password The user's password.
 * @return 1 if the user was registered successfully, -1 if the registration failed.
 */
int register_user(MYSQL *conn, char name[30], char email[30], char password[30]) {
    char query[200]; // Declare a character array called query with a length of 200.

    char *hash_string = to_sha256(
            password); // Call the to_sha256 function to compute the SHA256 hash of the password and store it in a new string called hash_string.

    sprintf(query, "SELECT * FROM usuarios WHERE nombre='%s'",
            name); // Construct a SQL query to check if a user with the same name already exists in the database and store it in the query string.

    if (mysql_query(conn,
                    query)) { // Execute the query using the mysql_query function and check if it returns an error.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }

    MYSQL_RES *result = mysql_store_result(conn); // Store the result of the query in a MYSQL_RES object.

    if (result == NULL) { // Check if the result is null.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }

    int num_fields = mysql_num_fields(result); // Get the number of fields in the result set.

    printf("Comprobando si ya esta registrado ese usuario"); // Print a message to the console.

    if (mysql_num_rows(result) == 0) { // Check if there are any rows in the result set.
        sprintf(query, "INSERT INTO usuarios(nombre,correo,password) VALUES('%s','%s','%s')", name, email,
                hash_string); // Construct a SQL query to insert the user's information into the database and store it in the query string.

        if (mysql_query(conn,
                        query)) { // Execute the query using the mysql_query function and check if it returns an error.
            fprintf(stderr, "%s\n", mysql_error(conn));
            exit(1);
        }

        printf("User registered successfully!\n"); // Print a message to the console.
        return 1; // Return 1 to indicate that the user was registered successfully.
    } else {
        printf("A user with this name already exists!\n"); // Print a message to the console.
        return -1; // Return -1 to indicate that the registration failed.
    }
}

/**
 * Logs a user into a MySQL database.
 * @param conn A pointer to a MySQL connection object.
 * @param name The user's name.
 * @param password The user's password.
 * @return 1 if the login was successful, 0 if the login failed.
 */
int login(MYSQL *conn, char name[30], char password[30]) {
    char query[200]; // Declare a character array called query with a length of 200.

    char *hash_string = to_sha256(
            password); // Call the to_sha256 function to compute the SHA256 hash of the password and store it in a new string called hash_string.

    sprintf(query, "SELECT * FROM usuarios WHERE nombre='%s' AND password='%s'", name,
            hash_string); // Construct a SQL query to check if a user with the given name and password exists in the database and store it in the query string.

    if (mysql_query(conn,
                    query)) { // Execute the query using the mysql_query function and check if it returns an error.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }

    MYSQL_RES *result = mysql_store_result(conn); // Store the result of the query in a MYSQL_RES object.

    if (result == NULL) { // Check if the result is null.
        fprintf(stderr, "%s\n", mysql_error(conn));
        exit(1);
    }

    int num_fields = mysql_num_fields(result); // Get the number of fields in the result set.

    if (mysql_num_rows(result) == 0) { // Check if there are any rows in the result set.
        printf("Invalid email or password\n"); // Print a message to the console.
        return 0; // Return 0 to indicate that the login failed.
    } else {
        printf("Login successful!\n"); // Print a message to the console.
        return 1; // Return 1 to indicate that the login was successful.
    }
}

