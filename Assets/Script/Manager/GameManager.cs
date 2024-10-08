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
    
    ///summary
    ///     Localization
    ///summary

    [Header("For Examine UI")]
    [SerializeField] private LocalizedStringTable examineTextTable;
    [SerializeField] private LocalizeStringEvent stringEvent;
    [Header("For Wrong Procedure")]
    [SerializeField] private LocalizedString wrongProcedureLocalString;

    [Tooltip ("Level Number")]
    [SerializeField] private LocalizedString wrongProcedureLocalStringV2;
    [SerializeField] private int levelIndex;
    private int nextLevelIndex; //getting next lvl idx
    private int levelUnlocked; //getting total lvl unlocked
    private int selectedObjectIndex;
    private int dialogueSkipButtonIndicator;
    private bool isWin;//dowewin this level
    private bool isAnimating;//
    private bool isPauseMenuAnimating;//isPauseMenuAnimating rn
    private bool canSkip;//can we skip this dialogue
    private bool canPause;//can we pause rn
    [SerializeField] private bool isSpecial;
    public GameObject[] neededObjects; //||Ganti Ga ya||

    [Header("For Inspect")]
    public GameObject[] objects;
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
    private ScenesManager sm;
    private int procedureObjectIndex;

    //CONST
    private const string PREFS_DIALOGUESKIP_INDICATOR = "DialogueSkipIndicator";
    private const string PREFS_LEVEL_UNLOCKED ="LevelUnlocked";
    [SerializeField] private int startProcedureIndex;
    public int indexToWinTheGame;
    
    void Start() 
    {
        ///summary
        ///     Get Other Instance
        ///summary
        storyManager = StoryManager.instance;
        dialogueManager = DialogueManager.instance;
        am = AudioManager.instance;
        sm = ScenesManager.instance;

        ///summary
        ///     Getting Levels Data
        ///summary
        dialogueSkipButtonIndicator = PlayerPrefs.GetInt(PREFS_DIALOGUESKIP_INDICATOR, 0);
        levelUnlocked = PlayerPrefs.GetInt(PREFS_LEVEL_UNLOCKED, 1);
        nextLevelIndex = levelIndex + 1;

        ///summary
        ///     For Skipping Dialogue
        ///summary
        if(dialogueSkipButtonIndicator < levelIndex) canSkip = false;
        else if(dialogueSkipButtonIndicator >= levelIndex) canSkip = true;

        procedureObjectIndex = startProcedureIndex;

        // for(int i = 0; i < objects.Length; i++)
        // {
        //     inspectText[i] = examineTextTable.GetTable().GetEntry(i.ToString()).GetLocalizedString();
        // }

        ///summary
        ///     Set Game State
        ///summary
        gameState = State.Playing;
    }

    #region Localization
    //||Ganti Ga ya||
    void OnEnable()
    {
        wrongProcedureLocalString.Arguments = new object[] {wrongProcedureText};
        wrongProcedureLocalString.StringChanged += UpdateText;
    }

    void OnDisable() => wrongProcedureLocalString.StringChanged -= UpdateText;

    private void UpdateText(string value) => wrongProcedureUI.UpdateText(value);
    #endregion

    void Update()
    {
        Pause();
    }

    ///summary
    ///     Pause (Is there a way to make the pause animation outside here?; Input di luar sini kali?) ||Want Change||
    ///summary
    #region Pause
    private void Pause()
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
        yield return new WaitForSeconds(0.6f);
        isPauseMenuAnimating = false;
        if(isPause) gameState = State.Playing;
        else if(!isPause) gameState = State.Pause;
    }
    #endregion
    
    ///summary
    ///     Enable Collider
    ///summary
    public void EnableCollider()
    {
        foreach(GameObject go in objects)
        {
            go.GetComponent<Collider>().enabled = true;
        }
    }
    ///summary
    ///     Inspect UI (Pisah dr sini krn ini UI) ||Want Change||
    ///summary
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

    ///summary
    ///     Show Wrong Procedure UI (Pisah dr sini krn ini UI -> Implementasi penggunaan lokalisasi jg hrs dipikirin) ||Want Change||
    ///summary    
    public void ShowWrongProcedureUI()
    {
        wrongProcedureUI.MoveIn();
        wrongProcedureLocalString.Arguments[0] = objects[procedureObjectIndex].name;
        wrongProcedureLocalString.RefreshString();
        // wrongProcedureUI.UpdateText("Kamu Harus Menggunakan " + objects[procedureObjectIndex].name + " Terlebih Dahulu");
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void ShowWrongProcedureUIV2()
    {
        wrongProcedureUI.MoveIn();
        wrongProcedureUI.UpdateText(wrongProcedureLocalStringV2.GetLocalizedString());
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

    ///summary
    ///     Check win Condition, kalo menang dianimasiiin UI, tp ini blk lg UI dipisah ||Want Change||
    ///summary  
    public void CheckWinCondition()
    {
        if(procedureObjectIndex == indexToWinTheGame && !isWin) StartCoroutine(CompleteAnimation());
    }

    IEnumerator CompleteAnimation()
    {
        gameState = State.Pause;

        am.PlayLevelCompleteSFX();
        missionCompleteUI.OpenUI();
        yield return new WaitForSeconds(1.5f);

        if(dialogueManager != null) dialogueManager.ShowEndDialogue();
        if(storyManager != null) storyManager.ShowEndStory();

        if(dialogueSkipButtonIndicator < levelIndex && nextLevelIndex <= 6) PlayerPrefs.SetInt(PREFS_DIALOGUESKIP_INDICATOR, levelIndex);
        if(levelUnlocked < nextLevelIndex && nextLevelIndex <= 5) PlayerPrefs.SetInt(PREFS_LEVEL_UNLOCKED, nextLevelIndex);

        if(!isSpecial) sm.GoToTargetScene("PatientReception");

        isWin = true;
    }

    ///summary
    ///     Animating value nya item ||Ganti Ga ya||
    ///summary 

    public void ChangeIsAnimatingValue(bool value) => isAnimating = value;
    
    public void ChangeGameState(bool isPause)
    {
        if(isPause) gameState = State.Pause;
        else if(!isPause) gameState = State.Playing;
    }

    ///summary
    ///     Animating Pause ||Ganti Ga ya||
    ///summary 
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
