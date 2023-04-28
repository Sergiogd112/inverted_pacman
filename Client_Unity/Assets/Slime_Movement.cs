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
        direction = Vector2.right; // establecer una dirección inicial
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("HEY HEY HEY");       
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
}