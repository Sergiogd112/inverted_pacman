using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour
{
    public Client client;
    // Update is called once per frame
    void Update()
    {
        if (client.invitado)
        {
            client.pscene="Menu";
            SceneManager.LoadScene("Invitacion");
            client.invitado=false;
        }
    }
}
