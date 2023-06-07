using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScenes : MonoBehaviour
{
    public static int SceneNumber;

    [SerializeField] Animator transitionAnim;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneNumber == 0)
        {
            StartCoroutine(ToSplashTwo());
        }
        if (SceneNumber == 6)
        {
            StartCoroutine(ToSplashOne());
        }
        if (SceneNumber == 7)
        {
            StartCoroutine(ToMenu());
        }
    }

    void Update()
    {
        if (SceneNumber == 6)
        {
            StartCoroutine(ToSplashOne());
        }
        if (SceneNumber == 7)
        {
            StartCoroutine(ToMenu());
        }

    }

    IEnumerator ToSplashTwo()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(2);
        SceneNumber = 6;
        SceneManager.LoadScene(6);
    }
    IEnumerator ToSplashOne()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(1);
        SceneNumber = 7;
        SceneManager.LoadScene(7);
    }

    IEnumerator ToMenu()
    {
        transitionAnim.SetTrigger("exit");
        yield return new WaitForSeconds(3);
        SceneNumber = 0;
        SceneManager.LoadScene("Menu");
    }
}