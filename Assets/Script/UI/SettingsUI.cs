using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    #region Singleton
    public static SettingsUI instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    public AudioMixer MainMixer;
    // private TMP_Dropdown QualityDropdown;
    // private TMP_Dropdown LanguagesDropdown;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Toggle fullscreenToogle;
    private float bgmVolume;
    private float sfxVolume;
    private int isFullscreen;
    private int width, height;

    private void Start()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0);
        isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1);

        BGMSlider.value = bgmVolume;
        SFXSlider.value = sfxVolume;
        MainMixer.SetFloat("BGM_Volume", bgmVolume);
        MainMixer.SetFloat("SFX_Volume", sfxVolume);

        if(isFullscreen == 1) 
        {
            fullscreenToogle.isOn = true;
            Screen.fullScreen = true;
            width = PlayerPrefs.GetInt("Width", Screen.currentResolution.width);
            height = PlayerPrefs.GetInt("Height", Screen.currentResolution.height);
            Screen.SetResolution(width, height, true);
        }
        else if(isFullscreen == 0) 
        {
            fullscreenToogle.isOn = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            width = PlayerPrefs.GetInt("Width",Screen.currentResolution.width);
            height = PlayerPrefs.GetInt("Height", Screen.currentResolution.height);
            Screen.SetResolution(Mathf.RoundToInt(width / 1.5f), Mathf.RoundToInt(height / 1.5f), false);
        }
    }

    public void UpdateBGMSound(float value)
    {
        MainMixer.SetFloat("BGM_Volume", value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
    public void UpdateSFXSound(float value)
    {
        MainMixer.SetFloat("SFX_Volume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void UpdateQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void UpdateLanguages(int languageIndex)
    {
        LocalizationManager.instance.ChangeLocale(languageIndex);
        PlayerPrefs.SetInt("LanguageIndex", languageIndex);
    }

    public void UpdateFullscreen(bool isFullscreen)
    {
        if (isFullscreen) 
        {
            Screen.fullScreen = isFullscreen;
            width = PlayerPrefs.GetInt("Width", Screen.currentResolution.width);
            height = PlayerPrefs.GetInt("Height", Screen.currentResolution.height);
            Screen.SetResolution(width, height, true);
            PlayerPrefs.SetInt("IsFullscreen", 1);
        }
        else if (!isFullscreen) 
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            width = PlayerPrefs.GetInt("Width", Screen.currentResolution.width);
            height = PlayerPrefs.GetInt("Height", Screen.currentResolution.height);
            Screen.SetResolution(Mathf.RoundToInt(width / 1.5f), Mathf.RoundToInt(height / 1.5f), false);
            PlayerPrefs.SetInt("IsFullscreen", 0);
        }
    }

    public void OpenSettings() => LeanTween.moveLocalY(gameObject, 0.0f, 0.8f).setEaseSpring();

    public void CloseSettings() => LeanTween.moveLocalY(gameObject, -1092.0f, 0.8f).setEaseSpring();
}
