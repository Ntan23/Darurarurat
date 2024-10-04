using System;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private TimeManager tm;
    [SerializeField] RectTransform minuteHand;
    [SerializeField] RectTransform hourHand;

    private const float hoursToDegrees = 360/12;
    private const float minutesToDegrees = 360/60;

    void Start() 
    {
        tm = TimeManager.instance;
        
        UpdateClockHands();
    }

    void Update() => UpdateClockHands();

    private void UpdateClockHands()
    {
        hourHand.rotation = Quaternion.Euler(0,0, -tm.GetHour() * hoursToDegrees);
        minuteHand.rotation = Quaternion.Euler(0, 0, -tm.GetMinutes() * minutesToDegrees);
    }

    // private void OnTimeChanged(object sender, float newTime)
    // {
    //     // hourHand.rotation = Quaternion.Euler(0,0, -newTime.Hour * hoursToDegrees);
    //     LeanTween.rotateZ(hourHand.gameObject, -tm.GetHour() * hoursToDegrees, 0.1f).setEaseOutQuint();
        
    //     minuteHand.rotation = Quaternion.Euler(0, 0, -newTime.Minute * minutesToDegrees);
    // }
}
