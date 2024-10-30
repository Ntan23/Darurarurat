using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System;

public class DayTextUI : MonoBehaviour
{
    private TextMeshProUGUI dayText;
    [SerializeField] private LocalizedString dayString;
    
    void Start() => UpdateText();
    
    void OnEnable() 
    {
        dayText = GetComponent<TextMeshProUGUI>();

        dayString.Arguments = new string[1];
        dayString.StringChanged += UpdateDayText;
    }
    
    void OnDisable() => dayString.StringChanged -= UpdateDayText;

    private void UpdateDayText(string value) => dayText.text = value;
    
    private void UpdateText()
    {
        dayString.Arguments[0] = PlayerPrefs.GetInt("DaySaved", 1).ToString();
        dayString.RefreshString();
    }
}
