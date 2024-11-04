using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
// using UnityEditor.SearchService;


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
        [SerializeField] private RectTransform _bgDialogueContainer;
        [SerializeField] private RectTransform _bgContainer, _topBGContainer, _botBGContainer;
        [SerializeField] private GameObject _pressToConContainer, _nameContainer;
        [SerializeField] private TMP_Text _textContainer, _nameTextContainer;
        [SerializeField] private Image _spriteContainer;
        [SerializeField] private Button _nextButtonContainer;
        [SerializeField] private float _showBGDuration = 0.2f;

        [Header("BG TOP BOT Component")]
        [SerializeField]private float nextYPoint;
        [SerializeField]private float moveDuration = 0.3f;
        private float startYPointTop, startYPointBot, nextYPointTop, nextYPointBot;

        [Tooltip("If true, langsung hide, if not true, tunggu Action apa baru tutup dr sana")]
        [SerializeField]private bool hasSceneDialogueFinish;

        [SerializeField]IEnumerator dialogSeq;
        private void Awake() 
        {
            if(_dialogueLineContainer == null) _dialogueLineContainer = GetComponent<Chat_DialogueLine>();
            startYPointTop = _topBGContainer.localPosition.y;
            startYPointBot = _botBGContainer.localPosition.y;
             nextYPointTop = startYPointTop - nextYPoint;
            nextYPointBot = startYPointBot + nextYPoint;
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
            
            _nextButtonContainer.onClick.AddListener(_dialogueLineContainer.NextButtonClicked);
            _nextButtonContainer.gameObject.SetActive(true);
            
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
            _nextButtonContainer.gameObject.SetActive(false);
            MoveOutBG(DialogueFinish);
            
            
        }

        public void ShowDialogue(SOChatDialogues SceneDialogue)
        {

            StopCourotineNow();
            
            
            //Nyalakan ini dan matikan bawah jika gamau animasi
            // dialogSeq = dialogueSequence(SceneDialogue);

            // if(dialogSeq != null)StartCoroutine(dialogSeq);
            dialogSeq = dialogueSequence(SceneDialogue);

            MoveInBG(StartDialogue);

        }
        private void StartDialogue()
        {
            _bgDialogueContainer.gameObject.SetActive(true);
            
            if(dialogSeq != null)StartCoroutine(dialogSeq);
        }
        private void DialogueFinish()
        {
            hasSceneDialogueFinish = true;
            HideDialogue();

            //Subs to this if you want to do something after dialogue finish
            DialogueManager.OnDialogueFinish?.Invoke(_currSceneDialogue.dialoguesTitle);
            _currSceneDialogue = null;
        }
        public void HideDialogue()
        {
            if(hasSceneDialogueFinish)hasSceneDialogueFinish = false;

            _pressToConContainer.SetActive(false);
            _nameContainer.gameObject.SetActive(false);
            _nameTextContainer.gameObject.SetActive(false);
            _spriteContainer.gameObject.SetActive(false);
            _nextButtonContainer.gameObject.SetActive(false);
            _nextButtonContainer.onClick.RemoveAllListeners();
            _bgDialogueContainer.gameObject.SetActive(false);

            _topBGContainer.gameObject.SetActive(false);
            _botBGContainer.gameObject.SetActive(false);
            _bgContainer.gameObject.SetActive(false);

            _textContainer.text = "";
            LeanTween.alpha(_bgContainer, 0f, 0); //matikan ini jika gamau animasi
            LeanTween.moveLocalY(_topBGContainer.gameObject, startYPointTop, 0);
            LeanTween.moveLocalY(_botBGContainer.gameObject, startYPointBot, 0);
            // gameObject.SetActive(false);
            
        }
        private void StopCourotineNow()
        {
            if(!_dialogueLineContainer.Finished)_dialogueLineContainer.StopDialogue();
            if(dialogSeq == null)return;
            if(!hasSceneDialogueFinish)StopCoroutine(dialogSeq);
            HideDialogue();
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
        public void MoveInBG(Action OnComplete)
        {
            _topBGContainer.gameObject.SetActive(true);
            _botBGContainer.gameObject.SetActive(true);
            _bgContainer.gameObject.SetActive(true);
            LeanTween.moveLocalY(_topBGContainer.gameObject, nextYPointTop, moveDuration);
            LeanTween.moveLocalY(_botBGContainer.gameObject, nextYPointBot, moveDuration);
            LeanTween.alpha(_bgContainer, 0.5f, moveDuration).setOnComplete(OnComplete);
        }
        public void MoveOutBG(Action OnComplete)
        {
            LeanTween.moveLocalY(_topBGContainer.gameObject, startYPointTop, moveDuration);
            LeanTween.moveLocalY(_botBGContainer.gameObject, startYPointBot, moveDuration).setOnComplete(OnComplete);
        }
    }
}

