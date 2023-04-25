using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // velocidad del jugador
    private Rigidbody2D rb2d; // componente rigidbody del jugador
    private Vector2 movement; // dirección del movimiento del jugador

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // obtener el componente rigidbody del jugador
    }

    void Update()
    {
        // Obtener los valores de entrada para el movimiento
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        // Mover el jugador utilizando el componente rigidbody
        rb2d.MovePosition(rb2d.position + movement * speed * Time.fixedDeltaTime);
    }
}
