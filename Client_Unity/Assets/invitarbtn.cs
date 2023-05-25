using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class invitarbtn : MonoBehaviour
{
    public VertexGradient unselected;
    public VertexGradient selected;
    public bool pressed = false;
    public void Toggle()
    {
        TextMeshPro textMeshPro = GetComponent<TextMeshPro>();
        textMeshPro.text = "patata";
        UnityEngine.Debug.Log("pressed");
        if (pressed)
        {
            textMeshPro.colorGradient = unselected;
            pressed = false;
        }
        else
        {
            textMeshPro.colorGradient = selected;
            pressed = true;
        }

    }
}
