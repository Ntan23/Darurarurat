using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class DialogueLine : DialogueBase
    {


        [Header("Character")]
        protected TMP_Text _textContainer;

        protected string _dialogueText;
        protected IEnumerator _dialogue;

        protected Dialogue_Line _currdialogueInput;

        
        public void GetTextContainer(TMP_Text textContainer)
        {
            _textContainer = textContainer;
            // Debug.Log(textContainer.name);
        }

        public virtual void SetDialogue(Dialogue_Line dialogueInput)
        {
            _currdialogueInput = dialogueInput;
            _textContainer.text = "";

            // textHolder.color = textColor;
            // textHolder.font = textFont;

            _dialogue = typeText(dialogueInput.DialogueTextLocalized, _textContainer, dialogueInput.DelayTypeText, dialogueInput.DelayBetweenLines);

            // Debug.Log("???????");
            StartCoroutine(_dialogue);
        }
        public virtual void StopDialogue()
        {
            if(_dialogue == null) return;

            StopCoroutine(_dialogue);
            _dialogue = null;
            _finished = false;
        }
    }
}
