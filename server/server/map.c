//
// Created by sergiogd on 3/24/23.
//

#include "map.h"

/**
 * Converts the map data into a string representation.
 * @param map Pointer to the Map structure.
 * @param res The resulting string representation of the map.
 * @return 0 if successful, 1 if there is an invalid tile value.
 */
int to_str(struct Map *map, char res[(MAX_MAP_W + 1) * MAX_MAP_H])
{
    char temp[MAX_MAP_W];
    for (int i = 0; i < map->h; i++)
    {
        temp[0] = '\0';
        for (int j = 0; j < map->w; j++)
        {
            switch (map->tiles[i][j])
            {
            case 0:
                sprintf(temp, "%s%s", temp, "  ");
                break;
            case 1:
                sprintf(temp, "%s%s", temp, "||");
                break;
            default:
                return 1;
            }
        }
        sprintf(res, "%s%s\n", res, temp);
    }
    return 0;
}

/**
 * Checks if the given coordinates (x, y) are valid within the map dimensions.
 * @param map Pointer to the Map structure.
 * @param x The x-coordinate.
 * @param y The y-coordinate.
 * @return 1 if valid, 0 if not valid.
 */
int xy_valid(struct Map *map,int x,int y){
    return x>=0 && x<=map->w && y>=0 && y<=map->h;
}

/**
 * Retrieves the tile value at the given coordinates (x, y) from the map.
 * @param map Pointer to the Map structure.
 * @param x The x-coordinate.
 * @param y The y-coordinate.
 * @return The tile value at the given coordinates, or -1 if the coordinates are invalid.
 */
int get_tile(struct Map *map,int x,int y){
    if (xy_valid(map,x,y)==0)
        return -1;
    return map->tiles[y][x];

}

/**
 * Adds a wall at the given coordinates (x, y) in the map.
 * @param map Pointer to the Map structure.
 * @param x The x-coordinate.
 * @param y The y-coordinate.
 */
void add_wall(struct Map* map,int x,int y) {
    if (xy_valid(map, x, y) == 1)
        map->tiles[y][x]=1;
}