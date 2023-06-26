using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  This is the script for the main menu.
/// </summary>
public class MainMenu : MonoBehaviour 
{
    public Client client; // This is the client.
    /// <summary>
    /// This is called when the user clicks the Play button.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// This is called when the user clicks the Quit button.
    /// </summary>
    public void QuitGame()
    {
        string message = "0/";
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
        client.atender.Abort();
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
