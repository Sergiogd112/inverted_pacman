using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class invitation : MonoBehaviour
{
    public Client client;
    // Start is called before the first frame update
    void Start()
    {
        string mates = "";
        for (int i = 0; i < 3; i++)
        {
            if (client.invitados[i] == client.usuario) continue;
            mates += Regex.Unescape(client.invitados[i]) + "\n";
        }
        GetComponent<TextMeshProUGUI>().text = Regex.Unescape(client.amfitrion) +
                                                " has invited you to play with:\n" + mates + "Do you want to accpt the invitation?";
    }
    void Update()
    {
        if (client.invitationres == -1)
        {
            SceneManager.LoadScene("Menu");
        }
    }


}
