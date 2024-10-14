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
    private bool haveSpecialNPC;

    void Start()
    {
        day = PlayerPrefs.GetInt("DaySaved", 1);

        startOffset = (startHour / hoursInDay) * timeMultiplier;
        currentTime = (totalTime + startOffset) % timeMultiplier;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(day == 1 || day % 6 == 0) haveSpecialNPC = true;
        else haveSpecialNPC = false;
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
        
        //reset Time
        // startOffset = (startHour / hoursInDay) * timeMultiplier;
        // currentTime = (totalTime + startOffset) % timeMultiplier;

        // canStart = false;

        Destroy(this.gameObject);
    }

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

    public bool GetHaveSpecialNPC()
    {
        return haveSpecialNPC;
    }
}
