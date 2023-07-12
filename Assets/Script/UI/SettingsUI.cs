using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public AudioMixer MainMixer;
    [SerializeField] TMP_Dropdown QualityDropdown;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Toggle fullscreenToogle;
    private float bgmVolume;
    private float sfxVolume;
    private int isFullscreen;
    private int qualityLevel;

    void Awake()
    {
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 0);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0);
        isFullscreen = PlayerPrefs.GetInt("IsFullscreen", 1);
        qualityLevel = PlayerPrefs.GetInt("QualityLevel", 2);
    }

    private void Start()
    {
        BGMSlider.value = bgmVolume;
        SFXSlider.value = sfxVolume;
        MainMixer.SetFloat("BGM_Volume", bgmVolume);
        MainMixer.SetFloat("SFX_Volume", sfxVolume);

        if(isFullscreen == 1) 
        {
            fullscreenToogle.isOn = true;
            Screen.fullScreen = true;
            Screen.SetResolution(1920, 1080, true);
        }
        else if(isFullscreen == 0) 
        {
            fullscreenToogle.isOn = false;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
        }
        
        QualityDropdown.value = qualityLevel;
        QualitySettings.SetQualityLevel(qualityLevel);
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

    public void UpdateFullscreen(bool isFullscreen)
    {
        if (isFullscreen) 
        {
            Screen.fullScreen = isFullscreen;
            Screen.SetResolution(1920, 1080, true);
            PlayerPrefs.SetInt("IsFullscreen", 1);
        }
        else if (!isFullscreen) 
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
            PlayerPrefs.SetInt("IsFullscreen", 0);
        }
    }

    public void OpenSettings() => LeanTween.moveLocalY(gameObject, 0.0f, 0.8f).setEaseSpring();

    public void CloseSettings() => LeanTween.moveLocalY(gameObject, -1092.0f, 0.8f).setEaseSpring();
}
