using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chooseplayers : MonoBehaviour
{
    public void pressed()
    {
        SceneManager.LoadScene("Invitar");
    }

}
