using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public struct DialogueFinishActions
{
    public ChatDialoguesTitle title;
    public List<UnityEvent> actionOnFinish;
}
public class Chat_DialogueManager : MonoBehaviour
{
    public static Chat_DialogueManager Instance {get; private set;}
    // [Header("Testing")]
    // public bool isGO;
    // public bool isStop;
    // public ChatDialoguesTitle title;


    [Header("Chat Dialogue List & Dialogue Holder")]
    [SerializeField] private SOChatDialogueList _comicDialogueList;
    [SerializeField] private Chat_DialogueHolder _dialogueHolder;
    private SOChatDialogues _chosenDialogue;

    public Action<ChatDialoguesTitle> OnDialogueFinish; //SUBS TO THIS IF YOU WANT TO DO SOMETHING AFTER DIALOGUE DONEE
    public List<DialogueFinishActions> _dialogueFinishActionList;

    private void Awake() 
    {
        if(_dialogueHolder == null)_dialogueHolder = GetComponent<Chat_DialogueHolder>();
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start() 
    {
        OnDialogueFinish += OnDialogueFinishDo;
    }


    // private void Update() {
    //     if(isGO)
    //     {
    //         // Debug.Log("????");
    //         isGO = false;
    //         PlayDialogueScene(title);
    //     }
    //     if(isStop)
    //     {
    //         isStop = false;
    //         HideFinishedDialogueNow();
    //     }
    // }

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

    private void OnDialogueFinishDo(ChatDialoguesTitle title)
    {
        if(GameManager.instance != null && GameManager.instance.IsCutscene())
        {
            GameManager.instance.ChangeCutsceneStatus(false);
        }
        foreach(DialogueFinishActions action in _dialogueFinishActionList)
        {
            if(action.title == title)
            {
                foreach(UnityEvent actionNow in action.actionOnFinish)
                {
                    actionNow.Invoke();
                }
                break;
            }
        }
        if(title == ChatDialoguesTitle.OpeningGame)
        {
            PlayerPrefs.SetInt("IsFirstimePlaying", 1);
        }

    } 
}
