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
    private bool isWin;
    private bool isAnimating;
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
        //inspectUI.gameObject.SetActive(true);
        //inspectUI.FadeIn(); 
        inspectUI.MoveIn();
        inspectUI.ChangeUIText(inspectText[index]);
        isInInspectMode = true;
        selectedIndex = index;
    }

    public void CloseInspect()
    {
        // inspectUI.gameObject.SetActive(false);
        //inspectUI.FadeOut();
        inspectUI.MoveOut();
        isInInspectMode = false;
        objects[selectedIndex].GetComponent<ObjectControl>().CloseInspect();
    }
    
    public void ShowWrongProcedureUI()
    {
        wrongProcedureUI.FadeIn();
        wrongProcedureUI.UpdateText("Kamu Harus Menggunakan " + objects[procedureObjectIndex].name + " Terlebih Dahulu");
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void ShowWrongProcedureUIForProceduralObjects(string sentence)
    {
        wrongProcedureUI.FadeIn();
        wrongProcedureUI.UpdateText(sentence);
        StartCoroutine(WaitWrongProcedureUI());
    }

    IEnumerator WaitWrongProcedureUI()
    {
        yield return new WaitForSeconds(2.0f);
        wrongProcedureUI.FadeOut();
    }

    public void AddProcedureObjectIndex() => procedureObjectIndex += 1;

    public void CheckWinCondition()
    {
        if(procedureObjectIndex == objects.Length && !isWin) 
        {
            Debug.Log("You Win");
            isWin = true;
        }
    }

    public void ChangeIsAnimatingValue(bool value) => isAnimating = value;
    
    public int GetProcedureIndex()
    {
        return procedureObjectIndex;
    }

    public bool GetIsInInspectMode()
    {
        return isInInspectMode;
    }

    public bool GetIsAnimating()
    {
        return isAnimating;
    }
}   
