using System.Collections;
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
    [Header("For Wrong Procedure")]
    [SerializeField] private WrongProcedureUI wrongProcedureUI;

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
        inspectUI.FadeIn(); 
        inspectUI.ChangeUIText(inspectText[index]);
        isInInspectMode = true;
        selectedIndex = index;
    }

    public void CloseInspect()
    {
        inspectUI.FadeOut();
        isInInspectMode = false;
        objects[selectedIndex].GetComponent<ObjectControl>().CloseInspect();
    }
    
    public void ShowWrongProcedureUI()
    {
        wrongProcedureUI.FadeIn();
        wrongProcedureUI.UpdateText(objects[procedureObjectIndex].name);
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void AddProcedureObjectIndex() => procedureObjectIndex += 1;

    public int GetProcedureIndex()
    {
        return procedureObjectIndex;
    }

    public bool GetIsInInspectMode()
    {
        return isInInspectMode;
    }

    IEnumerator WaitWrongProcedureUI()
    {
        yield return new WaitForSeconds(2.0f);
        wrongProcedureUI.FadeOut();
    }
}
