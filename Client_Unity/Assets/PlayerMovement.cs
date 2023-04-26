using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody2D rb2d;
    private Vector2 movement; // Vector que guardará la dirección de movimiento
    private Bounds levelBounds;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // Mover el jugador utilizando el componente rigidbody
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);

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

        //Imprime por pantalla la posición del jugador por la consola
        Debug.Log("Posición del jugador: " + transform.position); 

    }
}
