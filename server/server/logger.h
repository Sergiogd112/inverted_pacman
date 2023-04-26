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


enum LogType {
    LOGINFO,
    LOGWARNING,
    LOGERROR
};

const char *LogTypeStrings[] = {"ⓘLOGINFO", "⚠LOGWARNING", "❌LOGERROR"};

struct LogElement {
    char datetime[DATETIMELOGLEN];
    enum LogType logType;
    char message[MAXLOGMSGLEN];
    struct LogElement *next;
    char filename[FILENAMEMAXLEN];
    char functionname[FUNCTIONNAMEMAXLEN];
    int linen;
};

typedef struct {
    struct LogElement *head;
    struct LogElement *tail;
    int count;
} LogQueue;

pthread_mutex_t log_queue_mutex = PTHREAD_MUTEX_INITIALIZER;

char *get_iso8601_datetime();

#endif //SERVER_LOGGER_H
