using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the script for the manageinvitacion scene.
/// </summary>
public class manageinvitacion : MonoBehaviour
{
    public Client client; // This is the client object.
    /// <summary>
    /// When the user clicks the yes button,
    /// It sends a message to the server to 
    /// accept the invitation.
    /// </summary>
    public void YES(){
        string message = "6/1/"+client.idpartida.ToString();
        byte[] msg=System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
        SceneManager.LoadScene("Loading");
    }
    /// <summary>
    /// When the user clicks the no button,
    /// It sends a message to the server to
    /// reject the invitation.
    /// </summary>
    public void NO(){
        string message = "6/0/"+client.idpartida.ToString();
        byte[] msg=System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
        SceneManager.LoadScene("Menu");

    }
}
