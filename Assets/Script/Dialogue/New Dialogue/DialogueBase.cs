using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace DialogueSystem{
    public class DialogueBase : MonoBehaviour
    {
        [SerializeField] protected int _maxWords_ForFullText = 1;
        protected bool _finished;
        private bool isRichText = false;
        private string saveRichText;
        private const string RICH_TEXT_SFX = "sfx";

        /// Kalo mo tambahin sfx tengah-tengah bisa pake isrichtext juga modelannya :D, tp bikin penanda sendiri; delay jg bisa
        /// kek misal <sfx: sfxname> or <delay: 0.2f>
        
        public bool Finished { get { return _finished;}}
        public IEnumerator typeText(string inputText, TMP_Text textHolder, float delayTypeText, float delayBetweenLines)
        {
            
            isRichText = false;
            
            for(int i=0; i<inputText.Length;i++)
            {
                
                if(i > _maxWords_ForFullText && Input.GetKey(KeyCode.Space)){
                    inputText = Regex.Replace(inputText, "<sfx:.*?>", string.Empty);
                    textHolder.text = inputText;
                    break;
                }
                if(inputText[i] == '<' && !isRichText)
                {
                    isRichText = true;
                    // isRichTextDone = false;
                    saveRichText = "";
                }
                if(isRichText)
                {
                    saveRichText += inputText[i];
                    if(inputText[i] == '>')
                    {
                        isRichText = false;
                        if(saveRichText.Contains(RICH_TEXT_SFX))
                        {
                            PlaySFXBaseOnDialogue();
                        }
                        else textHolder.text += saveRichText;
                    }
                    continue;
                }
                   

                textHolder.text += inputText[i];

                TypeFunction(i);
                // Debug.Log(inputText[i] + " " + i);
                yield return new WaitForSeconds(delayTypeText);
                
            }
            AfterDone_BeforeInput();
            
            yield return new WaitForSeconds(delayBetweenLines);
            
            yield return new WaitUntil(()=> IsInputTrue());
            
            AfterDone_AfterInput();
            
            _finished = true;
            // Debug.Log("this dialog done");
                        
        }


        public virtual void AfterDone_BeforeInput(){}
        public virtual void AfterDone_AfterInput(){}
        public virtual void TypeFunction(int inputText_IDX){}
        public void ChangeFinished_false()
        {
            _finished = false;
            // Debug.Log("fakse lg");
        }
        public void PlaySFXBaseOnDialogue()
        {
            int startIdx = saveRichText.IndexOf(":") + 1;
            string sfxName = saveRichText.Substring(startIdx, saveRichText.Length - startIdx - 1);
            // Debug.Log(sfxName);
            AudioManager.instance.PlayDialogueVAAudio_SFX(sfxName);
        }

        public bool IsInputTrue()
        {
            if(Input.GetKeyDown(KeyCode.Space)) return true;
            return false;
        }

    }

}
