using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;



public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb2d;
    private Vector2 movement; // Vector que guardará la dirección de movimiento
    private Bounds levelBounds;

    private Management radio; //Con esto importaré el valor del radio de deteccion definido en Management

    public int puntuation = 0;

    void Start()
    {
        radio = GameObject.FindObjectOfType<Management>();
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(-1.051f, 0.575f);
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        killSlime();
    }


    void ToroidalMap()
    {
        // Obtener los límites de la pantalla
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float screenRight = 3.91f;
        float screenLeft = -4.15f;
        float screenTop = screenBounds.y - 0.15f;
        float screenBottom = -screenBounds.y + 0.15f;

        // Verificar si el jugador está fuera de los límites de la pantalla
        if (transform.position.x > screenRight) //si se sale por la derecha
        {
            transform.position = new Vector2(screenLeft, transform.position.y);
        }
        else if (transform.position.x < screenLeft) //si se sale por la izquierda
        {
            transform.position = new Vector2(screenRight, transform.position.y);
        }

        if (transform.position.y > screenTop)
        {
            transform.position = new Vector2(transform.position.x, screenBottom);
        }
        else if (transform.position.y < screenBottom)
        {
            transform.position = new Vector2(transform.position.x, screenTop);
        }
    }



    //Si entro en el radio y aprieto el espacio devuelve true
    internal bool killSlime(){
        
        float distanciaAlSlime;
        if(GameObject.FindGameObjectWithTag("Food") != null){
            distanciaAlSlime = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Food").transform.position);
        }
        else{
            distanciaAlSlime = 999;
        }

        
        float radiodeDeteccion = radio.radiodeDeteccion;
         
        if (Input.GetKeyDown(KeyCode.Space) && distanciaAlSlime <= radiodeDeteccion) {
            return true;                  
        }
        return false;

    }







    void FixedUpdate()
    {
        // Mover el jugador utilizando el componente rigidbody
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
        ToroidalMap();
        //Imprime por pantalla la posición del jugador por la consola
        //Debug.Log("Posición del jugador: " + transform.position); 
    }




    //Método para fijar cuanto tiempo tarda el slime en respawnear
    internal void tiempoRespawn(float time){
        gameObject.SetActive(false); // desactiva el objeto al chocar
        Invoke("ToRespawn", time); // espera time segundos y lo envía al respawn
    }


    //Método para reaparecer en el respawn
    void ToRespawn()
    {
        transform.position = new Vector2(-1.051f, 0.575f);
        gameObject.SetActive(true); // activa el objeto después de 3 segundos
    }









}