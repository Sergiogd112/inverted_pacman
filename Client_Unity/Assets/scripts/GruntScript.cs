using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for the grunt.
/// </summary>
public class GruntScript : MonoBehaviour
{
    public Transform John; // This is the John object.(John is the player)
    public GameObject BulletPrefab; // This is the bullet prefab.
    

    private int Health = 3; // This is the grunt's health.
    private float LastShoot; // This is the time the grunt last shot.

    /// <summary>
    /// This is called once per frame. It is used to update the grunt's position.
    /// </summary>
    void Update()
    {
        if (John == null) return; 

        Vector3 direction = John.position - transform.position; // Get the direction to John.
        if (direction.x >= 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); // Flip the grunt if necessary.
        else transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f); // Flip the grunt if necessary.

        float distance = Mathf.Abs(John.position.x - transform.position.x); // Get the distance to John.
        // If the grunt is close enough to John and enough time has passed since the last shot, shoot.
        if (distance < 1.0f && Time.time > LastShoot + 0.25f) 
        {
            Shoot();
            LastShoot = Time.time;
        }
    }

    /// <summary>
    /// This is called when the grunt shoots.
    /// </summary>
    private void Shoot()
    {
        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f); // Get the direction to shoot.
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity); // Instantiate the bullet.
        bullet.GetComponent<BulletScript>().SetDirection(direction); // Set the direction of the bullet.
    }

    /// <summary>
    /// This is called when the grunt is hit by a bullet.
    /// </summary>
    public void Hit()
    {
        Health -= 1; // Decrease the grunt's health.
        if (Health == 0) Destroy(gameObject);
    }
}
