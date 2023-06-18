using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb2d;
    private Vector2 direction; // Vector que guarda la dirección de movimiento
    private Bounds levelBounds;

    private Management radio; //Con esto importaré el valor del radio de deteccion definido en Management


    public Color nuevoColor = new Color(1.0f, 0.0f, 0.0f, 1.0f); // Color del slime cuando el jugador esté cerca
    public Color originalColor; // color original del objeto

    public bool colision_player = false;
    public bool colision_pared = false;

    public int bajas_slime = 0;

    public float startx = -2.051f;
    public float starty = 0.575f;

    // Start is called before the first frame update
    void Start()
    {
        radio = GameObject.FindObjectOfType<Management>();
        transform.position = new Vector2(startx, starty);

        rb2d = GetComponent<Rigidbody2D>();

        // obtiene el SpriteRenderer del objeto y guarda su color original
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;    
    }

    // Update is called once per frame
    void Update()
    {
        if (InFrontOfPlayer()) {
            Debug.Log("Edtoy delante del jugador");
            //SetRandomDirection();
        }
        if(colision_pared){
            //Debug.Log("Pum Pared ");
            //SetRandomDirection();
            colision_pared = false;
        } 
    }


void SetRandomDirection()
{
    // Obtiene una dirección aleatoria diferente a la dirección actual
    Vector2 previousDirection = direction;
    int randomDirection;
    do
    {
        randomDirection = Random.Range(0, 4);
        switch (randomDirection)
        {
            case 0:
                direction = Vector2.up;
                break;
            case 1:
                direction = Vector2.down;
                break;
            case 2:
                direction = Vector2.right;
                break;
            case 3:
                direction = Vector2.left;
                break;
            default:
                break;
        }
    } while (direction == previousDirection); // Si la nueva direccion es la que llevaba antes, elige otra
}


    //Devuelve true si está delante de un jugador
    bool InFrontOfPlayer() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, 1f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }


    //Método para hacer que el mapasea toroidal
    void ToroidalMap(){
        // Obtener los límites de la pantalla
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float screenRight = 3.91f;
        float screenLeft = -4.15f;
        float screenTop = screenBounds.y - 0.15f;
        float screenBottom = -screenBounds.y + 0.15f;

        // Verificar si el slime está fuera de los límites de la pantalla
        if (transform.position.x > screenRight) // si se sale por la derecha
        {
            transform.position = new Vector2(screenLeft, transform.position.y);
        }
        else if (transform.position.x < screenLeft) // si se sale por la izquierda
        {
            transform.position = new Vector2(screenRight, transform.position.y);
        }

        if (transform.position.y > screenTop) // si se sale por arriba
        {
            transform.position = new Vector2(transform.position.x, screenBottom);
        }
        else if (transform.position.y < screenBottom) // si se sale por abajo
        {
            transform.position = new Vector2(transform.position.x, screenTop);
        }
    }


    void FixedUpdate()
    {
        //Debug.DrawLine(rb2d.position, rb2d.position + direction * 2f, Color.red);

        // Mover el slime en la dirección actual
        //rb2d.MovePosition(rb2d.position + direction * speed * Time.fixedDeltaTime);

        ToroidalMap();
    }





    //Detecta cuando se choca el objeto con el jugador o con la pared
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            colision_player = true;
        }
        else{
            colision_player = false;
        }
        if (collision.gameObject.CompareTag("Pared"))
        {
            colision_pared = true;
        }
        else{
            colision_pared = false;
        }
    }



    //Método para fijar cuanto tiempo tarda el slime en respawnear
    internal void tiempoRespawn(float time){
        transform.position = new Vector2(10, 10); //lo llevamos lejos para que no siga contando como muerte
        gameObject.SetActive(false); // desactiva el objeto al chocar
        Invoke("ToRespawn", time); // espera time segundos y lo envía al respawn
    }


    //Método para reaparecer en el respawn
    void ToRespawn()
    {
        //Creamos una matriz para guardar los 4 posibles respawns
        Vector2[] respawnPositions = new Vector2[] {
            new Vector2(-3.35f, 2.55f),
            new Vector2(3.11f, 2.57f),
            new Vector2(-2.51f, -2.98f),
            new Vector2(2.36f, -2.97f)
        };

        //Elegimos un respawn random
        int randomIndex = Random.Range(0, respawnPositions.Length);

        //Cambiamos la posición a la random
        transform.position = respawnPositions[randomIndex];

        gameObject.SetActive(true); // activa el objeto después de 3 segundos
    }


    //Método para hacerse rojo si un player está cerca
    internal void colorRojo()
    {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f); //pintarlo rojo
    }
    internal void colorOriginal()
    {
            GetComponent<SpriteRenderer>().color = originalColor;
    }

    


}