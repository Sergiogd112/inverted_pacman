//Progarma en C para la creacion de un metodo para introducir 
// un partido a nuestra base de datos...
#include <mysql.h>
#include <string.h>
#include <stdlib.h>
#include <stdio.h>


void introducir_partido(int *lista_id_usuario, int *lista_puntuaciones, int num_valores, **argv) //num_valores es la cantidas de usuarios-puntuaciones que añadira.
{
	MYSQL *conn;
	int err;
	
	//Creamos una conexion al servidor MYSQL 
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//inicializar la conexion, entrando nuestras claves de acceso y
	//el nombre de la base de datos a la que queremos acceder 
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "partidas_usuarios",0, NULL, 0);
	if (conn==NULL) {
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	int suma = 0; // inicializar la variable suma
	
	// Blucle para calcular la suma de puntuaciones
	for (int i = 0; i < num_valores; i++) {
		suma += lista_puntuaciones[i];
	}
	
	sprintf(consulta, "INSERT INTO partidas (puntuacion_global) VALUES (%d)", suma);
	err = mysql_query(conn, consulta);
	if (err != 0) {
		printf ("Error al introducir datos la base %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit(1);
	}
	
	int id_partida = mysql_insert_id(conn);
	
	// Blucle para meter todos los valores a la tabla partidas_usuarios
	for (int i = 0; i < num_valores; i++) {
			sprintf(consulta, "INSERT INTO partidas_usuarios (id_partida, id_usuario, puntuacion) VALUES (%d, %d, %d)", id_partida, lista_id_usuario[i], lista_puntuaciones[i]);
		err = mysql_query(conn, consulta);
		if (err != 0) {
			printf ("Error al introducir datos la base %u %s\n",
					mysql_errno(conn), mysql_error(conn));
			exit(1);
		}
		
	}
}
// cerrar la conexion con el servidor MYSQL 
mysql_close (conn);
exit(0);
}


		
		
