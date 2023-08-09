using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    private int levelUnlocked;
    private int levelIndex;
    [SerializeField] private GameObject clipboard;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject[] levelButton;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private GameObject levelIndicator;
    [SerializeField] private Button previousButton;

    // void Start() 
    // {
    //     CheckForlevel();

    //     StartCoroutine(StartAnimation());
    // }

    // public void CloseLevelSelectionUI()
    // {
    //     LeanTween.moveLocalY(box, 0.0f, 0.8f).setEaseSpring();
    //     LeanTween.moveLocalY(clipboard, -1080.0f, 0.8f).setEaseSpring(); 
    // }

    // IEnumerator StartAnimation()
    // {
    //     yield return new WaitForSeconds(0.3f);
    //     LeanTween.moveLocalY(box, 580.0f, 0.8f).setEaseSpring();
    //     LeanTween.moveLocalY(clipboard, 0.0f, 0.8f).setEaseSpring(); 
    // }

    private void CheckForlevel()
    {
        levelUnlocked = PlayerPrefs.GetInt("LevelUnlocked", 1);

        for(int i = 1; i <= levelButton.Length; i++)
        {
            if(i > levelUnlocked)
            {
                levelButton[i - 1].GetComponent<CanvasGroup>().interactable = false;
                levelButton[i - 1].GetComponent<CanvasGroup>().alpha = 0.5f;
            }
        }
    }

    public void MoveNext()
    {
        if(levelIndex < 3)
        {
            LeanTween.moveLocalX(levels[levelIndex], -1300.0f, 0.8f).setEaseOutExpo();
            LeanTween.moveLocalX(levels[levelIndex + 1], 0.0f, 0.8f).setEaseOutExpo();
            LeanTween.moveLocalX(levelIndicator.gameObject, levelIndicator.transform.localPosition.x - 191.0f, 0.8f).setEaseOutExpo();

            levelIndex++;
            if(levelIndex > 0) previousButton.interactable = true;
        }
    }

    public void MovePrevious()
    {
        if(levelIndex > 0)
        {
            LeanTween.moveLocalX(levels[levelIndex], 1300.0f, 0.8f).setEaseOutExpo();
            LeanTween.moveLocalX(levels[levelIndex - 1], 0.0f, 0.8f).setEaseOutExpo();
            LeanTween.moveLocalX(levelIndicator.gameObject, levelIndicator.transform.localPosition.x + 191.0f, 0.8f).setEaseOutExpo();
    
            levelIndex--;
            if(levelIndex == 0) previousButton.interactable = false;
        }
    }
}
