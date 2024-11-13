using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsUIV2 : MonoBehaviour
{
    public enum State {
        Reception, TeaTime
    }
    [SerializeField] private State state;
    [SerializeField] private GameObject[] texts;
    private int currentIndex;
    [SerializeField] private TeaTimeInput teaTimeInput;

    void Start()
    {
        if(PlayerPrefs.GetInt("IsFirstimePlaying", 0) == 1 && PlayerPrefs.GetInt("TipsReception", 0) == 0 && state == State.Reception) this.gameObject.SetActive(true); 
    }

    public void Check()
    {
        if(state == State.Reception)
        {
            if(PlayerPrefs.GetInt("TipsReception", 0) == 0)  this.gameObject.SetActive(true);
            if(PlayerPrefs.GetInt("TipsReception", 0) == 1)  
            {
                this.gameObject.SetActive(false);
                PatientsQueueManager.instance.CheckInjuriesAndSpawnPatient();
            }
        }

        if(state == State.TeaTime)
        {
            if(PlayerPrefs.GetInt("TipsTeaTime", 0) == 0) this.gameObject.SetActive(true);
            if(PlayerPrefs.GetInt("TipsTeaTime", 0) == 1) 
            {
                this.gameObject.SetActive(false);
                teaTimeInput.SetCanInput(true);
            } 
        }
    }

    public void Close()
    {
        this.gameObject.SetActive(false);

        if(state == State.Reception)
        {
            PlayerPrefs.SetInt("TipsReception", 1);
            PatientsQueueManager.instance.CheckInjuriesAndSpawnPatient();
        }

        if(state == State.TeaTime)
        {
            PlayerPrefs.SetInt("TipsTeaTime" , 1);
            teaTimeInput.SetCanInput(true);
        }
    }

    public void Next()
    {
        if(currentIndex == texts.Length - 1) Close();
        
        if(currentIndex < texts.Length - 1)
        {
            currentIndex++;

            texts[currentIndex - 1].SetActive(false);
            texts[currentIndex].SetActive(true);
        }

    }

    public void Previous()
    {
        if(currentIndex > 0)
        {
            currentIndex--;

            texts[currentIndex + 1].SetActive(false);
            texts[currentIndex].SetActive(true);
        }
    }
}
