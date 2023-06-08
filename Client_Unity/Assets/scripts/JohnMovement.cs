using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the script for the John character.
/// </summary>
public class JohnMovement : MonoBehaviour
{
    public float Speed; 
    public float JumpForce; // This is the force of the jump.
    public GameObject BulletPrefab; // This is the bullet prefab.

    private Rigidbody2D Rigidbody2D; // This is the rigidbody of the John object.
    private Animator Animator; // This is the animator of the John object.
    private float Horizontal; // This is the horizontal input.
    private bool Grounded; // This is true if the John object is on the ground.
    private float LastShoot; // This is the time the last bullet was fired.
    private int Health = 5; // This is the health of the John object.
    public GameManagerWO gameManager; // This is the game manager.
    private bool isDead; // This is true if the John object is dead.

    /// <summary>
    /// This is called when the John object is created.
    /// </summary>
    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>(); // Get the rigidbody.
        Animator = GetComponent<Animator>(); // Get the animator.
    }

    /// <summary>
    /// This is called every frame.
    /// </summary>
    private void Update()
    {
        // Movimiento
        Horizontal = Input.GetAxisRaw("Horizontal");

        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Animator.SetBool("running", Horizontal != 0.0f);

        // Detectar Suelo
        // Debug.DrawRay(transform.position, Vector3.down * 0.1f, Color.red);
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
        {
            Grounded = true;
        }
        else Grounded = false;

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Disparar
        if (Input.GetKey(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    /// <summary>
    /// This is called every fixed frame.
    /// </summary>
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y); // Move the John object.
    }

    /// <summary>
    /// This is called when the jump button/key is pressed.
    /// </summary>
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    /// <summary>
    /// This is called when the shoot button/key is pressed.
    /// It creates a bullet.
    /// </summary>
    private void Shoot()
    {
        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector3.right;
        else direction = Vector3.left;

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    /// <summary>
    /// This is called when the John object is hit by a bullet.
    /// </summary>
    public void Hit()
    {
        Health -= 1;
        if (Health == 0) 
        {
            Destroy(gameObject);
        }
    }
}