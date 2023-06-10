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
/// This script manages the invite menu.
/// </summary>
public class conected : MonoBehaviour
{
    public Client client; // This is the client.
    public GameObject button; // This is the button prefab (where the invitee is shown).
    List<GameObject> buttons = new List<GameObject>(); // This is the list of buttons.
    /// <summary>
    /// This is called when the menu is created. It adds the players to the menu.
    /// </summary>
    void Start()
    {
        for (int i = 0; i < client.connected.Length; i++)
        {
            if (Regex.Unescape(client.connected[i]) == client.usuario) continue; // If the player is the user, skip it.
            addPlayer(Regex.Unescape(client.connected[i]));
        }
        client.updated_conected_list = false; // Set the updated list to false.
    }

    /// <summary>
    /// This is called once per frame. It updates the list of players.
    /// </summary>
    void Update()
    {
        if (client.updated_conected_list)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Destroy(buttons[i]);
            }
            buttons = new List<GameObject>();
            for (int i = 0; i < client.connected.Length; i++)
            {
                if (Regex.Unescape(client.connected[i]) == client.usuario) continue; // If the player is the user, skip it.
                addPlayer(Regex.Unescape(client.connected[i]));
            }
            client.updated_conected_list = false; // Set the updated list to false.
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
    /// <summary>
    /// This is called when the user clicks the invite button. It sends the invite to the server.
    /// </summary>
    public void invitar()
    {
        int count = 0;
        string mensage = "5/";

        foreach (GameObject user in buttons)
        {
            float alpha = user.GetComponent<Image>().color.a;
            if (alpha > .3f)
            {
                if (count == 0)
                {
                    mensage += Regex.Escape(user.GetComponentInChildren<TextMeshProUGUI>().text); // Add the player's name to the message.
                }
                else
                {
                    mensage += "*" + Regex.Escape(user.GetComponentInChildren<TextMeshProUGUI>().text); // Add the player's name to the message.
                }
                count++;
            }
        }
        UnityEngine.Debug.Log(mensage);
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensage); // Convert the message to bytes.
        client.server.Send(msg); // Send the message to the server.
        SceneManager.LoadScene("Loading"); // Load the invitation scene.
    }

}
