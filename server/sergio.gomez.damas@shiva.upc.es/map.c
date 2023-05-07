//
// Created by sergiogd on 3/24/23.
//

#include "map.h"

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
int xy_valid(struct Map *map,int x,int y){
    return x>=0 && x<=map->w && y>=0 && y<=map->h;
}
int get_tile(struct Map *map,int x,int y){
    if (xy_valid(map,x,y)==0)
        return -1;
    return map->tiles[y][x];

}
void add_wall(struct Map* map,int x,int y) {
    if (xy_valid(map, x, y) == 1)
        map->tiles[y][x]=1;
}