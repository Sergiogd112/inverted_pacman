using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Management : MonoBehaviour
{
    public float radiodeDeteccion = 0.5f;
    public float tiemporespawnslime = 3f;

    public int numslimes = 4; //variable para definir los slime que habrán el la partida
    public int numplayers = 4; //variable para definir los jugadores online que habrán el la partida


    private float[,] distanceMatrix; // Declaración de la matriz

    //public Slime_Movement slime;
    //public PlayerMovement player;



    GameObject[] player = new GameObject[4];
    GameObject[] slime = new GameObject[4];



    void Start()
    {
        for(int i = 1; i <= numplayers; i++){
            player[i-1] = GameObject.Find("Player" + i.ToString());
        }

        for(int i = 1; i <= numslimes; i++){
            slime[i-1] = GameObject.Find("Slime" + i.ToString());
        }
        //slime = GameObject.Find("Slime1"); //antigua forma para cuando solo había 1 slime
    }

    // Update is called once per frame
    void Update()
    {
        
        //SlimeMovement slimemov = slime.GetComponent<SlimeMovement>();
        SlimeMovement[] slimemov = new SlimeMovement[numslimes];
        PlayerMovement[] playermov = new PlayerMovement[numplayers];
        killSlimeM();
        //Invoke("PrintMatrix()", 3f);
        PrintMatrix();
        
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

        int killSlimeM(){
            SlimeMovement[] slimemov = new SlimeMovement[numslimes];
            PlayerMovement[] playermov = new PlayerMovement[numplayers];

            //Definiré la matriz que guarda las distancias - rows = numero de slimes; columns = para players
            distanceMatrix = new float[numslimes, numplayers];

            //for(int i = 0; i < numplayers; i++){
                //distanceMatrix[i, 1] = 0;
            //}
            distanceMatrix[0,0] = 1;
            distanceMatrix[0,1] = 11;
            distanceMatrix[0,2] = 1111;
            distanceMatrix[0,3] = 11111;

            Debug.Log("");

            return 0;
        }


        private void PrintMatrix()
        {
            string matrixString = "";

            for (int i = 0; i < numslimes; i++)
            {
                for (int j = 0; j < numplayers; j++)
                {
                    matrixString += distanceMatrix[i, j].ToString() + " ";
                }
                matrixString += "\n";
            }

            Debug.Log(matrixString);
        }
            

    
    
    
    
    
    
    
    }

   
 


