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

    //public AudioMixer MainMixer;
    // private TMP_Dropdown QualityDropdown;
    // private TMP_Dropdown LanguagesDropdown;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Toggle fullscreenToogle;
    private float bgmVolume;
    private float sfxVolume;
    private int isFullscreen;
    private int width, height;
    private AudioManager am;
    
    private void Start()
    {
        am = AudioManager.instance;

        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1);
        isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1);


        // MainMixer.SetFloat("BGM_Volume", bgmVolume);
        // MainMixer.SetFloat("SFX_Volume", sfxVolume);
        // am.SetBGMVolume(bgmVolume);
        // am.SetSFXVolume(sfxVolume);

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

        StartCoroutine(UpdateVolume());
    }

    public void UpdateBGMSound(float value)
    {
        // MainMixer.SetFloat("BGM_Volume", value);
        am.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void UpdateSFXSound(float value)
    {
        //MainMixer.SetFloat("SFX_Volume", value);
        am.SetSFXVolume(value);
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

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenSettings()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f);
    }
    //LeanTween.moveLocalY(gameObject, 0.0f, 0.8f).setEaseSpring();
    public void CloseSettings() => LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    });
    //LeanTween.moveLocalY(gameObject, -1092.0f, 0.8f).setEaseSpring();

    IEnumerator UpdateVolume()
    {
        yield return new WaitForSeconds(0.1f);
        BGMSlider.value = bgmVolume;
        SFXSlider.value = sfxVolume;
        
        am.SetBGMVolume(bgmVolume);
        am.SetSFXVolume(sfxVolume);
    }
}
