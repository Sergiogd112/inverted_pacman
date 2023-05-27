using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerFB : MonoBehaviour
{
    public Player player;

    public GameObject playButton;
    public GameObject gameOverUI;
    private int score; 

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Pause();
    }

    public void Play()
    {
        score = 0;

        playButton.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void GameOver()
    {
        //playButton.SetActive(true);
        gameOverUI.SetActive(true);
        Pause();
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
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
    }
}
