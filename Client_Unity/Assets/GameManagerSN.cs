using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerSN : MonoBehaviour
{
    public GameObject gameOverUI;
    // Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }
}
