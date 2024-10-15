using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class Chat_DialogueLine : DialogueLine
    {
        private TMP_Text _nameTextContainer;
        private GameObject _bgContainer;
        private Image _spriteContainer;
        private GameObject _pressToContinueContainer;

        private INeedChatDialogue _getChatDialogueData;

        protected DialogueCharacter _currCharacter;
        public DialogueCharacter CurrCharacter { get { return _currCharacter; } set { _currCharacter = value; } }

        public void GetCharaContainer(TMP_Text nameContainer, Image spriteContainer, GameObject pressToCon)
        {
            _nameTextContainer = nameContainer;
            _spriteContainer = spriteContainer;
            _pressToContinueContainer = pressToCon;
        }
        public override void SetDialogue(Dialogue_Line dialogueInput)
        {
            _currdialogueInput = dialogueInput;
            _textContainer.text = "";
            
            _getChatDialogueData = dialogueInput as INeedChatDialogue;
            if(_getChatDialogueData.DialogueTypeNow == DialogueType.Character )
            {
                // _bgContainer.gameObject.SetActive(true);
                _nameTextContainer.text = CurrCharacter.name;
                if(CurrCharacter.charaSprites.Length > 0)_spriteContainer.sprite = CurrCharacter.charaSprites[_getChatDialogueData.SpriteNumber];
                _spriteContainer.gameObject.SetActive(true);
            }
            else if(_getChatDialogueData.DialogueTypeNow == DialogueType.Instruction)
            {
                _nameTextContainer.text = "Instruction";
            }
            _nameTextContainer.gameObject.SetActive(true);

            
            _dialogue = typeText(dialogueInput.DialogueText, _textContainer, dialogueInput.DelayTypeText, dialogueInput.DelayBetweenLines);

            // Debug.Log("???????");
            StartCoroutine(_dialogue);
        }

        public override void AfterDone_BeforeInput()
        {
            _pressToContinueContainer.SetActive(true);
        }
        public override void AfterDone_AfterInput()
        {
            _pressToContinueContainer.SetActive(false);
        }
    }

}
