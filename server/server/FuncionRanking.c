int Devuelveme_Ranking(char ID[20],char Nombre[20],int Puntos)
{
	MYSQL *conn;
	int err;
	
	//Creamos una conexion al servidor MYSQL
	conn = mysql_init(NULL);
	if (conn==NULL) 
	{
		printf ("Error al crear la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	//Inicializar la conexion
	conn = mysql_real_connect (conn, "localhost","root", "mysql", "partidas_usuarios",0, NULL, 0);
	if (conn==NULL) 
	{
		printf ("Error al inicializar la conexion: %u %s\n",
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	
	
	//Consulta para recuperar valores de las columnas
	mysql *result = mysqlexec(conn, "SELECT ID, puntos, nombre FROM usuarios ORDER BY puntos DESC");
	
	//Compruebo si la consulta ha ido bien
	if (mysqlresultStatus(result) != mysqlRES_TUPLES_OK)
	{
		fprintf(stderr, "Error de consulta: %s", mysqlerrorMessage(conn));
		mysqlclear(result);
		mysqlfinish(conn);
		exit(1);
	}
	
	//Creo listas para cada columna
	int num_rows = mysqlntuples(result);
	char **ID = malloc(sizeof(char*) * num_rows);
	char **Nombre = malloc(sizeof(char*) * num_rows);
	int *Puntos = malloc(sizeof(int) * num_rows);
	
	//Agregar los valores de cada columna a las listas
	for (int i=0; i < num_rows; i++)
	{
		ID[] = strdup(mysqlgetvalue(result, i, 0));
		Nombre[] = strdup(mysqlgetvalue(result, i, 1));
		Puntos[] = strdup(mysqlgetvalue(result, i, 2));
	}
	
	//Liberamos memoria
	mysqlclear(result);
	mysqlfinish(conn);
	
	return 0;
	
	mysql_close (conn);
	exit(0);
}