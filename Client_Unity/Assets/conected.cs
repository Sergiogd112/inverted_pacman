using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text;
using UnityEngine.SceneManagement;

public class conected : MonoBehaviour
{
    public Client client;
    public GameObject button;
    List<GameObject> buttons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < client.connected.Length; i++)
        {
            if (client.connected[i] == client.usuario) continue;
            addPlayer(client.connected[i]);
        }
        client.updated_conected_list = false;
    }

    // Update is called once per frame
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
                if (client.connected[i] == client.usuario) continue;
                addPlayer(client.connected[i]);
            }
            client.updated_conected_list = false;
        }

    }
    void addPlayer(string playerName)
    {
        GameObject childOb = Instantiate(button, transform.position, Quaternion.identity);

        childOb.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
        childOb.transform.SetParent(transform);
        buttons.Add(childOb);
    }
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
                    mensage += user.GetComponentInChildren<TextMeshProUGUI>().text;
                }
                else
                {
                    mensage += "*" + user.GetComponentInChildren<TextMeshProUGUI>().text;
                }
                count++;
            }
        }
        UnityEngine.Debug.Log(mensage);
        byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensage);
        client.server.Send(msg);
        SceneManager.LoadScene("Invitacion");
    }

}
