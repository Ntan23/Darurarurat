using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private List<TMP_Text> _dialogueTextContainersPage;
    [SerializeField] private List<ScrollRect> _dialogueBoxScrollRect;
    [Header("Just Empty if No SFX")]
    [Header("Put it based on other number")]
    [SerializeField] private List<string> _startingDialogueSFX;
    [SerializeField] private List<string> _endingDialogueSFX;
    [Header("This is just for starting, when comicpage was shown")]
    [SerializeField] private string _startComicSFX;
    private bool _isFinished;
    private IEnumerator scrollRect;

    #region  GETTER SETTER VARIABLE
    public int TotalBox { get { return _dialogueBoxsPage.Count;}}
    public bool IsFinished {get { return _isFinished; } }
    public List<GameObject> DialogueBoxsPage { get { return _dialogueBoxsPage;}}
    public List<TMP_Text> DialogueTextContainersPage { get { return _dialogueTextContainersPage; } set{ _dialogueTextContainersPage = value;}}
    public List<ScrollRect> DialogueBoxScrollRect { get { return _dialogueBoxScrollRect; } set{ _dialogueBoxScrollRect = value;}}


    #endregion
    public void ClearAllTextContainers()
    {
        foreach(TMP_Text dialogueText in _dialogueTextContainersPage)
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

        return _dialogueTextContainersPage[_currDialogueBoxIdx - 1];
    }

    public IEnumerator ScrollRectChecker()
    {
        while(true)
        {
            // yield return
            yield return new WaitForEndOfFrame();
            // yield return new WaitForEndOfFrame();
            // Debug.Log(_dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition + "scroll" + _dialogueBoxsPage[_currDialogueBoxIdx - 1]);
            _dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition = new Vector2(_dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition.x, 0);
            Debug.Log("normalized pos normal " +_dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition );
        }
        
    }
    public void NormalizeScrollRectPosFinal()
    {
        _dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition = new Vector2(1, 0);
        Debug.Log("normalized pos normal2 " +_dialogueBoxScrollRect[_currDialogueBoxIdx - 1].normalizedPosition );
    }
    public void PlayStartingDialogueSFX()
    {
        if(_startingDialogueSFX == null || _currDialogueBoxIdx > _startingDialogueSFX.Count - 1 ||_startingDialogueSFX[_currDialogueBoxIdx] == "" ) return;

        AudioManager.instance.PlayDialogueVAAudio_SFX(_startingDialogueSFX[_currDialogueBoxIdx]);
    }
    public void PlayEndingDialogueSFX()
    {
        if(_endingDialogueSFX == null || _currDialogueBoxIdx - 1 == _endingDialogueSFX.Count || _currDialogueBoxIdx - 1 > _endingDialogueSFX.Count - 1 || _endingDialogueSFX[_currDialogueBoxIdx - 1] == "" ) return;
        Debug.Log("Ending" + _endingDialogueSFX[_currDialogueBoxIdx - 1]);
        AudioManager.instance.PlayDialogueVAAudio_SFX(_endingDialogueSFX[_currDialogueBoxIdx - 1]);
    }
    public void PlayStartingSFX()
    {
        if(_startComicSFX == "" ) return;

        AudioManager.instance.PlayDialogueVAAudio_SFX(_startComicSFX);
    }

}
[Serializable]
public class ComicPageEffect
{

}
