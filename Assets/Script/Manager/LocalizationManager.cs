using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
    #region Singleton
    public static LocalizationManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    void Start() => ChangeLocale(PlayerPrefs.GetInt("LanguageIndex", 0));
    
    public void ChangeLocale(int localeID) => StartCoroutine(SetLocale(localeID));
    
    private IEnumerator SetLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
    }
}
