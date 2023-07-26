using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void ChangeLocale(int localeID) => StartCoroutine(SetLocale(localeID));
    
    private IEnumerator SetLocale(int localeID)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
    }
}
