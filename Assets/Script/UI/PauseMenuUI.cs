using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;
    public GameObject background;
    public GameObject clipboard;
    private CanvasGroup backgroundCanvasGroup;
    private GameManager gm;

    void Start() 
    {
        gm = GameManager.instance;

        backgroundCanvasGroup = background.GetComponent<CanvasGroup>();
    }

    public void OpenPauseMenu(bool value)
    {
        if(!gm.IsPausing())
        {
            gm.ChangePauseMenuIsAnimatingValue(true);
            LeanTween.value(background, UpdateBackgroundAlpha, 0.0f, 1.0f, 0.8f);
            LeanTween.moveLocalY(clipboard, 0.0f, 0.8f).setEaseSpring();
            backgroundCanvasGroup.blocksRaycasts = true;
            
            if(value) StartCoroutine(Wait(true));
        }
    }

    public void ClosePauseMenu(bool value)
    {
        if(!gm.IsPausing())
        {
            gm.ChangePauseMenuIsAnimatingValue(true);
            LeanTween.value(background, UpdateBackgroundAlpha, 1.0f, 0.0f, 0.8f);
            LeanTween.moveLocalY(clipboard, -1092.0f, 0.8f).setEaseSpring();
            backgroundCanvasGroup.blocksRaycasts = false;
            
            if(value) StartCoroutine(Wait(false));
        }
    }

    void UpdateBackgroundAlpha(float alpha) => backgroundCanvasGroup.alpha = alpha;

    IEnumerator Wait(bool value)
    {
        yield return new WaitForSeconds(1.0f);
        gm.ChangePauseMenuIsAnimatingValue(false);
        gm.ChangeGameState(value);
    }
}
