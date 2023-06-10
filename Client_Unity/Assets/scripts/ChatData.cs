using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is the script for the chat data. It is a scriptable object. It is used to store the chat data.
/// </summary>
[CreateAssetMenu(fileName = "chatdata", menuName = "ScriptableObjects/ChatData", order = 1)] // This is used to create the scriptable object.

public class ChatData : ScriptableObject
{
    public List<string> users; // This is the list of users.
    public List<string> timestamps; // This is the list of timestamps.
    public List<string> messages; // This is the list of messages.
    public bool updated_chat = false; // This is used to check if the chat has been updated.

    void Awake()
    {
        users = new List<string>(); // Initialize the list of users.
        timestamps = new List<string>(); // Initialize the list of timestamps.
        messages = new List<string>(); // Initialize the list of messages.
        updated_chat = false; // Set the chat as not updated.
    }

    /// <summary>
    /// This is called when the chat is created. It initializes the lists.
    /// </summary>
    /// <param name="playerName"> The player's name.</param>
    /// <param name="timestamp"> The timestamp.</param>
    /// <param name="message"> The message.</param>
    public void addMessage(string playerName, string timestamp, string message)
    {
        users.Add(playerName); // Add the user.
        timestamps.Add(timestamp); // Add the timestamp.
        messages.Add(message); // Add the message.
    }

}
