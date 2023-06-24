using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonFX : MonoBehaviour
{
    private TextMeshProUGUI text;

    void Start() => text = GetComponent<TextMeshProUGUI>();

    public void HoverFX(float to) 
    {
        text.fontStyle = FontStyles.Underline;
        LeanTween.value(gameObject, UpdateFontSize, text.fontSize, to, 0.3f).setEaseSpring();
    }

    public void UnhoverFX(float to) 
    {
        text.fontStyle = FontStyles.Normal;
        LeanTween.value(gameObject, UpdateFontSize, text.fontSize, to, 0.3f).setEaseSpring();
    }

    void UpdateFontSize(float value) => text.fontSize = value;
}
