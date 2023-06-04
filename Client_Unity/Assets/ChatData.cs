using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "chatdata", menuName = "ScriptableObjects/ChatData", order = 1)]

public class ChatData : ScriptableObject
{
    public List<string> users;
    public List<string> timestamps;
    public List<string> messages;
    public bool updated_chat = false;

    public void addMessage(string playerName, string timestamp, string message)
    {
        users.Add(playerName);
        timestamps.Add(timestamp);
        messages.Add(message);
    }

}
