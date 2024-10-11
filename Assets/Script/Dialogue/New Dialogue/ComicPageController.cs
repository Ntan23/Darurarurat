using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;


public enum ComicEffectAnimEffect
{
    MoveLeft, MoveRight, MoveUp, MoveDown, ShowingABit
}
[Serializable]
public class ComicPage
{
    [SerializeField] private Image _comicImagePage;
    private int _currDialogueBoxIdx;
    [SerializeField] private List<GameObject> _dialogueBoxsPage;
    [SerializeField] private List<TMP_Text> _dialogTextContainersPage;
    private bool _isFinished;
    public int TotalBox { get { return _dialogueBoxsPage.Count;}}
    public bool IsFInished {get { return _isFinished; } }

    public void ClearAllTextContainers()
    {
        foreach(TMP_Text dialogueText in _dialogTextContainersPage)
        {
            dialogueText.text = "";
        }
    }
    public void HideAllDialogueBox()
    {
        foreach(GameObject dialogueBox in _dialogueBoxsPage)
        {
            dialogueBox.SetActive(false);
            LeanTween.alpha(dialogueBox, 0f, 0);
        }
        _currDialogueBoxIdx = 0;
        _isFinished = false;
    }
    // public void SetTransparentImage(float transparentPoint)
    // {
    //     Color newColor = _comicImagePage.color;
    //     newColor.a = transparentPoint;
    //     _comicImagePage.color = newColor;
    // }

    public void SetTransparentImage_Animated(Action functionAfterFade, float toAlpha, float duration)
    {
        LeanTween.alpha(_comicImagePage.rectTransform, toAlpha, duration).setOnComplete(functionAfterFade);
    }
    public void ShowDialogueBox(Action functionAfterFade, float duration)
    {
        if(_currDialogueBoxIdx == _dialogueBoxsPage.Count)
        {
            return;
        }
        _dialogueBoxsPage[_currDialogueBoxIdx].SetActive(true);
        LeanTween.alpha(_dialogueBoxsPage[_currDialogueBoxIdx], 1f, duration).setOnComplete(functionAfterFade);
        _currDialogueBoxIdx++;
        if(_currDialogueBoxIdx == _dialogueBoxsPage.Count)
        {
            _isFinished = true;
        }
    }
    public TMP_Text GiveDialogueText()
    {
        if(_currDialogueBoxIdx - 1 == _dialogueBoxsPage.Count) return null;
        return _dialogTextContainersPage[_currDialogueBoxIdx - 1];
    }

}
[Serializable]
public class ComicPageEffect
{

}
public class ComicPageController : MonoBehaviour
{
    [Header("Test")]
    public bool isGo;
    public bool isStop;

    [Header("The SODialogues")]
    [SerializeField] private SODialogues _comicPagesAllDialogues_SODIALOGUES;
    private Dialogue_Line _currDialogue;
    private int _currDialogueIdx;

    [Header("All Container")]
    [SerializeField] private Comic_DialogueLine _dialogueLineContainer;
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
    private IEnumerator _comicTime;
    private bool _finished;
    private bool _canMovePage;
    public bool Finished { get { return _finished; } }

    private void Awake() {
        HideAllDialogueBox();
    }
    private void Update() {
        if(isGo)
        {
            isGo =false;
            ShowComic();
        }
        if(isStop)
        {
            isStop = false;
            StopComicTime();
        }
    }
    public void ShowComic()
    {
        if(_fade)_fade.Fade_WithFunction(PreparingComic, _fadeInDuration, 1);
        else PreparingComic();

        
    }
    public void HideComic()
    {
        _comicTime = null;
        _finished = false;
        HideAllParent();
        
    }
    public void HideAllParent()
    {
        if(_dialogueBoxParent.gameObject.activeSelf)_dialogueBoxParent.gameObject.SetActive(false);
        if(_comicImageParent.gameObject.activeSelf)_comicImageParent.gameObject.SetActive(false);
    }
    public void PreparingComic()
    {
        HideAllDialogueBox();
        _dialogueBoxParent.localPosition = _startingPoint;
        _comicImageParent.localPosition = _startingPoint;

        _currDialogueIdx = 0;
        _currDialogue = null;
        _currPage = null;
        _dialogueBoxParent.gameObject.SetActive(true);
        _comicImageParent.gameObject.SetActive(true);
        StartComicTime();
        
    }
    public void HideAllDialogueBox()
    {
        HideAllParent();
        foreach(ComicPage page in _comicPages)
        {
            page.HideAllDialogueBox();
            page.ClearAllTextContainers();
            page.SetTransparentImage_Animated(null, 0.5f, _fadeComicBGDuration);
        }
    }
    public void StartComicTime()
    {
        _comicTime = ComicTime();
        StartCoroutine(_comicTime);
    }
    public void StopComicTime()
    {
        if(_comicTime == null)return;

        StopCoroutine(_comicTime);
        
        HideComic();
    }

    public IEnumerator ComicTime()
    {
        for(int i=0; i < _comicPages.Count; i++)
        {
            _currPage = _comicPages[i];
            if(i==0)
            {
                _currPage.SetTransparentImage_Animated(null, 1f, _fadeComicBGDuration);
                if(_fade)_fade.FadeNormal(_fadeOutDuration, 0);
            }


            
            yield return new WaitUntil(()=> IsInputTrue());

            while(!_currPage.IsFInished)
            {

                _currDialogue = _comicPagesAllDialogues_SODIALOGUES.dialogue_Lines[_currDialogueIdx];
                _currPage.ShowDialogueBox(DoDialogue, _fadeDialogBoxDuration);
                _currDialogueIdx++;
                yield return new WaitUntil(()=> 
                _dialogueLineContainer.Finished);

                _dialogueLineContainer.ChangeFinished_false();
            }
            
            yield return new WaitUntil(()=> IsInputTrue());

            _currPage.SetTransparentImage_Animated(null, 0.2f, _fadeComicBGDuration);
            if( i + 1 != _comicPages.Count) _comicPages[i+1].SetTransparentImage_Animated(MoveNext, 1f, _fadeComicBGDuration);

            yield return new WaitUntil(()=> _canMovePage);
            _canMovePage = false;
            
        }
        yield return new WaitUntil(()=> IsInputTrue());
        _finished = true;

        if(_fade)_fade.Fade_WithFunction(HideComic, _fadeInDuration, 1);

    }
    public void DoDialogue()
    {
        if(_currDialogue != null)
        {
            Debug.Log("Halo");
            _dialogueLineContainer.GetTextContainer(_currPage.GiveDialogueText());
            _dialogueLineContainer.SetDialogue(_currDialogue);
        }
    }
    public bool IsInputTrue()
    {
        if(Input.GetKeyDown(KeyCode.Space)) return true;
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
}
