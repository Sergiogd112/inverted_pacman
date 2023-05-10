using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Management : MonoBehaviour
{
    public float radiodeDeteccion = 0.5f;
    public float tiemporespawnslime = 3f;



    //public Slime_Movement slime;
    //public PlayerMovement player;

    public GameObject player;
    public GameObject slime;


    void Start()
    {
        player = GameObject.Find("Player");
        slime = GameObject.Find("Slime");
    }

    // Update is called once per frame
    void Update()
    {
        SlimeMovement slimemov = slime.GetComponent<SlimeMovement>(); 
        PlayerMovement playermov = player.GetComponent<PlayerMovement>();   
        if(playermov.killSlime())
        {
            playermov.puntuation += 1;
            slimemov.tiempoRespawn(tiemporespawnslime);
            Debug.Log("Bajas del Player: " + playermov.puntuation);
        }
        if(slimemov.colision_player){
            slimemov.bajas_slime += 1;
            playermov.tiempoRespawn(3f);
            Debug.Log("Bajas del Slime: " + slimemov.bajas_slime);
            slimemov.colision_player = false;
        }   
    }

   
 




}
