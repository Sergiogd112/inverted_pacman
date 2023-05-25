using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    /// <summary>
    /// Cargar diferentes escenas
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
    }
}
