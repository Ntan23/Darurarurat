using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    private GameManager gm;
    private DialogueManager dm;

    void Start()
    {
        gm = GameManager.instance;
        dm = DialogueManager.instance;
    }

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void FadeIn()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 1.5f).setOnComplete(() => 
        {
            if(gm != null) gm.ChangeCanPauseValue(true);
        });  
    }

    public void FadeOut() 
    {
        if(gm != null) gm.ChangeCanPauseValue(false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }
}
