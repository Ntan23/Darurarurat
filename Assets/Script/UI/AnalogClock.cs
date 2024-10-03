using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : MonoBehaviour
{
    private TimeManager tm;
    [SerializeField] RectTransform minutesHand;
    [SerializeField] RectTransform hourHand;

    private const float hoursToDegrees = 360/12;
    private const float minutesToDegrees = 360/60;

    void Start()
    {
        tm = TimeManager.instance;
    }

    void Update()
    {
        hourHand.rotation = Quaternion.Euler(0, 0, -tm.GetHour() * hoursToDegrees);
        minutesHand.rotation = Quaternion.Euler(0, 0, -tm.GetMinutes() * minutesToDegrees);
    }
}
