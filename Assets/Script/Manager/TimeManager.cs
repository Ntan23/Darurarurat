using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private TextMeshProUGUI digitalClockText;
    private DateTime currentTime;

    void Start() => currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

    void Update()
    {
        UpdateTimeOfDay();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        digitalClockText.text = currentTime.ToString("HH:mm");
    }
    
    public float GetHour()
    {
        return currentTime.Hour;
    }

    public float GetMinutes()
    {
        return currentTime.Minute;
    }
}
