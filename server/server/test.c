//
// Created by antonia on 6/05/23.
//
#include <stdio.h>
#include <unistd.h>

int main() {
    char buf[1024];
    getlogin_r(buf, sizeof(buf));
    printf("Username: %s\n", buf);
    return 0;
}
