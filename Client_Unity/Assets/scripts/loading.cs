using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

public class loading : MonoBehaviour
{
    // Start is called before the first frame update
    public Client client;
    public MainGameManager manager;

    void Start()
    {
        if (client.amfitrion == client.usuario)
        {
            manager.host = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (client.invitationres == 1)
        {
            client.atender.Abort();
            client.invitationres = 0;
            manager.names[0] = client.amfitrion;
            manager.names[1] = client.invitados[0];
            manager.names[2] = client.invitados[1];
            manager.names[3] = client.invitados[2];
            
            ThreadStart ts = delegate { manager.AtenderPartida(); };
            manager.atender = new Thread(ts);
            manager.atender.Start();
            SceneManager.LoadScene("Game");
        }
        else if (client.invitationres == -1)
        {
            client.invitationres = 0;
            SceneManager.LoadScene("Menu");
        }
    }
    public void StartGame()
    {

    }
}
