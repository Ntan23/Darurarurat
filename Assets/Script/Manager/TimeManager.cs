using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    #region Singleton
    public static TimeManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private int day;

    private const int hoursInDay = 24, minutesInHour = 60;

    [SerializeField] private float timeMultiplier;

    private float totalTime;
    private float currentTime;
    [SerializeField] private float startHour;
    private float startOffset;
    public bool canStart;
    private bool fromUpgrade;

    void Start()
    {
        day = PlayerPrefs.GetInt("DaySaved", 1);

        startOffset = (startHour / hoursInDay) * timeMultiplier;
        currentTime = (totalTime + startOffset) % timeMultiplier;
    }

    void Update() 
    {
        if(canStart && GetHour() <= 17) UpdateTimeOfDay();
    }

    private void UpdateTimeOfDay()
    {
        totalTime += Time.deltaTime;
        currentTime = (totalTime + startOffset) % timeMultiplier;
    }
 
    public void UpdateDay()
    {
        day++;
        PlayerPrefs.SetInt("DaySaved", day);
        PlayerPrefs.SetInt("Served", 0);
        PlayerPrefs.SetInt("Treated", 0);
        PlayerPrefs.SetInt("Failed", 0);

        PlayerPrefs.SetInt("IsTeaTime", 0);
        PlayerPrefs.SetInt("IsUpgrade", 0);
        
        Destroy(this.gameObject);
    }

    public void ChangeFromUpgradeValue(bool value) => fromUpgrade = value;

    public float GetHour()
    {
        return currentTime * hoursInDay / timeMultiplier;
    }

    public float GetMinutes()
    {
        return (currentTime * hoursInDay * minutesInHour / timeMultiplier) % minutesInHour;
    }

    public int GetDay()
    {
        return day;
    }

    public void ChangeCanStartValue(bool value) => canStart = value;
}
