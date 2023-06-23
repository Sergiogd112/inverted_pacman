using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// This is the script for the back button in the chat scene.
/// </summary>
public class backbuttonchat : MonoBehaviour
{
    /// <summary>
    /// This is called when the user clicks the back button.
    /// </summary>
    public void BackClick()
    {
        SceneManager.LoadScene("PlayOnline");
    }
}
