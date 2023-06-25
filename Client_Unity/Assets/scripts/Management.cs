using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;



public class Management : MonoBehaviour
{
    public Client cliente;
    public float radiodeDeteccion = 0.5f; //radio desde el cual el jugador ya puede matar
    public float tiemporespawnslime = 3f;
    public float tiemporespawnplayer = 4f;

    public int numslimes = 4; //variable para definir los slime que habrán el la partida
    public int numplayers = 4; //variable para definir los jugadores online que habrán el la partida


    private float[,] distanceMatrix; // Declaración de la matriz
    private int[,] nearestMatrix; // Declaración de la matriz de jugador más cercano a cada slime

    //public Slime_Movement slime;
    //public PlayerMovement player;
    public GameObject playerprefab;

    private Rigidbody2D rb2d;


    GameObject[] player = new GameObject[4];
    GameObject[] slime = new GameObject[4];
    public MainGameManager mainGameManager;

    //[SerializeField] private Transform objetivo;
    private NavMeshAgent navMeshAgent;


    //Estos dos atributos serán para detectar si hay paredes alrededor
    public float raycastDistance = 5.0f;
    public LayerMask wallLayer;




    void Start()
    {
        TextMeshProUGUI texto = GameObject.Find("Text1").GetComponent<TextMeshProUGUI>();

        for (int i = 1; i <= numplayers; i++)
        {
            player[i - 1] = GameObject.Find("Player" + i.ToString());
        }

        for (int i = 1; i <= numslimes; i++)
        {
            slime[i - 1] = GameObject.Find("Slime" + i.ToString());
        }
        //InvokeRepeating("printMatrix1", 3f, 3f);

        //slime = GameObject.Find("Slime1"); //antigua forma para cuando solo había 1 slime
        //InvokeRepeating("printMatrix2", 0.01f, 4f);
         texto.text = "HOla?";
    }

    // Update is called once per frame
    void Update()
    {

        //SlimeMovement slimemov = slime.GetComponent<SlimeMovement>();
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];
        calculateDistances();
        
        calculateNearest();
        aporelplayer();
        slimesRojos();
        muerteSlime();
        muertePlayer();
        paredDebajo(999);
        

        /*
        for(int i = 0; i < numplayers; i++)
        {
            playermov[i] = player[i].GetComponent<PlayerMovement>();

            //Debug.Log("Posicion del Player " + (i+1).ToString() + ": " + playermov[i].transform.position);
            //Debug.Log("Posicion del Slime: " + slimemov.transform.position);
            

            if(slimemov.colision_player){
                slimemov.bajas_slime += 1;
                playermov[i].tiempoRespawn(3f);
                Debug.Log("El Slime lleva " + slimemov.bajas_slime + " bajas y ha matado al Player " + (i+1).ToString());
                slimemov.colision_player = false;
            }   */
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
            for(int k = 0; k < distanceMatrix.GetLength(1); k++){
                if(distanceMatrix[i, k] < 0.288) //Consideramos esta distancia como que ya se chocan
                {
                    playermov[k].tiempoRespawn(tiemporespawnplayer);
                    playermov[k].muerto += 1;
                    //Debug.Log("El slime " + (i+1).ToString() + " debería matar al jugador " + (k+1).ToString() + " .");
                    //Debug.Log("El player " + (k+1).ToString() + " ha muerto " + playermov[k].muerto + " veces.");

                }
            }
        }
    }


    //Calcula la matriz de distancias
    int calculateDistances()
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
        return 0;
    }


    //Metodo para imprimir la matrix de distancias
    private void printMatrix1()
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
    private void calculateNearest()
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
    private void printMatrix2()
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

    private void aporelplayer()
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

        Vector2 direction = player[0].transform.position - slime[0].transform.position;
        direction.Normalize();


        rb2d = slime[0].GetComponent<Rigidbody2D>();

        
        float velocidad = 12f;

        
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){ //derecha o izq
            if (direction.x > 0) //mueve hacia la derecha
            { 
                rb2d.MovePosition(rb2d.position + new Vector2(velocidad * Time.deltaTime, 0f));
                //Debug.Log("HAcia derecha " + direction);
            }
            if (direction.x < 0) //mueve hacia la izq
            { 
                rb2d.MovePosition(rb2d.position + new Vector2(-velocidad * Time.deltaTime, 0f));
                //Debug.Log("HAcia izq " + direction);
            }
        }
        else{

            if (direction.y > 0) //mueve hacia arriba
            {
                //Debug.Log("Arriba " + direction);
                rb2d.MovePosition(rb2d.position + new Vector2(0f, velocidad * Time.deltaTime));
            }
            if (direction.y < 0) //mueve hacia abajp
            { 
                //Debug.Log("Abajo " + direction);
                rb2d.MovePosition(rb2d.position + new Vector2(0f, -velocidad * Time.deltaTime));
            }
        }


        //rb2d.MovePosition(rb2d.position + direction * velocidad * Time.deltaTime);
    }

    bool paredDebajo(int jugador){
        
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

        Vector2 slimePosition = slimemov[0].transform.position;

        //Tengo que inicializar la variable wallLayer y asignarla al objeto de unity conveniente
    



        // Lanza un rayo hacia abajo desde la posición del jugador
        RaycastHit2D hit = Physics2D.Raycast(slimePosition, Vector2.down, raycastDistance, wallLayer);

        // Dibuja el rayo en la escena para depuración
        Debug.DrawRay(slimePosition, Vector2.down * raycastDistance, Color.red);


        //Imprime por consola si hay pared debajo o no
        Debug.Log("¿Hay pared debajo del jugador? " + hit.collider != null);




      








        // Realiza la detección del rayo
        if (hit.collider != null)
        {
            // Si el rayo colisiona con un objeto en la capa de paredes, hay una pared debajo del jugador
            Debug.Log("¡Hay una pared debajo del jugador!");
            return true;
        }

        else{
            Debug.Log("¡no Hay una pared debajo del jugador!");
        }
        


        return false;
    }






}