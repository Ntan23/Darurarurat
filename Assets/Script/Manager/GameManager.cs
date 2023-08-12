using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

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

    [Header("For Examine UI")]
    [SerializeField] private LocalizedStringTable examineTextTable;
    [SerializeField] private LocalizeStringEvent stringEvent;
    [Header("For Wrong Procedure")]
    [SerializeField] private LocalizedString wrongProcedureLocalString;
    [SerializeField] private int levelIndex;
    private int nextLevelIndex;
    private int levelUnlocked;
    private int selectedObjectIndex;
    private int dialogueSkipButtonIndicator;
    private bool isWin;
    private bool isAnimating;
    private bool isPauseMenuAnimating;
    private bool canSkip;
    private bool canPause;
    public GameObject[] objects;

    [Header("For Inspect")]
    [SerializeField] private InspectUI inspectUI;
    [SerializeField] private string[] inspectText;
    private bool isInInspectMode;
    [Header("For Wrong Procedure")]
    [SerializeField] private WrongProcedureUI wrongProcedureUI;
    private string wrongProcedureText;
    [Header("Pause Menu")]
    [SerializeField] private PauseMenuUI pauseMenuUI;
    [Header("Complete UI")]
    [SerializeField] private MissionCompleteUI missionCompleteUI;
    private StoryManager storyManager;
    private DialogueManager dialogueManager;
    private AudioManager am;
    private int procedureObjectIndex;

    
    void Start() 
    {
        storyManager = StoryManager.instance;
        dialogueManager = DialogueManager.instance;
        am = AudioManager.instance;

        dialogueSkipButtonIndicator = PlayerPrefs.GetInt("DialogueSkipIndicator", 0);
        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);
        nextLevelIndex = levelIndex + 1;

        if(dialogueSkipButtonIndicator < levelIndex) canSkip = false;
        else if(dialogueSkipButtonIndicator >= levelIndex) canSkip = true;

        // for(int i = 0; i < objects.Length; i++)
        // {
        //     inspectText[i] = examineTextTable.GetTable().GetEntry(i.ToString()).GetLocalizedString();
        // }

        gameState = State.Playing;
    }
    
    void OnEnable()
    {
        wrongProcedureLocalString.Arguments = new object[] {wrongProcedureText};
        wrongProcedureLocalString.StringChanged += UpdateText;
    }

    void OnDisable() => wrongProcedureLocalString.StringChanged -= UpdateText;

    private void UpdateText(string value) => wrongProcedureUI.UpdateText(value);

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuAnimating && canPause)
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
        inspectUI.MoveIn();
        stringEvent.StringReference.SetReference(examineTextTable.ToString(), index.ToString());
        inspectUI.ChangeUIText(examineTextTable.GetTable().GetEntry(index.ToString()).GetLocalizedString());
        isInInspectMode = true;
        selectedObjectIndex = index;
    }

    public void CloseInspect()
    {
        inspectUI.MoveOut();
        isInInspectMode = false;
        objects[selectedObjectIndex].GetComponent<ObjectControl>().CloseInspect();
    }
    
    public void ShowWrongProcedureUI()
    {
        wrongProcedureUI.MoveIn();
        wrongProcedureLocalString.Arguments[0] = objects[procedureObjectIndex].name;
        wrongProcedureLocalString.RefreshString();
        // wrongProcedureUI.UpdateText("Kamu Harus Menggunakan " + objects[procedureObjectIndex].name + " Terlebih Dahulu");
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
        if(procedureObjectIndex == objects.Length && !isWin) StartCoroutine(CompleteAnimation());
    }

    IEnumerator CompleteAnimation()
    {
        gameState = State.Pause;

        am.PlayLevelCompleteSFX();
        missionCompleteUI.OpenUI();
        yield return new WaitForSeconds(1.5f);

        if(dialogueManager != null) dialogueManager.ShowEndDialogue();
        if(storyManager != null) storyManager.ShowEndStory();

        if(dialogueSkipButtonIndicator < levelIndex && nextLevelIndex <= 5) PlayerPrefs.SetInt("DialogueSkipIndicator", levelIndex);
        if(levelUnlocked < nextLevelIndex && nextLevelIndex <= 4) PlayerPrefs.SetInt("LevelUnlocked", nextLevelIndex);

        isWin = true;
    }

    public void ChangeIsAnimatingValue(bool value) => isAnimating = value;
    
    public void ChangeGameState(bool isPause)
    {
        if(isPause) gameState = State.Pause;
        else if(!isPause) gameState = State.Playing;
    }

    public void ChangePauseMenuIsAnimatingValue(bool value) => isPauseMenuAnimating = value;

    public void ChangeCanPauseValue(bool value) => canPause = value;
    
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

    public bool CanSkip()
    {
        return canSkip;
    }
}   
