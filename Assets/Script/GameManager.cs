using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }

    private int selectedIndex;
    public GameObject[] objects;

    [Header("For Inspect")]
    [SerializeField] private InspectUI inspectUI;
    [SerializeField] private string[] inspectText;
    private bool isInInspectMode;

    private int procedureObjectIndex;

    public void EnableCollider()
    {
        foreach(GameObject go in objects)
        {
            go.GetComponent<Collider>().enabled = true;
        }
    }

    public void OpenInspectUI(int index)
    {
        //inspectUI.gameObject.SetActive(true);  
        LeanTween.value(inspectUI.gameObject, UpdateInspectUIAlpha, 0.0f, 1.0f, 0.5f); 
        inspectUI.ChangeUIText(inspectText[index]);
        isInInspectMode = true;
        selectedIndex = index;
    }

    public void CloseInspect()
    {
        //inspectUI.gameObject.SetActive(false);
        LeanTween.value(inspectUI.gameObject, UpdateInspectUIAlpha, 1.0f, 0.0f, 0.5f); 
        isInInspectMode = false;
        objects[selectedIndex].GetComponent<ObjectControl>().CloseInspect();
    }

    void UpdateInspectUIAlpha(float alpha) => inspectUI.gameObject.GetComponent<CanvasGroup>().alpha = alpha;
    
    public void AddProcedureObjectIndex() => procedureObjectIndex += 1;

    public int GetProcedureIndex()
    {
        return procedureObjectIndex;
    }

    public bool GetIsInInspectMode()
    {
        return isInInspectMode;
    }
}
