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
        if(canInput) ScenesManager.instance.GoToNextScene();
    }

    public void SetCanInput(bool value) => canInput = value;
}
