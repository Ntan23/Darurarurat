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

    public event EventHandler<DateTime> TimeChanged;
    private int day;
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private TextMeshProUGUI digitalClockText;
    private DateTime currentTime;

    void Start() 
    {
        day = PlayerPrefs.GetInt("DaySaved", 1);

        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);
    }

    void Update() => UpdateTimeOfDay();

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);
        TimeChanged?.Invoke(this, currentTime);
        digitalClockText.text = currentTime.ToString("HH:mm");
    }
}
