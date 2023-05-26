using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class manageinvitacion : MonoBehaviour
{
    public Client client;
    public void YES(){
        string message = "6/1/"+client.idpartida.ToString();
        byte[] msg=System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
        SceneManager.LoadScene("Loading");
    }
    public void NO(){
        string message = "6/0/"+client.idpartida.ToString();
        byte[] msg=System.Text.Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);
        SceneManager.LoadScene("Menu");

    }
}
