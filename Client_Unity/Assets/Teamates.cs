using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class Teamates : MonoBehaviour
{
    public Client client; // This is the client.
    public GameObject button; // This is the button prefab (where the invitee is shown).
    List<GameObject> buttons = new List<GameObject>(); // This is the list of buttons.
    /// <summary>
    /// This is called when the menu is created. It adds the players to the menu.
    /// </summary>
    void Start()
    {
        client.Save(); // Save the client data.
        client.updated_usjugado = false; // Set the updated list to false.
        string message = "12/" + client.usuario;
        byte[] msg = Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);

    }

    /// <summary>
    /// This is called once per frame. It updates the list of players.
    /// </summary>
    void Update()
    {
        if (client.updated_usjugado)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
            }
            buttons = new List<GameObject>();
            for (int i = 0; i < client.usjugado.Length; i++)
            {
                if (Regex.Unescape(client.usjugado[i]) == client.usuario) continue; // If the player is the user, skip it.
                addPlayer(Regex.Unescape(client.usjugado[i]));
            }
            client.updated_usjugado = false; // Set the updated list to false.
        }


    }
    /// <summary>
    /// This adds a player to the list.
    /// </summary>
    /// <param name="playerName"> The player's name.</param>
    void addPlayer(string playerName)
    {
        GameObject childOb = Instantiate(button, transform.position, Quaternion.identity); // Instantiate the button.

        childOb.GetComponentInChildren<TextMeshProUGUI>().text = playerName; // Set the button's text to the player's name.
        childOb.transform.SetParent(transform); // Set the button's parent to the menu.
        buttons.Add(childOb); // Add the button to the list.
    }
}
