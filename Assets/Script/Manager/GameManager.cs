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

    [SerializeField] private int levelIndex;
    private int selectedIndex;
    private bool isWin;
    private bool isAnimating;
    private bool isPauseMenuAnimating;
    public GameObject[] objects;

    [Header("For Inspect")]
    [SerializeField] private InspectUI inspectUI;
    [SerializeField] private string[] inspectText;
    private bool isInInspectMode;
    [Header("For Wrong Procedure")]
    [SerializeField] private WrongProcedureUI wrongProcedureUI;
    [Header("Pause Menu")]
    [SerializeField] private PauseMenuUI pauseMenuUI;
    private StoryManager storyManager;
    private int procedureObjectIndex;

    void Start() 
    {
        storyManager = StoryManager.instance;

        gameState = State.Playing;
    }
    
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
        wrongProcedureUI.MoveIn();
        wrongProcedureUI.UpdateText("Kamu Harus Menggunakan " + objects[procedureObjectIndex].name + " Terlebih Dahulu");
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void ShowWrongProcedureUIForProceduralObjects(string sentence)
    {
        wrongProcedureUI.MoveIn();
        wrongProcedureUI.UpdateText(sentence);
        StartCoroutine(WaitWrongProcedureUI());
    }

    IEnumerator WaitWrongProcedureUI()
    {
        yield return new WaitForSeconds(2.0f);
        wrongProcedureUI.MoveOut();
    }

    public void AddProcedureObjectIndex() => procedureObjectIndex += 1;

    public void CheckWinCondition()
    {
        if(procedureObjectIndex == objects.Length && !isWin) 
        {
            Debug.Log("Win");
            if(storyManager != null) storyManager.ShowEndStory();
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

    public int GetProcedureIndex()
    {
        return procedureObjectIndex;
    }

    public int GetLevelIndex()
    {
        return levelIndex;
    }

    public bool GetPauseMenuIsAnimating()
    {
        return isPauseMenuAnimating;
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
