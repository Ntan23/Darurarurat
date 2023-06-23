using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonFX : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start() => text = GetComponent<TextMeshProUGUI>();

    public void HoverFX() 
    {
        text.fontStyle = FontStyles.Underline;
        LeanTween.value(gameObject, UpdateFontSize, 60.0f, 70.0f, 0.5f).setEaseSpring();
    }

    public void UnhoverFX() 
    {
        text.fontStyle = FontStyles.Normal;
        LeanTween.value(gameObject, UpdateFontSize, 70.0f, 60.0f, 0.5f).setEaseSpring();
    }

    void UpdateFontSize(float value) => text.fontSize = value;
}
