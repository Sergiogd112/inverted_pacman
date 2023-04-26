//
// Created by antonia on 25/04/23.
//

#include "logger.h"


char *get_iso8601_datetime() {
    static char buffer[30];
    time_t rawtime;
    struct tm *timeinfo;

    time(&rawtime);
    timeinfo = localtime(&rawtime);

    strftime(buffer, sizeof(buffer), "%Y-%m-%dT%H:%M:%S%z", timeinfo);

    return buffer;
}

int enqueue(LogQueue *queue, char datetime[DATETIMELOGLEN], enum LogType logType, char message[MAXLOGMSGLEN],
            char filename[FILENAMEMAXLEN], char functionname[FUNCTIONNAMEMAXLEN], int linen) {

    struct LogElement new_element;
    snprintf(new_element.datetime, DATETIMELOGLEN, "%s", datetime);
    new_element.logType = logType;
    snprintf(new_element.message, MAXLOGMSGLEN, "%s", message);
    new_element.next = NULL;
    snprintf(new_element.filename, FILENAMEMAXLEN, "%s", filename);
    snprintf(new_element.functionname, FUNCTIONNAMEMAXLEN, "%s", functionname);
    new_element.linen = linen;
    pthread_mutex_lock(&log_queue_mutex);
    if (queue->head == NULL) {
        queue->head = &new_element;
    } else
        queue->tail->next = &new_element;
    queue->tail = &new_element;
    queue->count++;
    pthread_mutex_unlock(&log_queue_mutex);

    return 0;
}

int dequeue(LogQueue *queue, struct LogElement * element) {
    if (queue->count <= 0) {
        return 1;
    }
    element = queue->head;
    pthread_mutex_lock(&log_queue_mutex);
    queue->head = queue->head->next;
    if (queue->count == 1)
        queue->tail = NULL;
    queue->count--;
    pthread_mutex_lock(&log_queue_mutex);
    return 0;
}

void print_logelement(struct LogElement *element) {
    printf("[%s][%s:%s:%d][%s] %s", element->datetime, element->filename, element->functionname, element->linen,
           LogTypeStrings[element->logType], element->message);
}

void logthreadconsole(LogQueue *queue){
    struct LogElement element;
    while(1==1){
        if (queue->count>0){
            dequeue(queue,&element);
            print_logelement(&element);

        }
    }
}


