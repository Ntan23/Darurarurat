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
        [Header("Option")]
        [Tooltip("Select if this a instruction or character dialogue")]
        [SerializeField] private DialogueType dialogueType;
        [Header("Time Delay ")]
        [SerializeField] private float delayTypeText;
        [SerializeField] private float delayBetweenLines;
        [Header("Character")]
        [SerializeField] private string charaName;
        [SerializeField] private TMP_Text _textContainer;
        [SerializeField] private TMP_Text _nameTextContainer;
        [SerializeField] private GameObject _bgContainer;
        [SerializeField] private Image _imgContainer;
        private string dialogueText;
        private IEnumerator dialogue;

        private void Awake() 
        {
            _textContainer = GetComponent<TMP_Text>();
        }
        //Selalu Panggil Atas dl Baru Bawah
        public void GetContainer(TMP_Text textContainer, TMP_Text nameContainer, GameObject bgContainer, Image imgContainer)
        {
            _textContainer = textContainer;
            _nameTextContainer = nameContainer;
            _bgContainer = bgContainer;
            _imgContainer = imgContainer;

            //jujur keknya muncul-munculan dri bg ama img diurusnya jgn disini
        }
        public void SetDialogue(Dialogue_Line dialogueInput, DialogueCharacter character)
        {
            
            _textContainer.text = "";
            if(dialogueInput.dialogueType == DialogueType.Character || dialogueInput.dialogueType == DialogueType.SFX)
            {
                _bgContainer.gameObject.SetActive(true);
                if(dialogueInput.dialogueType == DialogueType.Character)
                {
                    _nameTextContainer.color = character._textColor; // or colornya bs buat ganti color bg idk
                    _nameTextContainer.text = character.name;
                    _nameTextContainer.gameObject.SetActive(true);
                }
            }
            dialogue = typeText(dialogueInput.dialogueText, _textContainer, dialogueInput.delayTypeText, dialogueInput.delayBetweenLines, dialogueInput.VAname, character._textColor);
            StartCoroutine(dialogue);
        }
        public void StopDialogue()
        {
            if(dialogue == null) return;

            StopCoroutine(dialogue);
            dialogue = null;
            _finished = false;
        }
    }
}
