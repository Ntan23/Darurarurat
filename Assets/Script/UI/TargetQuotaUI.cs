using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using System;

public class TargetQuotaUI : MonoBehaviour
{
    private TextMeshProUGUI targetQuotaText;
    [SerializeField] private LocalizedString targetQuotaString;
    
    void Start() => UpdateText();
    
    void OnEnable() 
    {
        targetQuotaText = GetComponent<TextMeshProUGUI>();

        targetQuotaString.Arguments = new string[1];
        targetQuotaString.StringChanged += UpdateTargetQuotaText;
    }
    
    void OnDisable() => targetQuotaString.StringChanged -= UpdateTargetQuotaText;

    private void UpdateTargetQuotaText(string value) => targetQuotaText.text = value;
    
    private void UpdateText()
    {
        targetQuotaString.Arguments[0] = PlayerPrefs.GetFloat("TargetQuota", 10).ToString("0.00") + " Kp"; 
        targetQuotaString.RefreshString();
    }
}
