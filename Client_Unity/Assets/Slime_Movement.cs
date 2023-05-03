using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Movement : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb2d;
    private Vector2 direction; // Vector que guarda la dirección de movimiento
    private Bounds levelBounds;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        direction = Vector2.left; // establecer una dirección inicial
        
    }

    // Update is called once per frame
    void Update()
    {              
    }
    void FixedUpdate()
    {
        Debug.DrawLine(rb2d.position, rb2d.position + direction * 2f, Color.red);

        // mover el slime en la dirección actual
        rb2d.MovePosition(rb2d.position + direction * speed * Time.fixedDeltaTime);

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

        // Comprobar si el slime choca con una pared
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1.5f);

        if (hit.collider != null && hit.collider.CompareTag("Pared")) // si choca con una pared
        {
            Debug.Log("Porrazo del slime"); 
            // elegir aleatoriamente una dirección a la que moverse
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            direction = new Vector2(x, y).normalized;
        }
        Debug.Log("Direction: " + direction);
    }



    //detecta cuando se choca el objeto
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false); // desactiva el objeto al chocar
            Invoke("ToRespawn", 3f); // espera 3 segundos y lo envía al respawn
        }
    }

    void ToRespawn()
    {
        //Creamos una matriz para guardar los 4 posibles respawns
        Vector2[] respawnPositions = new Vector2[] {
            new Vector2(-3.21f, 2.59f),
            new Vector2(3.21f, 2.59f),
            new Vector2(-2.25f, -2.94f),
            new Vector2(2.25f, -2.94f)
        };

        //Elegimos un respawn random
        int randomIndex = Random.Range(0, respawnPositions.Length);
        //Cambiamos la posición a la random
        transform.position = respawnPositions[randomIndex];

        
        gameObject.SetActive(true); // activa el objeto después de 3 segundos
    }

}