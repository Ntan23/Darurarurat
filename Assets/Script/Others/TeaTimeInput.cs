using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTimeInput : MonoBehaviour
{
    private bool canInput;  

    void Start() => PlayerPrefs.SetInt("IsTeaTime", 1);

    void OnMouseDown()
    {
        //Debug.Log("Klik");
        if(canInput) 
        {
            GameObject go = GameObject.FindGameObjectWithTag("Patient");

            Destroy(go);

            PlayerPrefs.SetInt("IsTeaTime", 0);
            ScenesManager.instance.GoToTargetScene("UpgradeScene");
        }
    }

    public void SetCanInput(bool value) => canInput = value;
}
