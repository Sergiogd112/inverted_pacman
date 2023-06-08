using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for the camera.
/// </summary>
public class CameraScript : MonoBehaviour
{
    public Transform John; // This is the John object.(John is the player)

    /// <summary>
    /// This is called once per frame. It is used to update the camera's position.
    /// </summary>
    void Update()
    {
        if (John != null)
        {
            Vector3 position = transform.position;
            position.x = John.position.x;
            transform.position = position;
        }
    }
}
