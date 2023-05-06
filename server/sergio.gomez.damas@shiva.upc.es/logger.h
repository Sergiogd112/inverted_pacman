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
#define LOGINFO "ⓘLOGINFO"
#define LOGWARNING "⚠LOGWARNING"
#define LOGERROR "❌LOGERROR"


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
    char logType[20];
    char message[MAXLOGMSGLEN];
    struct LogElement *next;
    char filename[FILENAMEMAXLEN];
    char functionname[FUNCTIONNAMEMAXLEN];
    int linen;
};

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
    int keeplog;                                             // Global flag to control logging behavior

} LogQueue;


char *get_iso8601_datetime();

int enqueue(LogQueue *queue, char datetime[DATETIMELOGLEN], char logType[20], char message[MAXLOGMSGLEN],
            char filename[FILENAMEMAXLEN], char functionname[FUNCTIONNAMEMAXLEN], int linen);
int dequeue(LogQueue *queue, struct LogElement *element);

void logthreadconsole(LogQueue *queue);

void logthreadfile(LogQueue *queue);

void logthreadboth(LogQueue *queue);

#endif // SERVER_LOGGER_H
