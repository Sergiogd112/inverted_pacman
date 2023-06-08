using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class backbuttonchat : MonoBehaviour
{
    public void BackClick()
    {
        SceneManager.LoadScene("chat");
    }
}
