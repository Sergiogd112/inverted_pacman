#ifndef CONFIG_H
#define CONFIG_H
#include <stdio.h>

#define PDBSERVER "shiva2.upc.es"
#define LDBSERVER "localhost"

#define USER "root"
#define PASSWORD "mysql"
#define DATABASE "T1_InvertedPacman"
#define PORT 50053
#define MAXCHATMSGLEN 2000
#define MAXNOMBRELEN 20
#define NJUGADORESPARTIDA 4

typedef char Nombre[MAXNOMBRELEN];
#endif
