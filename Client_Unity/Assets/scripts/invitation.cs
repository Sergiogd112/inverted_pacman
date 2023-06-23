using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

/// <summary>
/// This is the script for the invitation.
/// </summary>
public class invitation : MonoBehaviour
{
    public Client client; // This is the client. 
    /// <summary>
    /// This is called when the invitation is created.
    /// </summary>
    void Start()
    {
        string mates = "";
        for (int i = 0; i < 3; i++)
        {
            if (client.invitados[i] == client.usuario) continue; // If the player is the user, skip it.
            mates += Regex.Unescape(client.invitados[i]) + "\n"; // Add the player to the list.
        }
        GetComponent<TextMeshProUGUI>().text = Regex.Unescape(client.amfitrion) +
                                                " has invited you to play with:\n" + mates + "Do you want to accpt the invitation?"; // Set the invitation text.
    }
    void Update()
    {
        if (client.invitationres == -1) // If the invitation was rejected.
        {
            SceneManager.LoadScene("Menu");
        }
    }


}
