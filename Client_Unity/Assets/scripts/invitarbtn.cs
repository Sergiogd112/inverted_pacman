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
    public Client client; // This is the client.
    public int i=-1;
    /// <summary>
    /// This is called when the button is clicked.
    /// </summary>
    public void Toggle()
    {
        TextMeshProUGUI textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        UnityEngine.Debug.Log("pressed");
        UnityEngine.Debug.Log(pressed);
        if (pressed)
        {
            // textMeshPro.colorGradient = unselected;
            // color.a = 0.0f;
            // button.color = color;
            client.sel[i]=0;
            i=-1;
            pressed = false;
        }
        else
        {
            // textMeshPro.colorGradient = selected;
            for(int j=0;j<3;j++){
                if(client.sel[j]==0){
                    client.sel[j]=1;
                    i=j;
                    client.comp[i]=textMeshPro.text;
                    break;
                }
            }
            pressed = true;
        }

    }
}
