using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    private Light2D _light;
    private Light light3D;
    [SerializeField] private GameObject roomLight;
    private TimeManager tm;
    [SerializeField] private Gradient dayNightLightColorGradient;
    private const int hoursInDay = 24;
    [SerializeField] private bool is2D;

    void Awake() 
    {
        if(is2D) _light = GetComponent<Light2D>();
        if(!is2D) light3D = GetComponent<Light>();
    } 
    
    void Start() => tm = TimeManager.instance;

    void Update() 
    {
        ChangeLightColor();

        if(tm.GetHour() >= 17 && is2D) roomLight.SetActive(true);
        if(tm.GetHour() >= 7 && tm.GetHour() <= 17 && is2D) roomLight.SetActive(false);
    }

    private void ChangeLightColor() 
    {
        if(is2D) _light.color = dayNightLightColorGradient.Evaluate(PercentageOfDay());
        if(!is2D) light3D.color = dayNightLightColorGradient.Evaluate(PercentageOfDay());
    }
    
    private float PercentageOfDay()
    {
        return  tm.GetHour() % hoursInDay / hoursInDay;
    }
}
