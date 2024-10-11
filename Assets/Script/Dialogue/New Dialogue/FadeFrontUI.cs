using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFrontUI : MonoBehaviour
{
    [SerializeField] private RectTransform _FrontUI;

    public void Fade_WithFunction(Action functionAfterFade, float fadeDuration, float toAlpha)
    {
        if(toAlpha == 1)
        {
            FadeNormal(0,0);
            if(!_FrontUI.gameObject.activeSelf)_FrontUI.gameObject.SetActive(true);
        }
        
        LeanTween.alpha(_FrontUI, toAlpha, fadeDuration).setOnComplete(functionAfterFade);
        if(toAlpha == 0)
        {
            _FrontUI.gameObject.SetActive(false);
        }
    }
    public void FadeNormal(float fadeDuration, float toAlpha)
    {
        if(toAlpha == 1)
        {
            LeanTween.alpha(_FrontUI, 0, 0);
            if(!_FrontUI.gameObject.activeSelf)_FrontUI.gameObject.SetActive(true);
        }
        LeanTween.alpha(_FrontUI, toAlpha, fadeDuration);
        if(toAlpha == 0)
        {
            _FrontUI.gameObject.SetActive(false);
        }
    }
}
