using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CONTROL PER COMIC PART
/// </summary>
public class ComicPageController : MonoBehaviour, INeedButtonInput
{
    private ComicManager comicManager;

    [Header("The SODialogues")]
    [SerializeField] private SOComicDialogues _comicPagesAllDialogues_SODIALOGUES;
    private ComicDialoguesTitle _comicTitle;
    private string _comicTitleInString;
    private Dialogue_Line _currDialogue;
    private int _currDialogueIdx;

    [Header("All Container")]
    [SerializeField] private Comic_DialogueLine _dialogueLineContainer;
    [SerializeField] private GameObject _comicUIParent, _comicBG, _comicFrontBG;
    [SerializeField] private RectTransform _dialogueBoxParent, _comicImageParent;
    [SerializeField] private List<ComicPage> _comicPages;
    private ComicPage _currPage;

    [Header("Move Point")]
    [SerializeField] private Vector3 _startingPoint;
    [SerializeField] private float _nextDialogueXPoint, _nextComicXPoint;
    [SerializeField] private float _dialogueBoxParent_MoveDuration, _comicImageParent_MoveDuration;

    [Header("GetComponent")]
    [SerializeField] private FadeFrontUI _fade;
    [SerializeField] private float _fadeInDuration = 0.2f, _fadeOutDuration = 0.5f, _fadeComicBGDuration = 0.2f, _fadeDialogBoxDuration = 0.2f;
    private IEnumerator _comicTime, _scrollRectSave, xxx;
    private bool _finished;
    private bool _canMovePage;
    [Header("NextButton")]
    [SerializeField]private Button _nextButtonContainer;
    private bool _isNextButtonClicked;

    #region  GETTER SETTER VARIABLE
    public bool Finished { get { return _finished; } }
    public ComicDialoguesTitle ComicTitle {get {return _comicTitle;}}
    public String ComicTitleInString {get {return _comicTitleInString;}}
    #endregion
    

    private void Awake() 
    {
        GetAllTextContainersScrollsFromComicPagesBox();
        HideAllDialogueBox();

        _comicTitle = _comicPagesAllDialogues_SODIALOGUES.dialoguesTitle;
        _comicTitleInString = _comicTitle.ToString();

        
    }

    private void Start() 
    {
        comicManager = ComicManager.Instance;
        _dialogueLineContainer.OnDialogueDoneBeforeInput += _dialogueLine_OnDialogueDoneBeforeInput;
        
    }



    public void GetAllTextContainersScrollsFromComicPagesBox()
    {
        for(int i=0 ; i < _comicPages.Count;i++)
        {
            ComicPage currPage = _comicPages[i];
            currPage.DialogueTextContainersPage = new List<TMP_Text>();
            currPage.DialogueBoxScrollRect = new List<ScrollRect>();
            for(int j=0; j < currPage.DialogueBoxsPage.Count;j++)
            {
                TMP_Text newContainer = currPage.DialogueBoxsPage[j].GetComponentInChildren<TMP_Text>();
                if(newContainer != null)currPage.DialogueTextContainersPage.Add(newContainer);

                ScrollRect newScrollRect = currPage.DialogueBoxsPage[j].GetComponentInChildren<ScrollRect>();
                if(newScrollRect != null)currPage.DialogueBoxScrollRect.Add(newScrollRect);
            }
        }
        
    }

    public void ShowComic()
    {
        if(_fade)_fade.Fade_WithFunction(PreparingComic, _fadeInDuration, 1);
        else PreparingComic();
    }

    public void HideComic()
    {
        StopComicTime();

        _finished = false;
        HideAllParent();

        comicManager.OnComicFinished?.Invoke(ComicTitle);
    }

    //Prepare Comic before showing
    private void PreparingComic()
    {
        // Debug.Log("Preparing Comic");
        StopComicTime();
        _finished = false;

        HideAllDialogueBox();
        _dialogueBoxParent.localPosition = _startingPoint;
        _comicImageParent.localPosition = _startingPoint;

        _currDialogueIdx = 0;
        _currDialogue = null;
        _currPage = null;
        _scrollRectSave = null;
        
        _dialogueBoxParent.gameObject.SetActive(true);
        _comicImageParent.gameObject.SetActive(true);
        StartComicTime();
        
    }
    private void HideAllParent()
    {
        _comicUIParent.SetActive(false);
        _comicBG.SetActive(false);
        if(_comicFrontBG != null)_comicFrontBG.SetActive(false);
        _nextButtonContainer.gameObject.SetActive(false);
        _nextButtonContainer.onClick.RemoveAllListeners();

        if(_dialogueBoxParent.gameObject.activeSelf)_dialogueBoxParent.gameObject.SetActive(false);
        if(_comicImageParent.gameObject.activeSelf)_comicImageParent.gameObject.SetActive(false);
    }
    private void HideAllDialogueBox()
    {
        HideAllParent();
        foreach(ComicPage page in _comicPages)
        {
            page.HideAllDialogueBox();
            page.ClearAllTextContainers();
            page.SetTransparentImage_Animated(null, 0.5f, _fadeComicBGDuration);
        }
    }
    private void StartComicTime()
    {
        _comicTime = ComicTime();
        StartCoroutine(_comicTime);
    }
    private void StopComicTime()
    {
        if(_comicTime == null)return;

        StopCoroutine(_comicTime);
        if(_scrollRectSave != null)
        {
            StopCoroutine(_scrollRectSave);
            _scrollRectSave = null;
        }
        _dialogueLineContainer.StopDialogue();
        
    }

    public IEnumerator ComicTime()
    {
        _comicUIParent.SetActive(true);
        _comicBG.SetActive(true);
        if(_comicFrontBG != null)_comicFrontBG.SetActive(true);
        for(int i=0; i < _comicPages.Count; i++)
        {
            _currPage = _comicPages[i];
            if(i==0)
            {
                _currPage.SetTransparentImage_Animated(null, 1f, _fadeComicBGDuration);
                if(_fade)_fade.FadeNormal(_fadeOutDuration, 0);
            }
            _currPage.PlayStartingSFX();
            _nextButtonContainer.onClick.AddListener(NextButtonClicked);
            _nextButtonContainer.gameObject.SetActive(true);
            
            yield return new WaitUntil(()=> IsInputTrue());
            _nextButtonContainer.onClick.RemoveListener(NextButtonClicked);
            _nextButtonContainer.onClick.AddListener(_dialogueLineContainer.NextButtonClicked);
            while(!_currPage.IsFinished)
            {
                _currPage.PlayStartingDialogueSFX();
                _currDialogue = _comicPagesAllDialogues_SODIALOGUES.dialogue_Lines[_currDialogueIdx];
                _currPage.ShowDialogueBox(DoDialogue, _fadeDialogBoxDuration);
                _currDialogueIdx++;
                yield return new WaitUntil(()=> 
                _dialogueLineContainer.Finished);
                // StopCoroutine(_scrollRectSave); -> pindah ke event biar stop pas, emg done
                
                _dialogueLineContainer.ChangeFinished_false();
                _currPage.PlayEndingDialogueSFX();
                _scrollRectSave = null;
            }
            _nextButtonContainer.onClick.RemoveListener(_dialogueLineContainer.NextButtonClicked);
            _nextButtonContainer.onClick.AddListener(NextButtonClicked);
            if(i+1 == _comicPages.Count)NextButtonClicked();
            yield return new WaitUntil(()=> IsInputTrue());
            
            if(i != _comicPages.Count - 1)
            {
                _nextButtonContainer.onClick.RemoveListener(NextButtonClicked);
                _nextButtonContainer.gameObject.SetActive(false);
            }

            if( i + 1 != _comicPages.Count)
            {
                _currPage.SetTransparentImage_Animated(null, 0.2f, _fadeComicBGDuration);
                _comicPages[i+1].SetTransparentImage_Animated(MoveNext, 1f, _fadeComicBGDuration);
            }
            else _canMovePage = true;
            yield return new WaitUntil(()=> _canMovePage);
            _canMovePage = false;
            
        }
        // Debug.Log("ALOOO");
        yield return new WaitUntil(()=> IsInputTrue());
        _nextButtonContainer.onClick.RemoveListener(NextButtonClicked);

        _finished = true;
        // Debug.Log("??????");
        if(_fade)_fade.Fade_WithFunction(HideComic, _fadeInDuration, 1);

    }
    public void DoDialogue()
    {
        if(_currDialogue != null)
        {
            // Debug.Log("Halo");
            _dialogueLineContainer.GetTextContainer(_currPage.GiveDialogueText());

            _scrollRectSave = _currPage.ScrollRectChecker();
            StartCoroutine(_scrollRectSave);

            _dialogueLineContainer.SetDialogue(_currDialogue);
        }
    }
    private bool IsInputTrue()
    {
        if(Input.GetKeyDown(KeyCode.Space) || _isNextButtonClicked)
        {
            if(_isNextButtonClicked)_isNextButtonClicked = false;
            return true;
        }
        return false;
    }
    public void MoveNext()
    {
        float dialoguenextXPoint = _dialogueBoxParent.localPosition.x + _nextDialogueXPoint;
        float comicnextXPoint = _comicImageParent.localPosition.x + _nextComicXPoint;
        LeanTween.moveLocalX(_dialogueBoxParent.gameObject, dialoguenextXPoint, _dialogueBoxParent_MoveDuration);
        LeanTween.moveLocalX(_comicImageParent.gameObject, comicnextXPoint, _dialogueBoxParent_MoveDuration).setOnComplete(
            ()=>
            {
                _canMovePage = true;
            }
        );
    }

    public void NextButtonClicked() => _isNextButtonClicked = true;
    private void _dialogueLine_OnDialogueDoneBeforeInput()
    {
        // Debug.Log("Duarr" + _scrollRectSave != null?"Yey":"Booo");
        if(_scrollRectSave != null)StopCoroutine(_scrollRectSave);
        _scrollRectSave = null;

        if(_currPage.IsFinished)
        {
            _dialogueLineContainer.NextButtonClicked();
        }
        // _currPage.NormalizeScrollRectPosFinal();
    }
}
