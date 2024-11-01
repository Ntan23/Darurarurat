using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaTimeInput : MonoBehaviour
{
    private bool canInput;  
    private SpriteRenderer firstAidBoxRenderer;

    void Start() 
    {
        firstAidBoxRenderer = GetComponent<SpriteRenderer>();

        //PlayerPrefs.SetInt("IsTeaTime", 1);
    }

    void OnMouseEnter() 
    {
        if(canInput) firstAidBoxRenderer.color = Color.yellow;
    }

    void OnMouseExit() 
    {
        if(canInput) firstAidBoxRenderer.color = Color.white;
    }
    
    void OnMouseDown()
    {
        //Debug.Log("Klik");
        if(canInput) 
        {
            firstAidBoxRenderer.color = Color.white;

            GameObject go = GameObject.FindGameObjectWithTag("Patient");

            Destroy(go);

            PlayerPrefs.SetInt("IsTeaTime", 0);
            ScenesManager.instance.GoToTargetScene("UpgradeScene");
        }
    }

    public void SetCanInput(bool value) => canInput = value;
}
