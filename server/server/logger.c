//
// Created by antonia on 25/04/23.
//

#include "logger.h"
pthread_mutex_t log_queue_mutex = PTHREAD_MUTEX_INITIALIZER; // Mutex for log queue access synchronization

/**
 * get_iso8601_datetime - A function that returns the current datetime in ISO 8601 format.
 *
 * @return: Pointer to a static buffer containing the formatted datetime string.
 */
char *get_iso8601_datetime()
{
    static char buffer[30]; // Static buffer to store the formatted datetime string
    time_t rawtime; // Variable to store the raw time value
    struct tm *timeinfo; // Pointer to a tm struct to store the broken-down time information

    time(&rawtime); // Get the current time in seconds since epoch
    timeinfo = localtime(&rawtime); // Convert the rawtime to the local time

    strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%S%z", timeinfo); // Format the datetime string in ISO 8601 format

    return buffer; // Return the pointer to the formatted datetime string
}


/**
 * enqueue - A function that adds a log element to the end of a LogQueue.
 *
 * @param queue: Pointer to the LogQueue to enqueue the log element.
 * @param datetime: Datetime string to be stored in the log element.
 * @param logType: Log type enum to be stored in the log element.
 * @param message: Log message string to be stored in the log element.
 * @param filename: Filename string to be stored in the log element.
 * @param functionname: Function name string to be stored in the log element.
 * @param linen: Line number to be stored in the log element.
 * @return: 0 on success.
 */
int enqueue(LogQueue *queue, char datetime[DATETIMELOGLEN], char logType[20], char message[MAXLOGMSGLEN],
            char filename[FILENAMEMAXLEN], char functionname[FUNCTIONNAMEMAXLEN], int linen)
{

    struct LogElement new_element; // LogElement struct to store the new log element
    snprintf(new_element.datetime, DATETIMELOGLEN, "%s", datetime); // Copy the datetime string to the log element
    snprintf(new_element.logType, 20, "%s", logType); // Copy the logType string to the log element
    snprintf(new_element.message, MAXLOGMSGLEN, "%s", message); // Copy the log message string to the log element
    new_element.next = NULL; // Set the next pointer of the log element to NULL
    snprintf(new_element.filename, FILENAMEMAXLEN, "%s", filename); // Copy the filename string to the log element
    snprintf(new_element.functionname, FUNCTIONNAMEMAXLEN, "%s", functionname); // Copy the function name string to the log element
    new_element.linen = linen; // Set the line number of the log element
    pthread_mutex_lock(&log_queue_mutex); // Lock the mutex for thread safety
    if (queue->head == NULL)
    {
        queue->head = &new_element; // If the queue is empty, set the head of the queue to the new log element
    }
    else
        queue->tail->next = &new_element; // If the queue is not empty, set the next pointer of the tail to the new log element
    queue->tail = &new_element; // Set the tail of the queue to the new log element
    queue->count++; // Increment the count of logs in the queue
    pthread_mutex_unlock(&log_queue_mutex); // Unlock the mutex

    return 0; // Return 0 to indicate success
}

/**
 * dequeue - A function that removes the head log element from a LogQueue.
 *
 * @param queue: Pointer to the LogQueue to dequeue the log element from.
 * @param element: Pointer to a LogElement struct to store the dequeued log element.
 * @return: 0 on success, 1 if the queue is empty.
 */
int dequeue(LogQueue *queue, struct LogElement *element)
{
    if (queue->count <= 0)
    {
        return 1; // Return 1 to indicate that the queue is empty
    }
    element = queue->head; // Store the head of the queue in the element pointer
    pthread_mutex_lock(&log_queue_mutex); // Lock the mutex for thread safety
    queue->head = queue->head->next; // Update the head of the queue to the next element
    if (queue->count == 1)
        queue->tail = NULL; // If there was only one element in the queue, set the tail to NULL
    queue->count--; // Decrement the count of logs in the queue
    pthread_mutex_lock(&log_queue_mutex); // Unlock the mutex

    return 0; // Return 0 to indicate success
}

/**
 * print_logelement - A function that prints the details of a LogElement.
 *
 * @param element: Pointer to a LogElement struct to be printed.
 */
void print_logelement(struct LogElement *element)
{
    printf("[%s][%s:%s:%d][%s] %s", element->datetime, element->filename, element->functionname, element->linen,
           element->logType, element->message);
    // Print the log element details using formatted string
    // element->datetime: datetime string
    // element->filename: filename string
    // element->functionname: functionname string
    // element->linen: line number
    // LogTypeStrings[element->logType]: log type string corresponding to log type enum
    // element->message: log message string
}

/**
 * string_logelement - A function that formats a LogElement into a string.
 *
 * @param element: Pointer to a LogElement struct to be converted to string.
 * @param res: Pointer to a char buffer to store the formatted string.
 */
void string_logelement(struct LogElement *element, char res[DATETIMELOGLEN + MAXLOGMSGLEN + FILENAMEMAXLEN + FUNCTIONNAMEMAXLEN])
{
    snprintf(res, DATETIMELOGLEN + MAXLOGMSGLEN + FILENAMEMAXLEN + FUNCTIONNAMEMAXLEN,
             "[%s][%s:%s:%d][%s] %s", element->datetime, element->filename, element->functionname, element->linen,
             element->logType, element->message);
    // Format the LogElement struct into a string using formatted string
    // element->datetime: datetime string
    // element->filename: filename string
    // element->functionname: functionname string
    // element->linen: line number
    // LogTypeStrings[element->logType]: log type string corresponding to log type enum
    // element->message: log message string
    // Store the formatted string in the char buffer res
}

/**
 * logthreadconsole - A function that logs elements from a LogQueue to console.
 *
 * @param queue: Pointer to a LogQueue struct from which log elements are dequeued and printed to console.
 */
void logthreadconsole(LogQueue *queue)
{
    struct LogElement element; // Create a local LogElement struct to store dequeued log element
    while (queue->keeplog==1) // Continue logging until keeplog flag is false
    {
        if (queue->count > 0) // Check if there are log elements in the queue
        {
            dequeue(queue, &element); // Dequeue a log element from the queue
            print_logelement(&element); // Print the dequeued log element to console
        }
    }
}

/**
 * logthreadfile - A function that logs messages from a LogQueue to a file.
 *
 * @param queue: Pointer to the LogQueue containing log messages.
 * @return: None.
 */
void logthreadfile(LogQueue *queue)
{
    char res[DATETIMELOGLEN + MAXLOGMSGLEN + FILENAMEMAXLEN + FUNCTIONNAMEMAXLEN]; // Buffer to store formatted log message
    struct LogElement element; // LogElement struct to store dequeued log message

    while (queue->keeplog==1) // Keep logging while the global variable 'keeplog' is true
    {
        if (queue->count > 0) // If there are logs in the queue
        {
            FILE *fp = fopen(LOGFILE, "a"); // Open the log file in append mode
            if (fp == NULL)
            {
                printf("Failed to open file\n"); // Print an error message if failed to open file
                continue; // Continue to the next iteration of the loop
            }
            dequeue(queue, &element); // Dequeue a log message from the queue
            string_logelement(&element, res); // Format the log message into a string
            fprintf(fp, "%s\n", res); // Write the formatted log message to the file
            print_logelement(&element); // Print the log message to the console
            fclose(fp); // Close the file
        }
    }
}


/**
 * logthreadboth - A function that logs elements from a LogQueue to both console and a file.
 *
 * @param queue: Pointer to a LogQueue struct from which log elements are dequeued and logged to console and file.
 */
void logthreadboth(LogQueue *queue)
{
    char res[DATETIMELOGLEN + MAXLOGMSGLEN + FILENAMEMAXLEN + FUNCTIONNAMEMAXLEN]; // Define a char array to store the log element as a string
    struct LogElement element; // Create a local LogElement struct to store dequeued log element

    while (queue->keeplog==1) // Continue logging until keeplog flag is false
    {
        if (queue->count > 0) // Check if there are log elements in the queue
        {
            FILE *fp = fopen(LOGFILE, "a"); // Open the log file in append mode
            if (fp == NULL) // Check if file opening failed
            {
                printf("Failed to open file\n");
                continue; // Skip to the next iteration of the loop
            }
            dequeue(queue, &element); // Dequeue a log element from the queue
            string_logelement(&element, res); // Convert the dequeued log element to a string
            fprintf(fp, "%s\n", res); // Write the log element string to the file
            print_logelement(&element); // Print the dequeued log element to console
            fclose(fp); // Close the log file
        }
    }
}


/**
 * print_last_n_lines_of_a_file - A function that prints the last n lines of a file.
 *
 * @param filename: Pointer to a string containing the name of the file to read.
 * @param n: Number of lines to print from the end of the file.
 */
void print_last_n_lines_of_a_file(char *filename, int n) {
    FILE *fp = fopen(filename, "r"); // Open the file in read mode
    if (fp == NULL) { // Check if file opening failed
        printf("Failed to open file\n");
        return;
    }

    int count = 0;
    char c;

    // Count number of lines in the file
    for (c = getc(fp); c != EOF; c = getc(fp)) {
        if (c == '\n') {
            count++;
        }
    }

    // Reset file pointer to the beginning of the file
    fseek(fp, 0, SEEK_SET);

    // Skip first (count - n) lines
    for (int i = 0; i < count - n; i++) {
        while ((c = getc(fp)) != '\n' && c != EOF); // Skip to the end of each line
    }

    // Print last n lines
    while ((c = getc(fp)) != EOF) {
        putchar(c); // Print character to console
    }

    fclose(fp); // Close the file
}
