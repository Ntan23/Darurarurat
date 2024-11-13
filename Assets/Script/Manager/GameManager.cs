using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using System.Collections.Generic;

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
        Pause, Playing, Cutscene
    }

    private State gameState;
    
    ///summary
    ///     Localization
    ///summary
    [Header("For Money")]
    [SerializeField] private PatientWoundSO woundSO;
    [SerializeField] private TextMeshProUGUI[] moneyText;
    private float currentPrice;
    private int error;

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
    [Header("Mission UI")]
    [SerializeField] private MissionUI missionCompleteUI;
    [SerializeField] private MissionUI missionFailUI;

    [Header("Dialogue Title That Want to be Played")]
    [Header("Ignore if Not calling dialogue")]
    [SerializeField] private ChatDialoguesTitle _introTitle;
    [SerializeField] private List<ChatDialoguesTitle> _midTitles;
    public List<ChatDialoguesTitle> GetMidTitles{get{return _midTitles;}}
    [SerializeField] private ChatDialoguesTitle _endTitle;
    [Space(5)]
    private StoryManager storyManager;
    private Chat_DialogueManager _chatDialogueManager;
    private DialogueManager dialogueManager;
    private AudioManager am;
    private ScenesManager sm;
    private TimeManager tm;
    private MoneyManager mm;
    private int procedureObjectIndex;
    [SerializeField] private int startProcedureIndex;
    public int indexToWinTheGame;
    private bool canError = true;
    [SerializeField] private bool isSpecial;
    //CONST
    private const string PREFS_DIALOGUESKIP_INDICATOR = "DialogueSkipIndicator";
    private const string PREFS_LEVEL_UNLOCKED ="LevelUnlocked";

    
    void Start() 
    {
        ///summary
        ///     Get Other Instance
        ///summary
        _chatDialogueManager = Chat_DialogueManager.Instance;
        storyManager = StoryManager.instance;
        dialogueManager = DialogueManager.instance;
        am = AudioManager.instance;
        sm = ScenesManager.instance;
        mm = MoneyManager.instance;
        tm  = TimeManager.instance;

        currentPrice = woundSO.treatPrice;
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
        if(_chatDialogueManager != null)
        {
            gameState = State.Cutscene;
            _chatDialogueManager.PlayDialogueScene(_introTitle);
        }
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
        if(Input.GetKeyDown(KeyCode.Escape) && !isPauseMenuAnimating && canPause && gameState != State.Cutscene)
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
        CheckNApplyErrorCount();
        wrongProcedureUI.MoveIn();
        wrongProcedureLocalString.Arguments[0] = objects[procedureObjectIndex].name;
        wrongProcedureLocalString.RefreshString();
        // wrongProcedureUI.UpdateText("Kamu Harus Menggunakan " + objects[procedureObjectIndex].name + " Terlebih Dahulu");
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void ShowWrongProcedureUIV2()
    {
        CheckNApplyErrorCount();
        wrongProcedureUI.MoveIn();
        wrongProcedureUI.UpdateText(wrongProcedureLocalStringV2.GetLocalizedString());
        StartCoroutine(WaitWrongProcedureUI());
    }

    public void ShowWrongProcedureUIForProceduralObjects(string sentence)
    {
        CheckNApplyErrorCount();
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

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Served", 0);
        PlayerPrefs.SetInt("Treated",0);
        PlayerPrefs.SetInt("Failed", 0);
    }
    ///summary
    ///     Check win Condition, kalo menang dianimasiiin UI, tp ini blk lg UI dipisah ||Want Change||
    ///summary  
    public void CheckWinCondition()
    {
        if(procedureObjectIndex == indexToWinTheGame && !isWin) StartCoroutine(CompleteAnimation());
    }

    private void CheckNApplyErrorCount()
    {
        if(canError)
        {
            StartCoroutine(DelayErrorCount());
            error++;
            currentPrice--;

            if(error >= 3) StartCoroutine(LossAnimation());
        }
    }

    IEnumerator DelayErrorCount()
    {
        canError = false;
        yield return new WaitForSeconds(0.1f);
        canError = true;
    }

    IEnumerator CompleteAnimation()
    {
        gameState = State.Pause;

        am.PlayLevelCompleteSFX();
        missionCompleteUI.OpenUI();
        mm.AddMoney(currentPrice);
        moneyText[0].text = currentPrice.ToString("0.00") + " Rp";

        GameObject.FindGameObjectWithTag("Patient").GetComponent<Patients>().SetTreatValue(true);

        int totalPatientServed, totalPatientTreated;
        
        totalPatientServed = PlayerPrefs.GetInt("Served", 0);
        totalPatientTreated = PlayerPrefs.GetInt("Treated", 0);

        totalPatientServed++;
        totalPatientTreated++;

        Debug.Log("S : " + totalPatientServed);
        Debug.Log("T : " + totalPatientTreated);

        PlayerPrefs.SetInt("Served", totalPatientServed);
        PlayerPrefs.SetInt("Treated", totalPatientTreated);

        GameObject go = GameObject.FindGameObjectWithTag("Patient");

        if(go != null) Destroy(go);
        
        
        yield return new WaitForSeconds(1.5f);

        if(dialogueManager != null) dialogueManager.ShowEndDialogue();
        if(storyManager != null) storyManager.ShowEndStory();
        if(_chatDialogueManager != null)
        {
            _chatDialogueManager.PlayDialogueScene(_endTitle);
            gameState = State.Cutscene;
        }
        // taruh sini (untuk panggil end story / dialog)
        
        // if(dialogueSkipButtonIndicator < levelIndex && nextLevelIndex <= 6) PlayerPrefs.SetInt(PREFS_DIALOGUESKIP_INDICATOR, levelIndex);
        // if(levelUnlocked < nextLevelIndex && nextLevelIndex <= 5) PlayerPrefs.SetInt(PREFS_LEVEL_UNLOCKED, nextLevelIndex);

        if(!isSpecial) sm.GoToTargetScene("PatientReception");
        if(tm.GetHour() >= 17) 
        {
            mm.SaveMoney();
            if(!isSpecial && tm.GetDay() != 1) sm.GoToTargetScene("TeaTime");
        }

        isWin = true;
    }

    IEnumerator LossAnimation()
    {
        gameState = State.Pause;

        //am.PlayLevelCompleteSFX();
        //missionCompleteUI.OpenUI();
        mm.DecreaseMoney(woundSO.failPrice);
        moneyText[1].text = woundSO.failPrice.ToString("0.00") + " Rp";
        missionFailUI.OpenUI();

        GameObject.FindGameObjectWithTag("Patient").GetComponent<Patients>().SetTreatValue(false);

        int totalPatientServed, totalPatientFailed;
            
        totalPatientServed = PlayerPrefs.GetInt("Served", 0);
        totalPatientFailed = PlayerPrefs.GetInt("Failed", 0);

        totalPatientServed++;
        totalPatientFailed++;

        Debug.Log("F : " + totalPatientFailed);
        Debug.Log("S : " + totalPatientServed);

        PlayerPrefs.SetInt("Served", totalPatientServed);
        PlayerPrefs.SetInt("Failed", totalPatientFailed);

        GameObject go = GameObject.FindGameObjectWithTag("Patient");

        if(go != null) Destroy(go);

        yield return new WaitForSeconds(1.5f);

        if(dialogueManager != null) dialogueManager.ShowEndDialogue();
        if(storyManager != null) storyManager.ShowEndStory();
        if(_chatDialogueManager != null)
        {
            _chatDialogueManager.PlayDialogueScene(_endTitle);
            gameState = State.Cutscene;
        }
        // taruh sini (untuk panggil end story / dialog)

        // if(dialogueSkipButtonIndicator < levelIndex && nextLevelIndex <= 6) PlayerPrefs.SetInt(PREFS_DIALOGUESKIP_INDICATOR, levelIndex);
        // if(levelUnlocked < nextLevelIndex && nextLevelIndex <= 5) PlayerPrefs.SetInt(PREFS_LEVEL_UNLOCKED, nextLevelIndex);

        if(!isSpecial) sm.GoToTargetScene("PatientReception");
        if(tm.GetHour() >= 17)
        {
            mm.SaveMoney();
            if(!isSpecial && tm.GetDay() != 1) sm.GoToTargetScene("TeaTime");
        } 
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
    public bool IsCutscene()
    {
        return gameState == State.Cutscene;
    }
    public void ChangeCutsceneStatus(bool IsCutscene)
    {
        if(IsCutscene && gameState == State.Pause)return;

        if(IsCutscene) gameState = State.Cutscene;
        else gameState = State.Playing;
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
