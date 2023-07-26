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
    private TMP_Dropdown dropdown;
    private SettingsUI settingsUI;

    void Start() 
    {
        settingsUI = SettingsUI.instance;

        dropdown = GetComponent<TMP_Dropdown>();
        
        dropdown.onValueChanged.AddListener( delegate { DropDownSelected(dropdown);});
    }

    private void DropDownSelected(TMP_Dropdown dropdown)
    {
        index = dropdown.value;

        if(type == Type.Graphics) settingsUI.UpdateQuality(index);
        if(type == Type.Languages) settingsUI.UpdateLanguages(index);
    }
}
