using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;



public class PlayerMovement : MonoBehaviour
{
    public Client cliente;
    public int idjugador = 1;
    public float speed = 2f;
    private Rigidbody2D rb2d;
    private Vector2 movement; // Vector que guardará la dirección de movimiento
    private Bounds levelBounds;

    private Management radio; //Con esto importaré el valor del radio de deteccion definido en Management
    private Management numplayers; //Con esto importaré el numero de jugadores en la partida

    public MainGameManager mainGameManager;
    public int puntuation = 0; //indica a cuantos slimes ha matado
    public int muerto = 0; //indica cuantas veces ha muerto (se lo ha comido algún slime)

    public float startx = -1.051f;
    public float starty = 0.575f;
    int frame = 0;
    void Start()
    {
        radio = GameObject.FindObjectOfType<Management>();
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector2(startx, starty);
    }

    void Update()
    {

        if (cliente.numplayergame == idjugador)
        {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            //killSlime();
        }
        else
        {
        }
        toroidalMap();
    }


    void toroidalMap()
    {
        // Obtener los límites de la pantalla
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float screenRight = 3.92f;
        float screenLeft = -4.13f;
        float screenTop = screenBounds.y - 0.15f;
        float screenBottom = -screenBounds.y + 0.15f;
        //Debug.Log("ToroidalMap va bien");
        //Debug.Log("Posicion del jugador: " + transform.position.x + " " + transform.position.y);

        // Verificar si el jugador está fuera de los límites de la pantalla
        if (transform.position.x > screenRight) //si se sale por la derecha
        {
            transform.position = new Vector2(screenLeft, transform.position.y);
        }
        else if (transform.position.x < screenLeft) //si se sale por la izquierda
        {
            transform.position = new Vector2(screenRight - 0.15f, transform.position.y);
        }

        if (transform.position.y > screenTop)
        {
            transform.position = new Vector2(transform.position.x, screenBottom + 0.1f);
        }
        else if (transform.position.y < screenBottom)
        {
            transform.position = new Vector2(transform.position.x, screenTop - 0.1f);
        }
    }



    //Si entro en el radio y aprieto el espacio devuelve true
    internal bool killSlime()
    {

        float distanciaAlSlime;
        if (GameObject.FindGameObjectWithTag("Food") != null)
        {
            distanciaAlSlime = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Food").transform.position);
        }
        else
        {
            distanciaAlSlime = 999;
        }


        float radiodeDeteccion = radio.radiodeDeteccion;

        if (Input.GetKeyDown(KeyCode.Space) && distanciaAlSlime <= radiodeDeteccion)
        {
            return true;
        }
        return false;

    }








    void FixedUpdate()
    {
        if (cliente.numplayergame == idjugador)
        {
            // Mover el jugador utilizando el componente rigidbody
            rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
            if (frame % 10 == 0)
            {
                string message = "8/1/0/" + cliente.usuario + "*" + transform.position.x.ToString() + "*" + transform.position.y.ToString()+"\0";
                UnityEngine.Debug.Log(message);
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
                cliente.server.Send(msg);

            }
            frame++;

        }
        else
        {
            rb2d.MovePosition(new Vector2(mainGameManager.ppositions[idjugador, 0], mainGameManager.ppositions[idjugador, 1]));
        }
    }




    internal bool spacebarPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    //Método para fijar cuanto tiempo tarda el slime en respawnear
    internal void tiempoRespawn(float time)
    {
        transform.position = new Vector2(100, 100); //lo llevamos lejos para que no siga contando como muerte
        gameObject.SetActive(false); // desactiva el objeto al chocar
        Invoke("ToRespawn", time); // espera time segundos y lo envía al respawn
    }


    //Método para reaparecer en el respawn
    void ToRespawn()
    {
        float x1 = -1.216f;
        float x2 = 1.014f;
        float y1 = 0.266f;
        float y2 = 0.791f;

        //Hago esto para que aparezca en el respawn pero en un sitio random de él
        transform.position = new Vector2(Random.Range(x1, x2), Random.Range(y1, y2));
        gameObject.SetActive(true); // activa el objeto después de 3 segundos
    }


    //Para cuando haya muerto 3 veces
    internal void finPartida()
    {
        transform.position = new Vector2(100, 100); //lo llevamos lejos para que no siga contando como muerte
        gameObject.SetActive(false); // desactiva el objeto
    }









}