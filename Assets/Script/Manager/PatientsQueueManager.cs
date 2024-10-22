using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PatientsQueueManager : MonoBehaviour
{
    public static PatientsQueueManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    [SerializeField] private List<PatientWoundSO> allWounds;
    [SerializeField] private List<PatientWoundSO> availableWounds;
    [SerializeField] private int medsCount;
    private int randomPatientsIndex;
    private bool[] isUnlocked = new bool[7];
    [SerializeField] private GameObject[] patients;
    [SerializeField] private GameObject[] specialPatients;
    private GameObject spawnedPatient;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform[] positions;
    [SerializeField] private TextMeshProUGUI moneyText;
    private int currentSpecialID = 0;
    private int totalPatientServed;
    private int totalPatientTreated;
    private int totalPatientFailed;
    private bool specialPatientSpawned;
    private bool haveSpecialNPC;
    private TimeManager tm; 

    void Start()
    {
        tm = TimeManager.instance;
        
        //PlayerPrefs.SetInt("SpecialSpawned", 0);
        if(tm.GetDay() == 1 || tm.GetDay() % 6 == 0) haveSpecialNPC = true;
        else haveSpecialNPC = false;

        if(haveSpecialNPC)
        {
            if(PlayerPrefs.GetInt("SpecialSpawned", 0) == 0) specialPatientSpawned = false;
            else if(PlayerPrefs.GetInt("SpecialSpawned", 0) == 1) specialPatientSpawned = true;
        }

        currentSpecialID = PlayerPrefs.GetInt("Special", 0);

        totalPatientServed = PlayerPrefs.GetInt("Served", 0);
        totalPatientTreated = PlayerPrefs.GetInt("Treated", 0);
        totalPatientFailed = PlayerPrefs.GetInt("Failed", 0);
 
        StartCoroutine(CheckAvailableInjuries());
    }

    private void CheckObject()
    {
        if(PlayerPrefs.GetInt("Object1", 0) == 0 || PlayerPrefs.GetInt("Object2", 0) == 0)
        {
            PlayerPrefs.SetInt("Object1", 1);
            PlayerPrefs.SetInt("Object2", 1);
        }

        /*if Object index - n value is 1 , its mean true (already unlocked and vice versa*/
        for(int i = 0; i < medsCount; i ++)
        {
            if(PlayerPrefs.GetInt("Object" + i.ToString(), 0) == 1) isUnlocked[i] = true;
            if(PlayerPrefs.GetInt("Object" + i.ToString(), 0) == 0) isUnlocked[i] = false;
        }
    }

    public void GenerateQueue()
    {
        randomPatientsIndex = Random.Range(0, patients.Length);
        
        spawnedPatient = Instantiate(patients[randomPatientsIndex]);
        spawnedPatient.transform.position = spawnPos.position;
        GenerateWoundsToPatient();
    }

    public void GenerateWoundsToPatient()
    {
        int randomIndex = Random.Range(0, availableWounds.Count);
        
        spawnedPatient.GetComponent<Patients>().SetWound(availableWounds[randomIndex]);
        spawnedPatient.GetComponent<Patients>().SetWoundSprite(availableWounds[randomIndex]);
        spawnedPatient.GetComponent<Patients>().SetTargetPositions(positions);
    }

    IEnumerator CheckAvailableInjuries()
    {
        CheckObject();
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < allWounds.Count; i++)
        {
            for(int j = 0; j < allWounds[i].objectNeededToTreatIndex.Length ; j++)
            {
                if(j == allWounds[i].objectNeededToTreatIndex.Length - 1 && isUnlocked[allWounds[i].objectNeededToTreatIndex[j]] == true) availableWounds.Add(allWounds[i]);
                if(isUnlocked[allWounds[i].objectNeededToTreatIndex[j]] == true) continue;
                else break;
            }
        }
        yield return new WaitForSeconds(0.1f);

        if(haveSpecialNPC) 
        {
            if(tm.GetHour() <= 16) GenerateQueue();
            if(tm.GetHour() > 16 && !specialPatientSpawned) SpawnSpecialNPC(); 
            if(Mathf.Floor(tm.GetHour()) == 17 && specialPatientSpawned)
            {
                Debug.Log("Day " + tm.GetDay().ToString() + " Finish !");
                PlayerPrefs.SetInt("SpecialSpawned", 0);
            }
        }

        if(!haveSpecialNPC)
        {
            if(tm.GetHour() < 17) GenerateQueue();
            if(Mathf.Floor(tm.GetHour()) == 17) Debug.Log("Day " + tm.GetDay().ToString() + " Finish !");
        }
    }

    private void SpawnSpecialNPC()
    {
        int specialNPCIndex;

        PlayerPrefs.SetInt("SpecialSpawned", 1);

        spawnedPatient = Instantiate(specialPatients[currentSpecialID]);
        specialNPCIndex = spawnedPatient.GetComponent<Patients>().GetSpecialID();
        spawnedPatient.transform.position = spawnPos.position;

        spawnedPatient.GetComponent<Patients>().SetWound(availableWounds[specialNPCIndex]);
        spawnedPatient.GetComponent<Patients>().SetWoundSprite(availableWounds[specialNPCIndex]);
        spawnedPatient.GetComponent<Patients>().SetTargetPositions(positions);
    }

    public void UpdateProgress(bool isTreated)
    {
        totalPatientServed++;

        if(isTreated) totalPatientTreated++;
        if(!isTreated) totalPatientFailed++;

        PlayerPrefs.SetInt("Served", totalPatientServed);
        PlayerPrefs.SetInt("Treated", totalPatientTreated);
        PlayerPrefs.SetInt("Failed", totalPatientFailed);
    }

    public Transform[] GetTargetPositions()
    {
        return positions;
    }

    public TextMeshProUGUI GetMoneyText()
    {
        return moneyText;
    }

    public int GetTotalPatientServed()
    {
        return totalPatientServed;
    }

    public int GetTotalPatientTreated()
    {
        return totalPatientTreated;
    }

    public int GetTotalPatientFailed()
    {
        return totalPatientFailed;
    }
}
