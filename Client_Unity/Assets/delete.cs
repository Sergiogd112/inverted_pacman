using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class delete : MonoBehaviour
{
    public Client client;
    public void deletePlayer()
    {
        string message = "11/";
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
    }
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
