using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// This is the script for the invite button.
/// </summary>
public class invitarbtn : MonoBehaviour
{
    public VertexGradient unselected; // This is the unselected color.
    public VertexGradient selected; // This is the selected color.
    public TMP_ColorGradient test; // This is the test color.
    public bool pressed = false; // This is whether the button is pressed or not.
    /// <summary>
    /// This is called when the button is clicked.
    /// </summary>
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
