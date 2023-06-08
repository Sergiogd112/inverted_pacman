using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit : MonoBehaviour
{
    /// <summary>
    /// This is called when the user clicks the Quit button.
    /// </summary>
    public void QuitClick()
    {
        Application.Quit();
    }
}