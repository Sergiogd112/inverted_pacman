using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class scrip : MonoBehaviour
{
    // Start is called before the first frame update
    public Client client;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (client.invitado)
        {
            client.pscene = "Menu";
            SceneManager.LoadScene("Invitacion");
            client.invitado = false;
        }
    }
}
