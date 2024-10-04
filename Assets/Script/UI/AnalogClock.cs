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

        tm.TimeChanged += OnTimeChanged;
    }
    void OnDestroy() => tm.TimeChanged -= OnTimeChanged;
    private void OnTimeChanged(object sender, DateTime newTime)
    {
        // hourHand.rotation = Quaternion.Euler(0,0, -newTime.Hour * hoursToDegrees);
        LeanTween.rotateZ(hourHand.gameObject, -newTime.Hour * hoursToDegrees, 0.1f).setEaseOutQuint();
        
        minuteHand.rotation = Quaternion.Euler(0, 0, -newTime.Minute * minutesToDegrees);
    }
}
