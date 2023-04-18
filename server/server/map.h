//
// Created by sergiogd on 3/24/23.
//

#ifndef SERVER_MAP_H
#define SERVER_MAP_H
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#define MAX_MAP_H 200
#define MAX_MAP_W 200
struct Map
{
    int tiles[MAX_MAP_H][MAX_MAP_W];
    int h;
    int w;
};
int to_str(struct Map *map, char res[(MAX_MAP_W + 1) * MAX_MAP_H]);
int xy_valid(struct Map* map,int x,int y);
int get_tile(struct Map* map,int x,int y);
#endif // SERVER_MAP_H
