using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for the bullet.
/// </summary>
public class BulletScript : MonoBehaviour
{

    public float Speed;
    public AudioClip Sound; // This is the sound that will play when the bullet is fired.

    private Rigidbody2D Rigidbody2D; // This is the rigidbody of the bullet.
    private Vector3 Direction; // This is the direction the bullet will travel.

    /// <summary>
    /// This is called when the bullet is created.
    /// </summary>
    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>(); // Get the rigidbody.
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound); // Play the sound.
    }

    /// <summary>
    /// Move the bullet.
    /// </summary>
    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;
    }

    /// <summary>
    /// Set the direction the bullet will travel.
    /// </summary>
    /// <param name="direction"></param>
    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    /// <summary>
    /// Destroy the bullet.
    /// </summary>
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// This is called when the bullet hits something.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        GruntScript grunt = other.GetComponent<GruntScript>();
        JohnMovement john = other.GetComponent<JohnMovement>();
        if (grunt != null)
        {
            grunt.Hit();
        }
        if (john != null)
        {
            john.Hit();
        }
        DestroyBullet();
    }
}
