using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] private GameObject startStoryBoard;
    [SerializeField] private GameObject endStoryboard;
    [SerializeField] private int cutStoryIndex;
    private int storyIndex;
    private ScenesManager sm;

    void Start() => sm =ScenesManager.instance;

    public void NextStory()
    {
        storyIndex++;

        if(storyIndex == cutStoryIndex) 
        {
            startStoryBoard.SetActive(false);
            MoveStoryBoard();
        }
        else if(storyIndex < stories.Length) StartCoroutine(PlayNextStoryAnimation());
    }

    public void PreviousStory() => StartCoroutine(PlayPreviousStoryAnimation());
    
    public void SkipStory()
    {
        storyIndex++;

        if(storyIndex <= cutStoryIndex) 
        {
            startStoryBoard.SetActive(false);
            storyIndex = cutStoryIndex;
            MoveStoryBoard();
        }
        else if(storyIndex > cutStoryIndex) sm.GoToNextScene();
    }

    public void ShowEndStory()
    {
        startStoryBoard.SetActive(false);
        endStoryboard.SetActive(true);
        LeanTween.moveLocalX(storyBoard, 0.0f, 0.5f).setEaseSpring();
    }

    IEnumerator PlayNextStoryAnimation()
    {
        LeanTween.moveLocalX(stories[storyIndex - 1], -1924.0f, 0.5f).setEaseSpring();
        LeanTween.moveLocalX(stories[storyIndex], 0, 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.5f);
        if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
    }

    IEnumerator PlayPreviousStoryAnimation()
    {
        storyIndex--;
        LeanTween.moveLocalX(stories[storyIndex + 1], 1924.0f, 0.5f).setEaseSpring();
        LeanTween.moveLocalX(stories[storyIndex], 0.0f, 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.5f);
        if(stories[storyIndex].GetComponent<StorySFX>() != null) stories[storyIndex].GetComponent<StorySFX>().PlaySFX();
    }

    void MoveStoryBoard() => LeanTween.moveLocalX(storyBoard, 1924.0f, 0.3f); 
}
