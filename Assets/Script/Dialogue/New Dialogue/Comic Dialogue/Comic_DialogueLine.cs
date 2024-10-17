using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Comic_DialogueLine : DialogueLine
    {
        [SerializeField] private int _delayTimeForVA;
        INeedVA _currVA;
        public override void TypeFunction(int inputText_IDX)
        {
            StartVA(inputText_IDX);
            
        }
        public override void AfterDone_AfterInput()
        {
            StopVA();
        }
        public override void StopDialogue()
        {
            base.StopDialogue();
            StopVA();
        }

        private void StartVA(int inputText_IDX)
        {
            if(_currdialogueInput != null)
            {
                _currVA = _currdialogueInput as INeedVA;
                if(_currVA == null) return;
                if(inputText_IDX == _delayTimeForVA && _currVA.VAname != "")
                {
                    AudioManager.instance.PlayDialogueVAAudio_SFX(_currVA.VAname);
                }
            }
        }
        private void StopVA()
        {
            AudioManager.instance.StopDialogueVAAudio_SFX(_currVA.VAname);
        }
    }

}
