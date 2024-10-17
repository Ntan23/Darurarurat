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
    private bool _isFinished;
    private IEnumerator scrollRect;

    #region  GETTER SETTER VARIABLE
    public int TotalBox { get { return _dialogueBoxsPage.Count;}}
    public bool IsFInished {get { return _isFinished; } }
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
        }
        
    }

}
[Serializable]
public class ComicPageEffect
{

}
