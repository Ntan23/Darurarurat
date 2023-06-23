using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject background;
    public GameObject clipboard;
    private CanvasGroup backgroundCanvasGroup;

    void Start() => backgroundCanvasGroup = background.GetComponent<CanvasGroup>();

    public void OpenPauseMenu()
    {
        LeanTween.value(background, UpdateBackgroundAlpha, 0.0f, 1.0f, 0.8f);
        LeanTween.moveLocalY(clipboard, 0.0f, 0.8f).setEaseSpring();
        backgroundCanvasGroup.blocksRaycasts = true;
    }

    public void ClosePauseMenu()
    {
        LeanTween.value(background, UpdateBackgroundAlpha, 1.0f, 0.0f, 0.8f);
        LeanTween.moveLocalY(clipboard, -1092.0f, 0.8f).setEaseSpring();
        backgroundCanvasGroup.blocksRaycasts = false;
    }

    void UpdateBackgroundAlpha(float alpha) => backgroundCanvasGroup.alpha = alpha;
}
