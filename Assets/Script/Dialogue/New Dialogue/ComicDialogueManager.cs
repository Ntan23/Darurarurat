using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicDialogueManager : MonoBehaviour
{
    [Header("Testing")]
    public bool isGO;
    public bool isStop;
    public DialoguesTitle title;
    #region CanvasPart
    #endregion

    [Header("Component Variable")]
    [SerializeField] private SODialogueList _comicDialogueList;
    [SerializeField] private DialogueHolder _dialogueHolder;
    private SODialogues _chosenDialogue;
    [Header("Container")]
    [SerializeField] private GameObject _bgContainer, _namatextContainer;
    [SerializeField] private TMP_Text _textContainer;
    private void Awake() 
    {
        if(_dialogueHolder == null)_dialogueHolder = GetComponent<DialogueHolder>();
    }
    private void Update() {
        if(isGO)
        {
            // Debug.Log("????");
            isGO = false;
            PlayDialogueScene(title);
        }
        if(isStop)
        {
            isStop = false;
            StopDialogue();
        }
    }
    public void PlayDialogueScene(DialoguesTitle dialoguesTitle)
    {
        _chosenDialogue = _comicDialogueList.SearchDialogues(dialoguesTitle);
        if(_chosenDialogue != null)
        {
            _dialogueHolder.ShowDialogue(_chosenDialogue, _textContainer);
        }
    }
    public void HideFinishedDialogueNow()
    {
        _dialogueHolder.StopCoroutineAbruptly();
        _dialogueHolder.HideDialogue();
    }
    public void StopDialogue()
    {
        _dialogueHolder.StopCourotineNow();
    }
}
