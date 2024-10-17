using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat_DialogueManager : MonoBehaviour
{
    public static Chat_DialogueManager Instance {get; private set;}
    [Header("Testing")]
    public bool isGO;
    public bool isStop;
    public ChatDialoguesTitle title;


    [Header("Chat Dialogue List & Dialogue Holder")]
    [SerializeField] private SOChatDialogueList _comicDialogueList;
    [SerializeField] private Chat_DialogueHolder _dialogueHolder;
    private SOChatDialogues _chosenDialogue;

    public Action<ChatDialoguesTitle> OnDialogueFinish; //SUBS TO THIS IF YOU WANT TO DO SOMETHING AFTER DIALOGUE DONEE

    private void Awake() 
    {
        if(_dialogueHolder == null)_dialogueHolder = GetComponent<Chat_DialogueHolder>();
        if(Instance == null)
        {
            Instance = this;
        }
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

    public void PlayDialogueScene(ChatDialoguesTitle dialoguesTitle)
    {
        _chosenDialogue = _comicDialogueList.SearchDialogues(dialoguesTitle);
        if(_chosenDialogue != null)
        {
            _dialogueHolder.ShowDialogue(_chosenDialogue);
        }
    }
    public void PlayDialogueSceneUsingStringTitlte(string titleNow)
    {
        _chosenDialogue = _comicDialogueList.SearchDialoguesUsingString(titleNow);
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
    // public void StopDialogueWithoutHiding()
    // {
    //     _dialogueHolder.StopCourotineNow();
    // }
    
}
