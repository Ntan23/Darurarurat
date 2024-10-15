using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat_DialogueManager : MonoBehaviour
{
    [Header("Testing")]
    public bool isGO;
    public bool isStop;
    public DialoguesTitle title;
    #region CanvasPart
    #endregion

    [Header("Component Variable")]
    [SerializeField] private SOChatDialogueList _comicDialogueList;
    [SerializeField] private Chat_DialogueHolder _dialogueHolder;
    private SOChatDialogues _chosenDialogue;

    // [Header("Container")]
    // [SerializeField] private GameObject _bgContainer;
    // [SerializeField] private TMP_Text _textContainer, _nameTextContainer;
    private void Awake() 
    {
        if(_dialogueHolder == null)_dialogueHolder = GetComponent<Chat_DialogueHolder>();
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
            HideFinishedDialogueNow();
        }
    }
    public void PlayDialogueScene(DialoguesTitle dialoguesTitle)
    {
        _chosenDialogue = _comicDialogueList.SearchDialogues(dialoguesTitle);
        if(_chosenDialogue != null)
        {
            _dialogueHolder.ShowDialogue(_chosenDialogue);
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
