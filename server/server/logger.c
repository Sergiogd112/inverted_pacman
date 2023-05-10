#include "logger.h"



pthread_mutex_t log_mutex = PTHREAD_MUTEX_INITIALIZER;

void logger_init(const char* log_file_path) {
    log_file = fopen(log_file_path, "a");
    if (log_file == NULL) {
        perror("Error opening log file");
        exit(EXIT_FAILURE);
    }
}
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
