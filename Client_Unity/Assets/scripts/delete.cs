using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This is the script for the delete account button.
/// </summary>
public class delete : MonoBehaviour
{
    public Client client;
    /// <summary>
    /// This is called when the user clicks the delete account button.
    /// It sends a message to the server to delete the account.
    /// </summary>
    public void deletePlayer()
    {
        string message = "11/";
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
    }
    /// <summary>
    /// This is called once per frame. It checks if the account was deleted.
    /// </summary>
    public void Update()
    {
        if (client.delete == 1)
        {
            SceneManager.LoadScene("Login");
        }
        else if (client.delete == -1)
        {
            UnityEngine.Debug.Log("Error al eliminar");
        }
    }
}
