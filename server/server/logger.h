//
// Created by antonia on 25/04/23.
//

#ifndef SERVER_LOGGER_H
#define SERVER_LOGGER_H

#include <string.h>
#include <stdio.h>
#include <pthread.h>
#include <time.h>
#include <unistd.h>
#include <stdlib.h>

#define DATETIMELOGLEN 30
#define MAXLOGMSGLEN 2000
#define FILENAMEMAXLEN 30
#define FUNCTIONNAMEMAXLEN 30
#define LOGFILE "logfile.log"

/**
 * LogType - An enum representing the type of a log.
 */
enum LogType
{
    LOGINFO,
    LOGWARNING,
    LOGERROR
};

/**
 * LogElement - A structure that represents a single log element in the log queue.
 *
 * @param datetime: String representing the date and time of the log.
 * @param logType: Enum representing the type of the log (LOGINFO, LOGWARNING, LOGERROR).
 * @param message: String representing the log message.
 * @param next: Pointer to the next LogElement in the log queue.
 * @param filename: String representing the filename where the log originated.
 * @param functionname: String representing the function name where the log originated.
 * @param linen: Integer representing the line number where the log originated.
 */
struct LogElement
{
    char datetime[DATETIMELOGLEN];
    enum LogType logType;
    char message[MAXLOGMSGLEN];
    struct LogElement *next;
    char filename[FILENAMEMAXLEN];
    char functionname[FUNCTIONNAMEMAXLEN];
    int linen;
};

/**
 * LogTypeStrings - An array of strings representing the string representations of the LogType enum values.
 */
const char *LogTypeStrings[] = {"ⓘLOGINFO", "⚠LOGWARNING", "❌LOGERROR"};

/**
 * LogQueue - A structure that represents a log queue.
 *
 * @param head: Pointer to the head (front) of the log queue.
 * @param tail: Pointer to the tail (end) of the log queue.
 * @param count: Integer representing the number of elements in the log queue.
 */
typedef struct
{
    struct LogElement *head;
    struct LogElement *tail;
    int count;
} LogQueue;

pthread_mutex_t log_queue_mutex = PTHREAD_MUTEX_INITIALIZER; // Mutex for log queue access synchronization
int keeplog = 1;                                             // Global flag to control logging behavior

char *get_iso8601_datetime();

int enqueue(LogQueue *queue, char datetime[DATETIMELOGLEN], enum LogType logType, char message[MAXLOGMSGLEN],
            char filename[FILENAMEMAXLEN], char functionname[FUNCTIONNAMEMAXLEN], int linen);
int dequeue(LogQueue *queue, struct LogElement *element);

void logthreadconsole(LogQueue *queue);

void logthreadfile(LogQueue *queue);

void logthreadboth(LogQueue *queue);

#endif // SERVER_LOGGER_H
