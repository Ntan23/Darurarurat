using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropdownValueChange : MonoBehaviour
{
    private enum Type{
        Graphics, Languages
    }

    [SerializeField] private Type type;

    private int index;
    private int qualityLevel, languageIndex;
    private TMP_Dropdown dropdown;
    private SettingsUI settingsUI;

    void Start() 
    {
        qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
        languageIndex = PlayerPrefs.GetInt("LanguageIndex", 0);

        settingsUI = SettingsUI.instance;

        dropdown = GetComponent<TMP_Dropdown>();
        
        dropdown.onValueChanged.AddListener( delegate { DropDownSelected(dropdown);});

        if(type == Type.Graphics)
        {
            dropdown.value = qualityLevel;
            QualitySettings.SetQualityLevel(qualityLevel);
        }

        if(type == Type.Languages) dropdown.value = languageIndex;
    }

    private void DropDownSelected(TMP_Dropdown dropdown)
    {
        index = dropdown.value;

        if(type == Type.Graphics) settingsUI.UpdateQuality(index);
        if(type == Type.Languages) settingsUI.UpdateLanguages(index);
    }
}
