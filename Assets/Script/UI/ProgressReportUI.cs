using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ProgressReportUI : MonoBehaviour
{
    [SerializeField] private LocalizedString dayString;
    [SerializeField] private LocalizedString totalPatientsServedString;
    [SerializeField] private LocalizedString totalPatientsTreatedString;
    [SerializeField] private LocalizedString totalPatientsFailedString;
    [SerializeField] private LocalizedString profitString;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI totalPatientsServedText;
    [SerializeField] private TextMeshProUGUI totalPatientsTreatedText;
    [SerializeField] private TextMeshProUGUI totalPatientsFailedText;
    [SerializeField] private TextMeshProUGUI profitText;
    [SerializeField] private GameObject progressBoard;
    [SerializeField] private TipsUIV2 tipsUI;
    private TimeManager tm;
    
    void Start() 
    {
        tm = TimeManager.instance;

        StartCoroutine(ShowProgress());
    }
    
    void OnEnable()
    {
       dayString.Arguments = new string[1];
       dayString.StringChanged += UpdateDayText;

       totalPatientsServedString.Arguments = new string[1];
       totalPatientsServedString.StringChanged += UpdateTotalPatientsServedText;

       totalPatientsTreatedString.Arguments = new string [1];
       totalPatientsTreatedString.StringChanged += UpdateTotalPatientsTreatedText;

       totalPatientsFailedString.Arguments = new string[1];
       totalPatientsFailedString.StringChanged += UpdateTotalPatientsFailedText;

       profitString.Arguments = new string[1];
       profitString.StringChanged += UpdateProfitText;
    }

    private void UpdateProfitText(string value) => profitText.text = value;

    private void UpdateTotalPatientsFailedText(string value) => totalPatientsFailedText.text = value;

    private void UpdateTotalPatientsTreatedText(string value) => totalPatientsTreatedText.text = value;

    private void UpdateTotalPatientsServedText(string value) => totalPatientsServedText.text = value;

    private void UpdateDayText(string value) => dayText.text = value; 

    void OnDisable() 
    {
       dayString.StringChanged -= UpdateDayText;
       totalPatientsServedString.StringChanged -= UpdateTotalPatientsServedText;
       totalPatientsTreatedString.StringChanged -= UpdateTotalPatientsTreatedText;
       totalPatientsFailedString.StringChanged -= UpdateTotalPatientsFailedText;
       profitString.StringChanged -= UpdateProfitText;
    }
    
    public IEnumerator ShowProgress()
    {
        yield return new WaitForSeconds(0.8f);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        UpdateProgress();
        LeanTween.value(this.gameObject, UpdateBackgroundAlpha, 0.0f, 1.0f, 0.5f);
        LeanTween.moveLocalY(progressBoard, 0.0f, 0.8f).setEaseSpring();
    }

    public void DisableProgressReport()
    {
        LeanTween.value(this.gameObject, UpdateBackgroundAlpha, 1.0f, 0.0f, 0.5f);
        LeanTween.moveLocalY(progressBoard, -970.0f, 0.8f).setEaseSpring().setOnComplete(() => tipsUI.Check());
    }

    private void UpdateBackgroundAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    private void UpdateProgress()
    {
        dayString.Arguments[0] = PlayerPrefs.GetInt("DaySaved").ToString();
        dayString.RefreshString();  
        totalPatientsServedString.Arguments[0] = PlayerPrefs.GetInt("Served").ToString();
        totalPatientsServedString.RefreshString();
        totalPatientsTreatedString.Arguments[0] = PlayerPrefs.GetInt("Treated").ToString();
        totalPatientsTreatedString.RefreshString();
        totalPatientsFailedString.Arguments[0] = PlayerPrefs.GetInt("Failed").ToString();
        totalPatientsFailedString.RefreshString();
        profitString.Arguments[0] = PlayerPrefs.GetFloat("Money").ToString("0.00") + " Kp";
        profitString.RefreshString();
    }
}
