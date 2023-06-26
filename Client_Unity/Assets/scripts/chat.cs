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
/// This is the script for the chat. It is used to display the chat messages.
/// </summary>
public class chat : MonoBehaviour
{
    public Client client; // This is the client.
    public GameObject message; // This is the message prefab.
    public ChatData chatdata; // This is the chat data.
    public int count = 0;   // This is the number of messages.
    public TMP_InputField messageField; // This is the message field.
    public List<GameObject> messageList;
    public int maxMessages = 10;
    /// <summary>
    /// This is called when the chat is created. It adds the messages to the chat.
    /// </summary>
    void Start()
    {
        client.Save(); // Save the client data.
        chatdata.Save(); // Save the chat data.
        for (count = (chatdata.users.Count - maxMessages) * Convert.ToInt32(maxMessages < chatdata.users.Count); count < chatdata.users.Count; count++) 
        {
            addMessage(chatdata.users[count], chatdata.timestamps[count], chatdata.messages[count]); // Add the message.
        }
    }

    /// <summary>
    /// This is called once per frame. It adds the messages to the chat if they are new.
    /// </summary>
    void Update()
    {
        if (count > 0)
        {
            foreach (GameObject message in messageList) // Destroy the old messages.
            {
                Destroy(message);
            }
        }
        int i;
        for (i = (chatdata.users.Count - maxMessages) * Convert.ToInt32(maxMessages < chatdata.users.Count); i < chatdata.users.Count; i++)
        {
            addMessage(chatdata.users[i], chatdata.timestamps[i], chatdata.messages[i]);
        }
        count = i;
        chatdata.updated_chat = false; // Set the updated_chat to false.
    }
    /// <summary>
    /// Adds a message to the chat scroll view. It is called when a new message is received.
    /// </summary>
    /// <param name="user"> The user who sent the message.</param>
    /// <param name="time"> The time the message was sent.</param>
    /// <param name="text"> The text of the message.</param>
    void addMessage(string user, string time, string text)
    {
        GameObject messgObj = Instantiate(message, transform.position, Quaternion.identity); // Instantiate the message.
        messgObj.transform.SetParent(transform); // Set the parent.
        messgObj.transform.Find("Sender").GetComponent<TextMeshProUGUI>().text = user + " at:" + time; // Set the sender.
        messgObj.transform.Find("text").GetComponent<TextMeshProUGUI>().text = text; // Set the text.
        messageList.Add(messgObj); // Add the message to the list.
    }
    /// <summary>
    /// This is called when the user clicks the send button. It sends the message to the server.
    /// </summary>
    public void Send()
    {
        string text = messageField.text; // Get the text.
        if (text != "") // If the text is not empty.
        {
            string message = "9/" + Regex.Escape(text); // Create the message.
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message); // Convert the message to bytes.
            client.server.Send(msg); // Send the message.
        }
    }
}
