using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerFB : MonoBehaviour
{
    public Player player;
    //private Spawner spawner;

    //public Text scoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public GameObject MenuOffButton;
    private int score; 

    private void Awake()
    {
        Application.targetFrameRate = 60;

        //player = FindObjectOfType<Player>();
    //    spawner = FindObjectOfType<Spawner>();

        Pause();
    }

    public void Play()
    {
        score = 0;
        //scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOver.SetActive(false);
        MenuOffButton.SetActive(false);

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
        playButton.SetActive(true);
        gameOver.SetActive(true);
        MenuOffButton.SetActive(true);

        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
        //scoreText.text = score.ToString();
    }
}
