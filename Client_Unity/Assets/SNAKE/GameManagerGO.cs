using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerGO : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject playButton;
    // Start is called before the first frame update

    public void gameOver()
    {
        gameOverUI.SetActive(true);
        Pause();
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }

    public void Play()
    {

        playButton.SetActive(false);

        Time.timeScale = 1f;
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
    public void Pause()
    {
        Time.timeScale = 0f;
    }
}
