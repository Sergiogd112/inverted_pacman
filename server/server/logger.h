//
// Created by antonia on 25/04/23.
//
#ifndef LOGGER_H
#define LOGGER_H
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <pthread.h>
#include <string.h>
#include <unistd.h>
#include <sys/syscall.h>
static FILE* log_file = NULL;
#define LOGFILE "logfile.log"
#define ANSI_COLOR_BLUE    "\x1b[34m"
#define ANSI_COLOR_YELLOW  "\x1b[33m"
#define ANSI_COLOR_RED     "\x1b[31m"
#define ANSI_COLOR_RESET   "\x1b[0m"

#define LOGINFO ANSI_COLOR_BLUE "ⓘ LOGINFO" ANSI_COLOR_RESET
#define LOGWARNING ANSI_COLOR_YELLOW "⚠ LOGWARNING" ANSI_COLOR_RESET
#define LOGERROR ANSI_COLOR_RED "❌ LOGERROR" ANSI_COLOR_RESET
void logger_init(const char* log_file);
//void logger(const char* tag, const char* message);
#define logger(tag, message) _logger(tag, message, __FILE__, __LINE__, __func__)

#endif /* LOGGER_H */
