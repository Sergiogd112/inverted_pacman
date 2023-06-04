using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Management : MonoBehaviour
{
    public float radiodeDeteccion = 0.5f;
    public float tiemporespawnslime = 3f;

    public int numplayers = 4; //variable para definir los jugadores online que habrán el la partida



    //public Slime_Movement slime;
    //public PlayerMovement player;

    public GameObject slime;

    GameObject[] player = new GameObject[4];


    void Start()
    {

        for(int i = 1; i <= numplayers; i++){
            player[i-1] = GameObject.Find("Player" + i.ToString());
        }
        slime = GameObject.Find("Slime");
    }

    // Update is called once per frame
    void Update()
    {
        SlimeMovement slimemov = slime.GetComponent<SlimeMovement>();
        PlayerMovement[] playermov = new PlayerMovement[numplayers];
        
        for(int i = 0; i < numplayers; i++)
        {
            playermov[i] = player[i].GetComponent<PlayerMovement>();
            
            if(playermov[i].killSlime())
            {
                playermov[i].puntuation += 1;
                slimemov.tiempoRespawn(tiemporespawnslime);
                Debug.Log("Bajas del Player " + (i+1).ToString() + ": " + playermov[i].puntuation);
            }
            if(slimemov.colision_player){
                slimemov.bajas_slime += 1;
                playermov[i].tiempoRespawn(3f);
                Debug.Log("Bajas del Slime: " + slimemov.bajas_slime);
                slimemov.colision_player = false;
            }   
        }
         

    
    
    
    
    
    
    
    }

   
 




}
