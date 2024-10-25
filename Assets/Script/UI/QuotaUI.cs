using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using TMPro;
using System;

public class QuotaUI : MonoBehaviour
{
    [SerializeField] private QuotaChecker quotaChecker;
    [SerializeField] private LocalizedString targetQuotaString;
    [SerializeField] private TextMeshProUGUI targetQuotaText;
    [SerializeField] private LocalizedString neededQuotaString;
    [SerializeField] private TextMeshProUGUI neededQuotaText;

    void OnEnable()
    {
        targetQuotaString.Arguments = new string[1];
        neededQuotaString.Arguments = new string[1];

        targetQuotaString.StringChanged += UpdateTargetQuota;
        neededQuotaString.StringChanged += UpdateNeededQuota;
    }

    void OnDisable()
    {
        targetQuotaString.StringChanged -= UpdateTargetQuota;
        neededQuotaString.StringChanged -= UpdateNeededQuota;
    }

    private void UpdateNeededQuota(string value) => neededQuotaText.text = value;
    private void UpdateTargetQuota(string value) => targetQuotaText.text = value;

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenNCloseTargetAchievedWindow()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        targetQuotaString.Arguments[0] = PlayerPrefs.GetFloat("TargetQuota", 10).ToString("0.00"); 
        targetQuotaString.RefreshString();

        StartCoroutine(OpenNCloseWindowAnimation(true));
    }

    public void OpenNCloseTargetNotAchievedWindow()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        neededQuotaString.Arguments[0] = (PlayerPrefs.GetFloat("Quota", 10) - PlayerPrefs.GetFloat("Money", 0)).ToString("0.00"); 
        neededQuotaString.RefreshString();

        StartCoroutine(OpenNCloseWindowAnimation(false));
    }
    
    IEnumerator OpenNCloseWindowAnimation(bool value)
    {
        LeanTween.value(this.gameObject, UpdateAlpha, 0.0f, 1.0f, 0.8f);
        yield return new WaitForSeconds(2.5f);
        if(!value) ResetData();
        ScenesManager.instance.GoToTargetScene("PatientReception");
    }

    private void ResetData()
    {
        PlayerPrefs.SetInt("DaySaved", 1);
        PlayerPrefs.SetInt("Served", 0);
        PlayerPrefs.SetInt("Treated", 0);
        PlayerPrefs.SetInt("Failed", 0);
        PlayerPrefs.SetFloat("Money", 0);
        PlayerPrefs.SetFloat("TargetQuota", 10);

        PlayerPrefs.SetInt("IsTeaTime", 0);
        PlayerPrefs.SetInt("IsUpgrade", 0);
    }
}
