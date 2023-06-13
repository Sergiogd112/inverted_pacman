using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Management : MonoBehaviour
{
    public float radiodeDeteccion = 0.5f;
    public float tiemporespawnslime = 3f;

    public int numslimes = 4; //variable para definir los slime que habrán el la partida
    public int numplayers = 4; //variable para definir los jugadores online que habrán el la partida


    private float[,] distanceMatrix; // Declaración de la matriz
    private int[,] nearestMatrix; // Declaración de la matriz de jugador más cercano a cada slime

    //public Slime_Movement slime;
    //public PlayerMovement player;
    public GameObject playerprefab;


    GameObject[] player = new GameObject[4];
    GameObject[] slime = new GameObject[4];
    public MainGameManager mainGameManager;

    //[SerializeField] private Transform objetivo;
    private NavMeshAgent navMeshAgent;



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

        //slime = GameObject.Find("Slime1"); //antigua forma para cuando solo había 1 slime
        //InvokeRepeating("printMatrix2", 0.01f, 4f);
    }

    // Update is called once per frame
    void Update()
    {

        //SlimeMovement slimemov = slime.GetComponent<SlimeMovement>();
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];
        calculateDistances();
        //InvokeRepeating("printMatrix1", 2f, 2f);
        //PrintMatrix();
        calculateNearest();
        aporelplayer();

        /*
        for(int i = 0; i < numplayers; i++)
        {
            playermov[i] = player[i].GetComponent<PlayerMovement>();

            //Debug.Log("Posicion del Player " + (i+1).ToString() + ": " + playermov[i].transform.position);
            //Debug.Log("Posicion del Slime: " + slimemov.transform.position);
            
            if(playermov[i].killSlime())
            {
                playermov[i].puntuation += 1;
                slimemov.tiempoRespawn(tiemporespawnslime);
                Debug.Log("Bajas del Player " + (i+1).ToString() + ": " + playermov[i].puntuation);
            }
            if(slimemov.colision_player){
                slimemov.bajas_slime += 1;
                playermov[i].tiempoRespawn(3f);
                Debug.Log("El Slime lleva " + slimemov.bajas_slime + " bajas y ha matado al Player " + (i+1).ToString());
                slimemov.colision_player = false;
            }   */


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

        // Accedo al componente NavMeshAgent del slime4
        NavMeshAgent slime4NavAgent = slime[0].GetComponent<NavMeshAgent>();
        // Establezco el destino del slime4 como la posición del jugador1
        slime4NavAgent.SetDestination(playermov[0].transform.position);

        //navMeshAgent = slime[0].GetComponent<NavMeshAgent>();
        //navMeshAgent.updateRotation = false;
        //navMeshAgent.updateUpAxis = false;










    }






}





