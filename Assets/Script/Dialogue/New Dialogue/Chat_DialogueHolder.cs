using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;


namespace DialogueSystem
{
    public class Chat_DialogueHolder : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private SODialogueCharaList _SOdialogueCharaList;

        [Header("Dialogue Line")]
        [SerializeField] private Chat_DialogueLine _dialogueLineContainer;

        [Header("Containers")]
        [SerializeField] private GameObject _bgContainer, _pressToConContainer;
        [SerializeField] private TMP_Text _textContainer, _nameTextContainer;
        [SerializeField] private Image _spriteContainer;

        [Tooltip("If true, langsung hide, if not true, tunggu Action apa baru tutup dr sana")]
        [SerializeField]private bool hasSceneDialogueFinish;

        [SerializeField]IEnumerator dialogSeq;
        private void Awake() 
        {
            if(_dialogueLineContainer == null) _dialogueLineContainer = GetComponent<Chat_DialogueLine>();
            HideDialogue();
        }

        private IEnumerator dialogueSequence(SOChatDialogues SceneDialogue)
        {
            _dialogueLineContainer.GetTextContainer(_textContainer);
            _dialogueLineContainer.GetCharaContainer(_nameTextContainer, _spriteContainer, _pressToConContainer);
            
            for(int i=0;i<SceneDialogue.dialogue_Lines.Count;i++)
            {
                if(!_dialogueLineContainer.gameObject.activeSelf)_dialogueLineContainer.gameObject.SetActive(true);
                
                
                Chat_Dialogue_Line dialogueNow = SceneDialogue.dialogue_Lines[i];
                
                int dialogueTalkerNow = (int)dialogueNow.CharaName;

                _dialogueLineContainer.CurrCharacter = _SOdialogueCharaList.SearchCharacter(dialogueTalkerNow);
                _dialogueLineContainer.SetDialogue(dialogueNow);

                yield return new WaitUntil(()=> _dialogueLineContainer.Finished);
                _dialogueLineContainer.ChangeFinished_false();
                
            }
            hasSceneDialogueFinish = true;
            HideDialogue();

            // if(DialogueManager.DoSomethingAfterFinish != null)DialogueManager.DoSomethingAfterFinish();
        }
        private void Deactivate()
        {
            _dialogueLineContainer.gameObject.SetActive(false);
        }
        public void ShowDialogue(SOChatDialogues SceneDialogue)
        {

            StopCourotineNow();
            _bgContainer.SetActive(true);
            dialogSeq = dialogueSequence(SceneDialogue);

            if(dialogSeq != null)StartCoroutine(dialogSeq);
            else
            {
                Debug.Log("WHY IT'S NULL");
                Debug.Log(SceneDialogue);
            }
            
        }
        public void HideDialogue()
        {
            if(hasSceneDialogueFinish)hasSceneDialogueFinish = false;

            _pressToConContainer.SetActive(false);
            _nameTextContainer.gameObject.SetActive(false);
            _spriteContainer.gameObject.SetActive(false);
            _bgContainer.SetActive(false);
            // gameObject.SetActive(false);
            
        }
        public void StopCourotineNow()
        {
            if(!_dialogueLineContainer.Finished)_dialogueLineContainer.StopDialogue();
            if(dialogSeq == null)return;
            if(!hasSceneDialogueFinish)StopCoroutine(dialogSeq);
            dialogSeq = null;
            
        }

        public void StopCoroutineAbruptly()
        {
            StopCourotineNow();
            HideDialogue();

        }

        public bool HasSceneDialogueFinish() {return hasSceneDialogueFinish;}

        // public void GetContainer()
    }
}

