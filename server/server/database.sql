DROP DATABASE IF EXISTS InvertedPacman;
CREATE DATABASE InvertedPacman;
USE InvertedPacman;
CREATE TABLE usuarios (
    ID int PRIMARY KEY NOT NULL AUTO_INCREMENT,
    nombre varchar(256) NOT NULL,
    correo varchar(256) NOT NULL,
    password varchar(256) NOT NULL,
    puntos int DEFAULT 0
);
CREATE TABLE partidas (
    id_partida int PRIMARY KEY NOT NULL AUTO_INCREMENT,
    puntuacion_global int NOT NULL
);
CREATE TABLE partidas_usuarios (
    id_partida int NOT NULL,
    id_usuario int NOT NULL,
    puntuacion int NOT NULL,
    FOREIGN KEY (id_partida) REFERENCES partidas(id_partida),
    FOREIGN KEY (id_usuario) REFERENCES usuarios(ID)
);

INSERT INTO usuarios VALUES(1,"sergio","pera@gmail.com","8d69eac583367b7ecf431a857fc8e7903ff1ed988e8939fd0e92086dcfc1f98a",100+80+20+10+15+16);
INSERT INTO usuarios VALUES(2,"Ana","ana@gmail.com","8d69eac583367b7ecf431a857fc8e7903ff1ed988e8939fd0e92086dcfc1f98a",100+80+20+20+25+16);
INSERT INTO usuarios VALUES(3,"Carlos","Carlos@gmail.com","8d69eac583367b7ecf431a857fc8e7903ff1ed988e8939fd0e92086dcfc1f98a",100+80+20+30+35+16);
INSERT INTO usuarios VALUES(4,"pepa","george@gmail.com","f0b8c9d84dd2b877e0b952130b73e218106fec04c23852271d390213a1ff96f4",100+80+20+30+35+16);

INSERT INTO partidas VALUES(1,100);
INSERT INTO partidas VALUES(2,80);
INSERT INTO partidas VALUES(3,20);

INSERT INTO partidas_usuarios VALUES(1,1,10);
INSERT INTO partidas_usuarios VALUES(1,2,20);
INSERT INTO partidas_usuarios VALUES(1,3,30);
INSERT INTO partidas_usuarios VALUES(1,4,40);
INSERT INTO partidas_usuarios VALUES(2,1,15);
INSERT INTO partidas_usuarios VALUES(2,2,25);
INSERT INTO partidas_usuarios VALUES(2,3,35);
INSERT INTO partidas_usuarios VALUES(2,4,45);
INSERT INTO partidas_usuarios VALUES(3,1,16);
INSERT INTO partidas_usuarios VALUES(3,2,16);
INSERT INTO partidas_usuarios VALUES(3,3,16);
INSERT INTO partidas_usuarios VALUES(3,4,16);

