using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerWO : MonoBehaviour
{
    public GameObject gameOverUI;
    // Start is called before the first frame update

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MenuOffline()
    {
        SceneManager.LoadScene("PlayOffline");
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }
}
