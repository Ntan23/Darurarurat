using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DialogueSystem{
    public class DialogueBase : MonoBehaviour
    {
        protected bool _finished;
        public bool Finished { get { return _finished;}}
        private bool isRichText = false;
        string saveRichText;
        /// Kalo mo tambahin sfx tengah-tengah bisa pake isrichtext juga modelannya :D, tp bikin penanda sendiri; delay jg bisa
        /// kek misal <sfx: sfxname> or <delay: 0.2f>
        public IEnumerator typeText(string inputText, TMP_Text textHolder, float delayTypeText, float delayBetweenLines, string vaName, Color textColor)
        {
            // textHolder.color = textColor;
            // textHolder.font = textFont;
            
            for(int i=0; i<inputText.Length;i++)
            {
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
                        textHolder.text += saveRichText;
                    }
                    continue;
                }
                
                textHolder.text += inputText[i];
                
                // Debug.Log(inputText[i] + " " + i);
                yield return new WaitForSeconds(delayTypeText);
                
            }
            yield return new WaitForSeconds(delayBetweenLines);

            // yield return new WaitUntil(()=>GameInput.Instance.GetInputNextLine_Dialogue());
            // pressToContinue_textHolder.SetActive(false);
            
            _finished = true;
            // textHolder.gameObject.SetActive(false);
            // imageHolder.SetActive(false);
            // nameHolder.SetActive(false);
            // backgroundHolder.SetActive(false);
            
            
        }
        public void ChangeFinished_false()
        {
            _finished = false;
        }
    }

}
