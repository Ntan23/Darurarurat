using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class Chat_DialogueLine : DialogueLine
    {
        [SerializeField] private string charaName;
        [SerializeField] private TMP_Text _nameTextContainer;
        [SerializeField] private GameObject _bgContainer;
        [SerializeField] private Image _imgContainer;
        [SerializeField] private GameObject _pressToContinueContainer;

        protected DialogueCharacter _currCharacter;
        public DialogueCharacter CurrCharacter { get { return _currCharacter; } set { _currCharacter = value; } }
        public override void SetDialogue(Dialogue_Line dialogueInput)
        {
            
            _textContainer.text = "";
            // if(dialogueInput.dialogueType == DialogueType.Character || dialogueInput.dialogueType == DialogueType.SFX)
            // {
            //     _bgContainer.gameObject.SetActive(true);
            //     if(dialogueInput.dialogueType == DialogueType.Character)
            //     {
            //         _nameTextContainer.color = character._textColor; // or colornya bs buat ganti color bg idk
            //         _nameTextContainer.text = character.name;
            //         _nameTextContainer.gameObject.SetActive(true);
            //     }
            // }
            // dialogue = typeText(dialogueInput.dialogueText, _textContainer, dialogueInput.delayTypeText, dialogueInput.delayBetweenLines, dialogueInput.VAname, character._textColor, _pressToContinueContainer);
            // StartCoroutine(dialogue);
        }
    }

}
