using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    #region  Singleton
    public static MoneyManager instance;
    void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    #endregion
    
    private PatientsQueueManager pqm;
    private TextMeshProUGUI moneyText;
    private float currentMoney;

    void Start() 
    {
        pqm = PatientsQueueManager.instance;

        currentMoney = PlayerPrefs.GetFloat("Money", 0);
        if(pqm != null) moneyText = pqm.GetMoneyText();

        if(moneyText != null) moneyText.text = currentMoney.ToString("0.00") + " Rp";

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) => ShowMoney();
    public void AddMoney(float money) => currentMoney += money;
    public void DecreaseMoney(float money) 
    {
        currentMoney -= money;

        if(currentMoney < 0) currentMoney = 0;
    }
    
    public void ResetCurrentMoney() => currentMoney = 0;

    private void ShowMoney() 
    {
        if(SceneManager.GetActiveScene().name == "PatientReception") StartCoroutine(ShowCurrentMoney());
    }

    public void SaveMoney() => PlayerPrefs.SetFloat("Money", currentMoney);

    IEnumerator ShowCurrentMoney()
    {
        pqm = PatientsQueueManager.instance;
        moneyText = pqm.GetMoneyText();
        yield return new WaitForSeconds(0.1f);
        moneyText.text = currentMoney.ToString("0.00") + " Rp";
    }

    public float GetCurrentMoney()
    {
        return currentMoney;
    }
}

