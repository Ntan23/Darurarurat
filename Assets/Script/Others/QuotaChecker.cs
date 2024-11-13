using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotaChecker : MonoBehaviour
{
    private float targetQuota;
    [SerializeField] private float everyHowMuchDay;
    [SerializeField] private QuotaUI[] quotaUIs;
    private TimeManager tm;

    void Start() 
    {
        tm = TimeManager.instance;

        targetQuota = PlayerPrefs.GetFloat("TargetQuota",  50.0f);
    }

    public void CheckQuota()
    {
        if(tm.GetDay() % everyHowMuchDay == 0)
        {
            if(MoneyManager.instance.GetCurrentMoney() < targetQuota)
            {
                Debug.Log("Not Pass");
                quotaUIs[1].OpenNCloseTargetNotAchievedWindow();
            }

            if(MoneyManager.instance.GetCurrentMoney() >= targetQuota)
            {
                Debug.Log("Pass");
                targetQuota *= 2;

                PlayerPrefs.SetFloat("TargetQuota", targetQuota);
                
                quotaUIs[0].OpenNCloseTargetAchievedWindow();
            }

        }

        if(tm.GetDay() % everyHowMuchDay != 0) ScenesManager.instance.GoToTargetScene("PatientReception");

        tm.UpdateDay();
    }
}
