using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionUI : MonoBehaviour
{
    private int levelUnlocked;
    [SerializeField] private GameObject clipboard;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject[] levelButton;

    void Start() 
    {
        CheckForlevel();

        StartCoroutine(StartAnimation());
    }

    public void CloseLevelSelectionUI()
    {
        LeanTween.moveLocalY(box, 0.0f, 0.8f).setEaseSpring();
        LeanTween.moveLocalY(clipboard, -1080.0f, 0.8f).setEaseSpring(); 
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalY(box, 580.0f, 0.8f).setEaseSpring();
        LeanTween.moveLocalY(clipboard, 0.0f, 0.8f).setEaseSpring(); 
    }

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
}
