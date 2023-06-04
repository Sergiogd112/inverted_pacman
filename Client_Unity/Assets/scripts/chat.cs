using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class chat : MonoBehaviour
{
    public Client client;
    public GameObject message;
    public ChatData chatdata;
    public int count = 0;
    public TMP_InputField messageField;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = count; i < chatdata.users.Count; i++)
        {
            addMessage(chatdata.users[i], chatdata.timestamps[i], chatdata.messages[i]);
        }
        count = chatdata.users.Count;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = count; i < chatdata.users.Count; i++)
        {
            addMessage(chatdata.users[i], chatdata.timestamps[i], chatdata.messages[i]);
        }
        count = chatdata.users.Count;
    }
    void addMessage(string user, string time, string text)
    {
        GameObject messgObj = Instantiate(message, transform.position, Quaternion.identity);
        messgObj.transform.SetParent(transform);
        messgObj.transform.Find("Sender").GetComponent<TextMeshProUGUI>().text = user + " at:" + time;
        messgObj.transform.Find("text").GetComponent<TextMeshProUGUI>().text = text;
    }
    public void Send()
    {
        string text = messageField.text;
        if (text != "")
        {
            string message = "9/" + Regex.Escape(text);
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(message);
            client.server.Send(msg);
        }
    }
}
