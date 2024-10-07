using System;
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

    private int day;

    private const int hoursInDay = 24, minutesInHour = 60;

    [SerializeField] private float timeMultiplier;

    private float totalTime;
    private float currentTime;
    [SerializeField] private float startHour;
    private float startOffset;

    [SerializeField] private TextMeshProUGUI digitalClockText;
    public bool canStart;

    void Start()
    {
        day = PlayerPrefs.GetInt("DaySaved", 1);

        startOffset = (startHour / hoursInDay) * timeMultiplier;
        currentTime = (totalTime + startOffset) % timeMultiplier;
        
        digitalClockText.SetText(Clock24Hour());
    }

    void Update() 
    {
        if(canStart) UpdateTimeOfDay();
    }

    private void UpdateTimeOfDay()
    {
        totalTime += Time.deltaTime;
        currentTime = (totalTime + startOffset) % timeMultiplier;
        digitalClockText.SetText(Clock24Hour());
    }

    public float GetHour()
    {
        return currentTime * hoursInDay / timeMultiplier;
    }

    public float GetMinutes()
    {
        return (currentTime * hoursInDay * minutesInHour / timeMultiplier)%minutesInHour;
    }

    public string Clock24Hour()
    {
        return Mathf.FloorToInt(GetHour()).ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00");
    }

}
