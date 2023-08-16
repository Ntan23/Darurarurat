using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;
    // public GameObject background;
    // public GameObject clipboard;
    private CanvasGroup canvasGroup;
    private GameManager gm;

    void Start() 
    {
        gm = GameManager.instance;

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OpenPauseMenu(bool value)
    {
        if(!gm.IsPausing())
        {
            gm.ChangePauseMenuIsAnimatingValue(true);
            LeanTween.value(gameObject, UpdateBackgroundAlpha, 0.0f, 1.0f, 0.5f);
            //LeanTween.moveLocalY(clipboard, 0.0f, 0.8f).setEaseSpring();
            canvasGroup.blocksRaycasts = true;
            
            if(value) StartCoroutine(Wait(true));
        }
    }

    public void ClosePauseMenu(bool value)
    {
        if(!gm.IsPausing())
        {
            gm.ChangePauseMenuIsAnimatingValue(true);
            LeanTween.value(gameObject, UpdateBackgroundAlpha, 1.0f, 0.0f, 0.5f);
            //LeanTween.moveLocalY(clipboard, -1092.0f, 0.8f).setEaseSpring();
            canvasGroup.blocksRaycasts = false;
            
            if(value) StartCoroutine(Wait(false));
        }
    }

    void UpdateBackgroundAlpha(float alpha) => canvasGroup.alpha = alpha;

    IEnumerator Wait(bool value)
    {
        yield return new WaitForSeconds(1.0f);
        gm.ChangePauseMenuIsAnimatingValue(false);
        gm.ChangeGameState(value);
    }
}
