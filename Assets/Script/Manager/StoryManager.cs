using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    #region Singleton
    public static StoryManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    [SerializeField] private GameObject[] stories;
    [SerializeField] private GameObject storyBoard;
    // [SerializeField] private GameObject startStoryBoard;
    // [SerializeField] private GameObject endStoryboard;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private ButtonFX previousButtonFX;
    [SerializeField] private int cutStoryIndex;
    private bool isOpen = true;
    private int storyIndex;
    private ScenesManager sm;
    private GameManager gm;
    private AudioManager am;

    void Start()
    {
        sm = ScenesManager.instance;
        am = AudioManager.instance;
        gm = GameManager.instance;

        StartCoroutine(StartAnimation());
    }

    private void UpdateNextStoryAlpha(float alpha) => stories[storyIndex + 1].GetComponent<CanvasGroup>().alpha = alpha;
    private void UpdateCurrentStoryAlpha(float alpha) => stories[storyIndex].GetComponent<CanvasGroup>().alpha = alpha;
    private void UpdatePreviousStoryAlpha(float alpha) => stories[storyIndex - 1].GetComponent<CanvasGroup>().alpha = alpha;
    private void UpdatePreviousButtonAlpha(float alpha) => previousButton.GetComponent<CanvasGroup>().alpha = alpha;

    public void NextStory()
    {
        am.StopAllSFX();
        previousButton.interactable = false;
        nextButton.interactable = false;
        
        storyIndex++;

        if(storyIndex == cutStoryIndex) 
        {
            isOpen = false;
            stories[storyIndex - 1].GetComponent<CanvasGroup>().alpha = 0.0f;
            MoveStoryBoard();
        }
        else if(storyIndex < stories.Length) PlayNextStoryAnimation();
        else if(storyIndex == stories.Length) sm.GoToNextScene();
    }

    public void PreviousStory() => PlayPreviousStoryAnimation();
    
    public void SkipStory()
    {
        am.StopAllSFX();

        isOpen = false;
        storyIndex++;

        if(storyIndex <= cutStoryIndex) 
        {
            stories[storyIndex - 1].GetComponent<CanvasGroup>().alpha = 0.0f;
            storyIndex = cutStoryIndex;
            MoveStoryBoard();
        }
        else if(storyIndex > cutStoryIndex) sm.GoToNextScene();
    }

    public void ShowEndStory()
    {
        isOpen = true;
        nextButton.interactable = false;
        // startStoryBoard.SetActive(false);
        // endStoryboard.SetActive(true);
        LeanTween.moveLocalX(storyBoard, 0.0f, 0.5f).setEaseSpring().setOnComplete(() =>
        {
            LeanTween.value(stories[storyIndex], UpdateCurrentStoryAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
            {
                nextButton.interactable = true;
            });
        });
    }

    private void PlayNextStoryAnimation()
    {
        if(storyIndex == 1 || storyIndex == cutStoryIndex + 1) 
        {
            LeanTween.value(previousButton.gameObject , UpdatePreviousButtonAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() => previousButtonFX.EnableSFX());
        }
        // LeanTween.moveLocalX(stories[storyIndex - 1], -1924.0f, 0.5f).setEaseSpring();
        // LeanTween.moveLocalX(stories[storyIndex], 0, 0.5f).setEaseSpring().setOnComplete(() => 
        // {
        //     if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
        // });
        LeanTween.value(stories[storyIndex - 1], UpdatePreviousStoryAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
        {
            LeanTween.value(stories[storyIndex], UpdateCurrentStoryAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
            {
                previousButton.interactable = true;
                nextButton.interactable = true;

                if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
            });
        });
    }

    private void PlayPreviousStoryAnimation()
    {
        am.StopAllSFX();
        if(storyIndex > 0 && storyIndex != cutStoryIndex)
        {
            previousButton.interactable = false;
            nextButton.interactable = false;

            storyIndex--;

            if(storyIndex == 0 || storyIndex == cutStoryIndex) 
            {
                previousButtonFX.RemoveSFX();
                LeanTween.value(previousButton.gameObject, UpdatePreviousButtonAlpha, 1.0f, 0.0f, 0.5f);
            }
            
            // LeanTween.moveLocalX(stories[storyIndex + 1], 1924.0f, 0.5f).setEaseSpring();
            // LeanTween.moveLocalX(stories[storyIndex], 0.0f, 0.5f).setEaseSpring().setOnComplete(() =>
            // {
            //     if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
            // });
            LeanTween.value(stories[storyIndex + 1], UpdateNextStoryAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() => 
            {
                LeanTween.value(stories[storyIndex], UpdateCurrentStoryAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
                {
                    previousButton.interactable = true;
                    nextButton.interactable = true;

                    if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
                });
            });
        }
    }

    void MoveStoryBoard() => LeanTween.moveLocalX(storyBoard, 2000.0f, 0.3f).setOnComplete(() => 
    {
        LeanTween.value(previousButton.gameObject, UpdatePreviousButtonAlpha, 1.0f, 0.0f, 0.5f);
        isOpen = false;
    }); 

    public void EnableSkipButton() => skipButton.SetActive(true);
    public void DisableSkipButton() => skipButton.SetActive(false);

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.value(stories[storyIndex], UpdateCurrentStoryAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() => 
        {
            nextButton.interactable = true;
            if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
        });
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
