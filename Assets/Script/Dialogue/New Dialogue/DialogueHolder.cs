using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using Unity.VisualScripting;

namespace DialogueSystem
{
    public class DialogueHolder : MonoBehaviour
    {
        [Header("Dialogue Line")]
        [SerializeField] private DialogueLine _dialogueLineContainer;

        [Header("Reference")]
        [SerializeField] private GameObject _bgContainer, _namatextContainer;
        [SerializeField] private TMP_Text _textContainer;
        [SerializeField] private SODialogueCharaList _SOdialogueCharaList;

        [Tooltip("If true, langsung hide, if not true, tunggu Action apa baru tutup dr sana")]
        [SerializeField]private bool hasSceneDialogueFinish;

        [SerializeField]IEnumerator dialogSeq;
        private void Awake() 
        {
            if(_dialogueLineContainer == null) _dialogueLineContainer = GetComponent<DialogueLine>();
            // HideDialogue();
        }

        private IEnumerator dialogueSequence(SODialogues SceneDialogue)
        {
            _dialogueLineContainer.GetTextContainer(_textContainer);
            for(int i=0;i<SceneDialogue.dialogue_Lines.Count;i++)
            {
                if(!_dialogueLineContainer.gameObject.activeSelf)_dialogueLineContainer.gameObject.SetActive(true);
                
                
                // line.GoLineText();
                // yield return new WaitUntil(()=> line.finished);
                // line.ChangeFinished_false();
                Dialogue_Line dialogueNow = SceneDialogue.dialogue_Lines[i];
                int dialogueTalkerNow = (int)dialogueNow.charaName;
                _dialogueLineContainer.SetDialogue(dialogueNow);

                yield return new WaitUntil(()=> _dialogueLineContainer.Finished);
                _dialogueLineContainer.ChangeFinished_false();
                
            }
            hasSceneDialogueFinish = true;

            // if(DialogueManager.DoSomethingAfterFinish != null)DialogueManager.DoSomethingAfterFinish();
        }
        private void Deactivate()
        {
            _dialogueLineContainer.gameObject.SetActive(false);
        }
        public void ShowDialogue(SODialogues SceneDialogue, TMP_Text textContainer)
        {
            _textContainer = textContainer;
            StopCourotineNow();
            gameObject.SetActive(true);
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
            _namatextContainer.SetActive(false);
            _bgContainer.SetActive(false);
            gameObject.SetActive(false);
            
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

            //gaperlu bawah krn ini bakal cuma terjadi di A suruh ngapain, yg biasanya di akhirnya bakal disuru hal lain yg bukan manggil dialog ke dialog, ex : suruh ambil barang, kalo brgnya ud diambil si brg ini yg bakal ngelakuin itu, bukan dr sininya, biasanya ini kalo dr dialog abis itu lanjut dialog, ato dialog abis itu lanjut timeline, krn emg gabisa diskip lol
            
            // DialogueManager.DoSomethingAfterFinish();
        }

        public bool HasSceneDialogueFinish() {return hasSceneDialogueFinish;}
    }
}

