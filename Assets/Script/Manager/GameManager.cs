using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion
    
    private enum State 
    {
        Pause, Playing
    }

    private State gameState;

    private int selectedIndex;
    private bool isWin;
    private bool isAnimating;
    private bool isPauseMenuAnimating;
    private bool isNotUsingKey;
    public GameObject[] objects;

    [Header("For Inspect")]
    [SerializeField] private InspectUI inspectUI;
    [SerializeField] private string[] inspectText;
    private bool isInInspectMode;
    [Header("For Wrong Procedure")]
    [SerializeField] private WrongProcedureUI wrongProcedureUI;
    [Header("Pause Menu")]
    [SerializeField] private PauseMenuUI pauseMenuUI;

    private int procedureObjectIndex;

    void Start() => gameState = State.Playing;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuAnimating)
        {
            if(gameState == State.Playing)
            {
                pauseMenuUI.OpenPauseMenu(false);
                StartCoroutine(WaitForPauseMenuAnimation(false));
            }

            if(gameState == State.Pause)
            {
                pauseMenuUI.ClosePauseMenu(false);
                StartCoroutine(WaitForPauseMenuAnimation(true));
            }
        }
    }

    IEnumerator WaitForPauseMenuAnimation(bool isPause)
    {
        yield return new WaitForSeconds(1.0f);
        isPauseMenuAnimating = false;
        if(isPause) gameState = State.Playing;
        else if(!isPause) gameState = State.Pause;
    }

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
    
    public void ChangeGameState(bool isPause)
    {
        if(isPause) gameState = State.Pause;
        else if(!isPause) gameState = State.Playing;
    }

    public void ChangePauseMenuIsAnimatingValue(bool value) => isPauseMenuAnimating = value;

    public bool GetPauseMenuIsAnimating()
    {
        return isPauseMenuAnimating;
    }

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

    public bool IsPlaying()
    {
        return gameState == State.Playing;
    }

    public bool IsPausing()
    {
        return isPauseMenuAnimating;
    }
}   
