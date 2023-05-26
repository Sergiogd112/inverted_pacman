using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class invitarbtn : MonoBehaviour
{
    public VertexGradient unselected;
    public VertexGradient selected;
    public TMP_ColorGradient test;
    public bool pressed = false;
    public void Toggle()
    {
        // TextMeshProUGUI textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        UnityEngine.Debug.Log("pressed");
        UnityEngine.Debug.Log(pressed);
        Image button = GetComponent<Image>();
        Color color = button.color;
    
        if (pressed)
        {
            // textMeshPro.colorGradient = unselected;
            color.a = 0.0f;
            button.color = color;
            pressed = false;
        }
        else
        {
            // textMeshPro.colorGradient = selected;
            color.a = 0.5f;
            button.color = color;
            pressed = true;
        }

    }
}
