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
            LeanTween.alpha(_FrontUI, 0, 0);
            if(!_FrontUI.gameObject.activeSelf)_FrontUI.gameObject.SetActive(true);
            // Debug.Log("frontUI1" + _FrontUI.gameObject.activeSelf);
        }
        // Debug.Log("frontUI12" + _FrontUI.gameObject.activeSelf + _FrontUI.name);
        LeanTween.alpha(_FrontUI, toAlpha, fadeDuration).setOnComplete(
            ()=>
            {
                if(toAlpha == 0)
                {
                    _FrontUI.gameObject.SetActive(false);
                }
                functionAfterFade();
            }
        );
        // Debug.Log("frontUI13" + _FrontUI.gameObject.activeSelf + _FrontUI.name);
        
    }
    public void FadeNormal(float fadeDuration, float toAlpha)
    {
        if(toAlpha == 1)
        {
            LeanTween.alpha(_FrontUI, 0, 0);
            if(!_FrontUI.gameObject.activeSelf)_FrontUI.gameObject.SetActive(true);
            // Debug.Log("frontUI2" + _FrontUI.gameObject.activeSelf);
        }
        // Debug.Log("frontUI22" + _FrontUI.gameObject.activeSelf + _FrontUI.name);
        LeanTween.alpha(_FrontUI, toAlpha, fadeDuration).setOnComplete(
            ()=>
            {
                if(toAlpha == 0)
                {
                    _FrontUI.gameObject.SetActive(false);
                }
            }
        );
        // Debug.Log("frontUI23" + _FrontUI.gameObject.activeSelf + _FrontUI.name);
        
    }
}
