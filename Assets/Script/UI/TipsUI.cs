using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsUI : MonoBehaviour
{
    private bool alreadyShow;
    private TimeManager tm;

    void Start()
    {
        tm = TimeManager.instance;

        if(PlayerPrefs.GetInt("TipsShowed", 0) == 0) alreadyShow = false;
        if(PlayerPrefs.GetInt("TipsShowed", 0) == 1) alreadyShow = true;

        if(!alreadyShow) gameObject.SetActive(true);
        else if(alreadyShow) gameObject.SetActive(false);
    }

    public void CloseTipsUI() 
    {
        PlayerPrefs.SetInt("TipsShowed", 1);

        LeanTween.moveLocalY(this.gameObject, 850.0f, 0.5f).setOnComplete(() => tm.canStart = true);
    }
}
