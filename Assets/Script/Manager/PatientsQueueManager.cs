using System.Collections;
using System.Collections.Generic;
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
    private GameObject spawnedPatient;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private Transform[] positions;

    void Start()
    {
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
        GenerateQueue();
    }

    public Transform[] GetTargetPositions()
    {
        return positions;
    }
}
