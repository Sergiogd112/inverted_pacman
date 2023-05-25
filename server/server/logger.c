#include "logger.h"



pthread_mutex_t log_mutex = PTHREAD_MUTEX_INITIALIZER;

/**
 * Initializes the logger by opening the log file in append mode.
 * @param log_file_path The path to the log file.
 */
void logger_init(const char* log_file_path) {
    log_file = fopen(log_file_path, "a");
    if (log_file == NULL) {
        perror("Error opening log file");
        exit(EXIT_FAILURE);
    }
}

/**
 * Logs a message with the provided tag, file information, and message content.
 * @param tag The tag for the log message.
 * @param message The content of the log message.
 * @param file The name of the source file where the log message is generated.
 * @param line The line number in the source file where the log message is generated.
 * @param func The name of the function where the log message is generated.
 */
void _logger(const char* tag, const char* message, const char* file, int line, const char* func) {
    pthread_mutex_lock(&log_mutex); // Lock the mutex

    time_t now;
    time(&now);
    char* time_str = ctime(&now);
    time_str[strlen(time_str) - 1] = '\0'; // Remove newline character

    // Write to shell
    pid_t tid = syscall(SYS_gettid); // Get the thread ID
    printf("%s [%s] (TID: %ld) %s:%d:%s(): %s\n", ctime(&now), tag, (long)tid, file, line, func, message);

    // Write to log file
    if (log_file != NULL) {
        fprintf(log_file, "%s [%s] (TID: %ld) %s:%d:%s(): %s\n", ctime(&now), tag, (long)tid, file, line, func, message);
        fflush(log_file);
    }

    pthread_mutex_unlock(&log_mutex); // Unlock the mutex
}
#define BUFFER_SIZE 10
/**
 * Prints the last 10 log entries from the log file.
 */
void print_last_10_logs() {
    if (log_file == NULL) {
        perror("Error opening log file");
        exit(EXIT_FAILURE);
    }

    char buffer[BUFFER_SIZE][1024] = {0};
    int index = 0, count = 0;

    while (fgets(buffer[index], sizeof(buffer[index]), log_file)) {
        index = (index + 1) % BUFFER_SIZE;
        count++;
    }

    int start = count >= BUFFER_SIZE ? index : 0;
    int end = count >= BUFFER_SIZE ? BUFFER_SIZE : count;

    for (int i = start; i < end; i++) {
        printf("%s", buffer[i]);
    }

    fclose(log_file);
}
