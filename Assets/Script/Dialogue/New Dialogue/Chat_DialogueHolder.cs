using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using UnityEditor.SearchService;


namespace DialogueSystem
{
    public class Chat_DialogueHolder : MonoBehaviour
    {
        private Chat_DialogueManager DialogueManager;
        [Header("Character")]
        [SerializeField] private SODialogueCharaList _SOdialogueCharaList;
        private SOChatDialogues _currSceneDialogue;

        [Header("Dialogue Line")]
        [SerializeField] private Chat_DialogueLine _dialogueLineContainer;

        [Header("Containers")]
        [SerializeField] private RectTransform _bgContainer;
        [SerializeField] private GameObject _pressToConContainer, _nameContainer;
        [SerializeField] private TMP_Text _textContainer, _nameTextContainer;
        [SerializeField] private Image _spriteContainer;
        [SerializeField] private float _showBGDuration = 0.2f;

        [Tooltip("If true, langsung hide, if not true, tunggu Action apa baru tutup dr sana")]
        [SerializeField]private bool hasSceneDialogueFinish;

        [SerializeField]IEnumerator dialogSeq;
        private void Awake() 
        {
            if(_dialogueLineContainer == null) _dialogueLineContainer = GetComponent<Chat_DialogueLine>();
            HideDialogue();
        }
        private void Start() 
        {
            DialogueManager = Chat_DialogueManager.Instance;
        }

        private IEnumerator dialogueSequence(SOChatDialogues SceneDialogue)
        {
            _currSceneDialogue = SceneDialogue;
            _dialogueLineContainer.GetTextContainer(_textContainer);
            _dialogueLineContainer.GetCharaContainer(_nameTextContainer, _spriteContainer, _nameContainer, _pressToConContainer, SceneDialogue.isWholeDialogueUseSprite);
            
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

            //Subs to this if you want to do something after dialogue finish
            DialogueManager.OnDialogueFinish?.Invoke(SceneDialogue.dialoguesTitle);
            _currSceneDialogue = null;
            
        }

        public void ShowDialogue(SOChatDialogues SceneDialogue)
        {

            StopCourotineNow();



            
            _bgContainer.gameObject.SetActive(true);
            //Nyalakan ini dan matikan bawah jika gamau animasi
            // dialogSeq = dialogueSequence(SceneDialogue);

            // if(dialogSeq != null)StartCoroutine(dialogSeq);

            LeanTween.alpha(_bgContainer, 1, _showBGDuration).setOnComplete(
                ()=>
                {
                    dialogSeq = dialogueSequence(SceneDialogue);

                    if(dialogSeq != null)StartCoroutine(dialogSeq);
                }
            );
            


            
            
        }
        public void HideDialogue()
        {
            if(hasSceneDialogueFinish)hasSceneDialogueFinish = false;

            _pressToConContainer.SetActive(false);
            _nameContainer.gameObject.SetActive(false);
            _nameTextContainer.gameObject.SetActive(false);
            _spriteContainer.gameObject.SetActive(false);
            _bgContainer.gameObject.SetActive(false);
            _textContainer.text = "";
            LeanTween.alpha(_bgContainer, 0f, 0); //matikan ini jika gamau animasi

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

            if(_currSceneDialogue != null)
            {
                DialogueManager.OnDialogueFinish?.Invoke(_currSceneDialogue.dialoguesTitle);
                _currSceneDialogue = null;
            }
        }

        public bool HasSceneDialogueFinish() {return hasSceneDialogueFinish;}

        // public void GetContainer()
    }
}

