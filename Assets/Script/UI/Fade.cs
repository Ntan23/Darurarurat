using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private GameManager gm;
    private DialogueManager dm;
    private StoryManager sm;

    void Start()
    {
        gm = GameManager.instance;
        //dm = DialogueManager.instance;
        sm = StoryManager.instance;
    }

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void FadeIn() 
    {
        StartCoroutine(FadeInAnimation());
    }

    public void FadeOut() 
    {
        if(gm != null) gm.ChangeCanPauseValue(false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }

    IEnumerator FadeInAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        if(gm != null)
        {
            if(!gm.CanSkip()) sm.DisableSkipButton();
            else if(gm.CanSkip()) sm.EnableSkipButton();
        }

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 1.5f).setOnComplete(() => 
        {
            if(gm != null) gm.ChangeCanPauseValue(true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        });  
    }
}
