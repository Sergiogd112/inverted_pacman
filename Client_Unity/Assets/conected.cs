using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
            addPlayer(client.connected[i]);
        }
        client.updated_conected_list = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (client.updated_conected_list)
        {
            for(int i = 0; i < buttons.Count; i++){
                Destroy(buttons[i]);
            }
            for (int i = 0; i < client.connected.Length; i++)
            {
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
}
