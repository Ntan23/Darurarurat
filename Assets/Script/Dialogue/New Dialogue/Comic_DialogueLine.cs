using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class Comic_DialogueLine : DialogueLine
    {
        [SerializeField] private int _delayTimeForVA;
        public override void TypeFunction(int inputText_IDX)
        {
            base.TypeFunction(inputText_IDX);
            if(_currdialogueInput != null)
            {
                if(inputText_IDX == _delayTimeForVA && _currdialogueInput.VAname != "")
                {
                    AudioManager.instance.PlayDialogueVASFX(_currdialogueInput.VAname);
                }
            }
        }
    }

}
