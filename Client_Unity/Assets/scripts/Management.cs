using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;



public class Management : MonoBehaviour
{
    public Client cliente;
    public float radiodeDeteccion = 1.4f; //radio desde el cual el jugador ya puede matar
    public float tiemporespawnslime = 3f;
    public float tiemporespawnplayer = 4f;

    public int numslimes = 4; //variable para definir los slime que habrán el la partida
    public int numplayers = 4; //variable para definir los jugadores online que habrán el la partida


    private float[,] distanceMatrix; // Declaración de la matriz

    private float[,] distanceparedMatrix; // Declaración de la matriz de distancias a las paredes
    private int[,] paredMatrix; // Declaración de la matriz de si toca o no toca pared
    private int[,] nearestMatrix; // Declaración de la matriz de jugador más cercano a cada slime

    //public Slime_Movement slime;
    //public PlayerMovement player;
    public GameObject playerprefab;

    private Rigidbody2D rb2d;


    GameObject[] player = new GameObject[4];
    GameObject[] slime = new GameObject[4];
    public MainGameManager mainGameManager;


    //Estos dos atributos serán para detectar si hay paredes alrededor
    public float raycastDistance = 0f;
    public string tagPared = "Pared";

    //Tiempo transcurrido
    public float tiempoPartida = 0f;

    //Tiempo final cuando acaba el juego

    public TextMeshProUGUI textoMeshPro;
    public float tiempoFinal = 60f;
    public TextMeshProUGUI name1;
    public TextMeshProUGUI name2;
    public TextMeshProUGUI name3;
    public TextMeshProUGUI name4;





    void Start()
    {
        for (int i = 1; i <= numplayers; i++)
        {
            player[i - 1] = GameObject.Find("Player" + i.ToString());
        }

        for (int i = 1; i <= numslimes; i++)
        {
            slime[i - 1] = GameObject.Find("Slime" + i.ToString());
        }
        //InvokeRepeating("printMatrix3", 0.5f, 3f);

        //slime = GameObject.Find("Slime1"); //antigua forma para cuando solo había 1 slime
        //InvokeRepeating("printMatrix2", 0.01f, 4f);

        //Debug.Log("Los nombres: " + mainGameManager.names[0]);

        nombres();

    
    }

    // Update is called once per frame
    void Update()
    {

        //SlimeMovement slimemov = slime.GetComponent<SlimeMovement>();
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];
        
        calculateDistances();
        calculateNearest();
        
        slimesRojos();
        muerteSlime();
        muertePlayer();
        

        paredes(0.12f);
        aporelplayer();


        if(tiempoPartida < tiempoFinal)
        {
            tiempoPartida += Time.deltaTime;
            if(tiempoPartida > tiempoFinal){
                Debug.Log("Se ha acabado el tiempo");
            }
        }
        
    }



    void nombres(){
        name1 = GameObject.Find("Name1").GetComponent<TextMeshProUGUI>();
        name2 = GameObject.Find("Name2").GetComponent<TextMeshProUGUI>();
        name3 = GameObject.Find("Name3").GetComponent<TextMeshProUGUI>();
        name4 = GameObject.Find("Name4").GetComponent<TextMeshProUGUI>();

        /*
        if (name1 != null && name2 != null && name3 != null && name4 != null)
        {
            name1.text = mainGameManager.names[0];
            name2.text = mainGameManager.names[1];
            name3.text = mainGameManager.names[2];
            name4.text = mainGameManager.names[3];
        }*/

  
        name1.text = "Jaume";
        name2.text = "Teo";
        name3.text = "Sergio";
        name4.text = "Joel";

    }





    //Esta función hace que si hay algún jugador cerca de cualquiera de los slimes, éste último se pondrá rojo
    void slimesRojos(){
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        for(int i = 0; i < distanceMatrix.GetLength(0); i++) //recorre las filas (slimes)
        {
            bool algunoCerca = false; //variable que me dirá si cada uno de los slimes tiene algun jugador cerca
            
            for(int k = 0; k < distanceMatrix.GetLength(0); k++)
            {
                if(distanceMatrix[i, k] <= radiodeDeteccion) //el slime son las filas
                {
                    algunoCerca = true;
                    break;
                }
            }
            if(algunoCerca)
            {
                slimemov[i].colorRojo();
            }
            else
            {
                slimemov[i].colorOriginal();
            }
        }
    }


    //Este método mata al slime, suma la puntuación al player que lo mata y envía al respawn al slime 
    void muerteSlime()
    {
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        //si cliente.numplayergame me da 1, en realidad llevo al player2, que está ubicado en la columna 1 de la distanceMatrix
        int idplayer = cliente.numplayergame;

        for(int i = 0; i < distanceMatrix.GetLength(0); i++) //recorre las filas (slimes)
        {               
            if(Input.GetKeyDown(KeyCode.Space) && distanceMatrix[i, idplayer] < radiodeDeteccion)         
            {
                slimemov[i].tiempoRespawn(tiemporespawnslime);
                playermov[idplayer].puntuation += 1;
                //Debug.Log("Bajas del Player " + (idplayer+1).ToString() + ": " + playermov[idplayer].puntuation);
                //Debug.Log("El player " + (idplayer+1).ToString() + " mata al slime " + (i+1).ToString());      
            }            
        }
    }





    //Este método permite que el slime mate a un player y le envía al respawn
    void muertePlayer()
    {
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        for(int i = 0; i < distanceMatrix.GetLength(0); i++) //recorrerá las filas (slimes)
        {
            for(int k = 0; k < distanceMatrix.GetLength(1); k++)
            {
                if (playermov[k].muerto < 3) // Solo se tienen en cuenta los jugadores que no han perdido
                {
                    if (distanceMatrix[i, k] < 0.4)
                    {
                        playermov[k].tiempoRespawn(tiemporespawnplayer);
                        playermov[k].muerto += 1;
                        //Debug.Log("El player " + (k + 1).ToString() + " ha muerto " + playermov[k].muerto + " veces.");

                        if (playermov[k].muerto == 3)
                        {
                            playermov[k].finPartida();
                            Debug.Log("El player " + (k + 1).ToString() + " ha muerto 3 veces y ha perdido.");
                        }
                    }
                }
            }
        }
    }



    //Calcula la matriz de distancias
    void calculateDistances()
    {
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        //Definiré la matriz que guarda las distancias - rows = numero de slimes; columns = para players
        distanceMatrix = new float[numslimes, numplayers];

        for (int j = 0; j < numslimes; j++) //recorre las filas (slimes)
        {
            for (int k = 0; k < numplayers; k++) //recorre las columnas (players)
            {
                //para j = 0, se trata del slime 0
                //para k = 0, se trata del player 0
                distanceMatrix[j, k] = Vector2.Distance(slimemov[j].transform.position, playermov[k].transform.position);
                //Debug.Log("j = " + j + " k = "  + k );
            }
        }
    }


    //Metodo para imprimir la matrix de distancias
    void printMatrix1()
    {
        string matrixString = "";

        for (int s = 0; s < numslimes; s++)
        {
            for (int p = 0; p < numplayers; p++)
            {
                matrixString += "s" + (s + 1) + " to p" + (p + 1) + " = " + distanceMatrix[s, p].ToString() + ";  ";
            }
            matrixString += "\n";
        }
        Debug.Log(matrixString);
    }



    //Funcion que me dice cual es el jugador más cercano a cada slime
    void calculateNearest()
    {

        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        //Definiré la matriz que guarda en la columna 1 el numero del slime y en la 2 el jugador más cercano
        nearestMatrix = new int[numslimes, 2];


        for (int r = 0; r < distanceMatrix.GetLength(0); r++) //recorre todas las filas
        {
            float minValue = 99;
            int column = 99;

            for (int c = 0; c < distanceMatrix.GetLength(1); c++) //recorre las columnas
            {
                if (distanceMatrix[r, c] < minValue)
                {
                    minValue = distanceMatrix[r, c];
                    column = c;
                }
            }
            nearestMatrix[r, 0] = r;
            nearestMatrix[r, 1] = column + 1; //en la variable column se guarda el numero de la columna con la distancia más baja,
                                              //y esa columna pertenece al player columna+1, porque la primera columna (que es 
                                              // la 0 en C#) guarda al player 1  

        }

    }



    //Imprime la matriz del jugador más cercano a cada slime
    void printMatrix2()
    {
        string matrixString = "";

        for (int s = 0; s < nearestMatrix.GetLength(0); s++)
        {

            matrixString += "s" + (s + 1) + " nearest p = " + nearestMatrix[s, 1].ToString() + ";  ";
            matrixString += "\n";
        }
        Debug.Log(matrixString);

    }



    //Metodo para hacer que el slime1 vaya directamente  a por el jugador 1

    void aporelplayer()
    {
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        float velocidad = 10f;

        for(int i = 0; i < slimemov.GetLength(0); i++)
        {
            int playercerca = nearestMatrix[i, 1]; //me dice el numero del player más cercano al slime i+1
            Vector2 direction = player[playercerca-1].transform.position - slime[i].transform.position;
            direction.Normalize();
            //Debug.Log("El slime " + (i+1).ToString() + " direccion " + direction + " por el player " + playercerca.ToString() + ".");
            
            rb2d = slimemov[i].GetComponent<Rigidbody2D>();

            

            rb2d.MovePosition((Vector2)slime[i].transform.position + (direction * velocidad * Time.deltaTime));
            

        }


    }

    void paredes(float distance){ //La distancia es para el radio de deteccion de pared, consideramos 0.12
        
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];

        //Guardo todos los players en playersmov
        for (int p = 0; p < numplayers; p++)
        {
            playermov[p] = player[p].GetComponent<PlayerMovement>();
        }
        //Guardo todos los slimes en slimesmov
        for (int s = 0; s < numslimes; s++)
        {
            slimemov[s] = slime[s].GetComponent<SlimeMovement>();
        }

        
        //Es una matriz de numslimes*4, donde cada fila es un slime y cada columna es una pared
        //Columna 0 es pared arriba, columna 1 es pared de abajo, columna 2 es pared a la izq y columna 3 es pared a la derecha
        //En la posicion [i,j] hay la distancia del Slime i+1 a la pared en j
        distanceparedMatrix = new float[numslimes, 4];


        for (int j = 0; j < numslimes; j++) //recorre las filas (slimes)
        {
            // Lanzar un raycast hacia arriba desde la posición del jugador
            RaycastHit2D hit_up = Physics2D.Raycast(slimemov[j].transform.position, Vector2.up, raycastDistance);
            if (hit_up.collider != null && hit_up.collider.CompareTag(tagPared))
            {
                distanceparedMatrix[j, 0] = hit_up.distance; // Guardar en la matriz la distancia a la pared
            }  
            
            
            // Lanzar un raycast hacia abajo desde la posición del jugador
            RaycastHit2D hit_down = Physics2D.Raycast(slimemov[j].transform.position, Vector2.down, raycastDistance);
            if (hit_down.collider != null && hit_down.collider.CompareTag(tagPared))
            {
                distanceparedMatrix[j, 1] = hit_down.distance; // Guardar en la matriz la distancia a la pared
            }
            
            // Lanzar un raycast hacia la izquierda desde la posición del jugador
            RaycastHit2D hit_left = Physics2D.Raycast(slimemov[j].transform.position, Vector2.left, raycastDistance);
            if (hit_left.collider != null && hit_left.collider.CompareTag(tagPared))
            {
                distanceparedMatrix[j, 2] = hit_left.distance; // Guardar en la matriz la distancia a la pared
            }

            // Lanzar un raycast hacia la derecha desde la posición del jugador
            RaycastHit2D hit_right = Physics2D.Raycast(slimemov[j].transform.position, Vector2.right, raycastDistance);
            if (hit_right.collider != null && hit_right.collider.CompareTag(tagPared))
            {
                distanceparedMatrix[j, 3] = hit_right.distance; // Guardar en la matriz la distancia a la pared
            }
        }

        //1 si tiene una pared cerca, 0 si no; Posicion 0 es pared arriba, posicion 1 es pared de abajo, posicion 2 es pared a la izq y posicion 3 es pared a la derecha
        paredMatrix = new int[numslimes, 4];

        for(int i = 0; i < distanceMatrix.GetLength(0); i++){
            for(int k = 0; k < distanceMatrix.GetLength(1); k++){
                if(distanceparedMatrix[i, k] < distance){
                    paredMatrix[i, k] = 1;
                }
                else{
                    paredMatrix[i, k] = 0;
                }
            }
        }



    }

        //Metodo para imprimir la matrix de distancias
    void printMatrix3()
    {
        string matrixString = "";

        for (int s = 0; s < paredMatrix.GetLength(0); s++)
        {
            matrixString += "S" + (s + 1) + " -> arriba: " + paredMatrix[s, 0] + "; abajo: " + paredMatrix[s, 1] + "; izquierda: " + paredMatrix[s, 2] + "; derecha: " + paredMatrix[s, 3] + ";";

            //matrixString += "S" + (s + 1) + " -> arriba: " + distanceparedMatrix[s, 0] + "; abajo: " + distanceparedMatrix[s, 1] + "; izquierda: " + distanceparedMatrix[s, 2] + "; derecha: " + distanceparedMatrix[s, 3] + ";";


            matrixString += "\n";
        }
        Debug.Log(matrixString);
    }




    
}