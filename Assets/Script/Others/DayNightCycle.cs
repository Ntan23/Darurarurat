using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayNightCycle : MonoBehaviour
{
    private Light2D _light;
    [SerializeField] private GameObject roomLight;
    private TimeManager tm;
    [SerializeField] private Gradient dayNightLightColorGradient;
    private const int hoursInDay = 24;

    void Awake() => _light = GetComponent<Light2D>();

    void Start() => tm = TimeManager.instance;

    void Update() 
    {
        ChangeLightColor();

        if(tm.GetHour() >= 17) roomLight.SetActive(true);
        if(tm.GetHour() >= 7 && tm.GetHour() <= 17) roomLight.SetActive(false);
    }

    private void ChangeLightColor() => _light.color = dayNightLightColorGradient.Evaluate(PercentageOfDay());
    
    private float PercentageOfDay()
    {
        return  tm.GetHour() % hoursInDay / hoursInDay;
    }
}
