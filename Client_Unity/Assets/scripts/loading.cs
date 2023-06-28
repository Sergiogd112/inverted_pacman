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
            for (int i = 0; i < 4; i++)
            {
                if (client.invitados[0] == client.usuario)
                {
                    client.numplayergame = i;
                }
            }
            manager.names[1] = client.invitados[0];
            manager.names[2] = client.invitados[1];
            manager.names[3] = client.invitados[2];
            string message="7/1/"+client.amfitrion+","+client.invitados[0]+"*"+client.invitados[1]+"*"+client.invitados[2]+'\0';
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            client.server.Send(msg);
            ThreadStart ts = delegate { manager.AtenderPartida(); };
            manager.atender = new Thread(ts);
            manager.atender.Start();
            SceneManager.LoadScene("Game");
        }
        else if (client.invitationres == -1)
        {
            client.invitationres = 0;
            string message="7/0/"+client.amfitrion+","+client.invitados[0]+"*"+client.invitados[1]+"*"+client.invitados[2];
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            client.server.Send(msg);
            SceneManager.LoadScene("Menu");
        }
    }
    public void StartGame()
    {

    }
}
