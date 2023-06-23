using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class History : MonoBehaviour
{
    public Client client; // This is the client.
    public GameObject infoPartida; // This is the button prefab (where the invitee is shown).
    List<GameObject> history = new List<GameObject>(); // This is the list of buttons.
    public TMP_InputField userField; // This is the message field.

    /// <summary>
    /// This is called when the menu is created. It adds the players to the menu.
    /// </summary>
    void Start()
    {
        client.Save(); // Save the client data.
        client.updated_usjugado = false; // Set the updated list to false.
        string message = "14/" + client.usuario;
        byte[] msg = Encoding.ASCII.GetBytes(message);
        client.server.Send(msg);

    }

    /// <summary>
    /// This is called once per frame. It updates the list of players.
    /// </summary>
    void Update()
    {
        if (client.updated_partidas)
        {
            for (int i = 0; i < history.Count; i++)
            {
                Destroy(history[i]);
            }
            history = new List<GameObject>();
            for (int i = 0; i < client.partidas.Length; i++)
            {
                addGame(Regex.Unescape(client.partidas[i]),userField.text);
            }
            client.updated_partidas = false; // Set the updated list to false.
        }


    }
    void addGame(string data,string name)
    {
        int i;
        string[] parts = data.Split('*');
        for (i = 1; i < 5; i++)
        {
            if (parts[i].Contains(name))
            {
                break;
            }
        }
        if (i == 5)
        {
            return;
        }
        GameObject messgObj = Instantiate(infoPartida, transform.position, Quaternion.identity); // Instantiate the message.
        messgObj.transform.SetParent(transform); // Set the parent.
        messgObj.transform.Find("ID").GetComponent<TextMeshProUGUI>().text = parts[0]; // Set the text.
        messgObj.transform.Find("Player1").GetComponent<TextMeshProUGUI>().text = parts[1]; // Set the text.
        messgObj.transform.Find("Player2").GetComponent<TextMeshProUGUI>().text = parts[2]; // Set the text.
        messgObj.transform.Find("Player3").GetComponent<TextMeshProUGUI>().text = parts[3]; // Set the text.
        messgObj.transform.Find("Player4").GetComponent<TextMeshProUGUI>().text = parts[4]; // Set the text.
        messgObj.transform.Find("Points").GetComponent<TextMeshProUGUI>().text = parts[5]; // Set the text.
        history.Add(messgObj); // Add the message to the list.
    }

}
