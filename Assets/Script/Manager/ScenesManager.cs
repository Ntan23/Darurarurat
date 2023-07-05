using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    #region Singleton
    public static ScenesManager instance;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    #endregion

    [SerializeField] private Fade fade;
    private GameManager gm;
    
    void Start() 
    {
        gm = GameManager.instance;

        fade.FadeIn();
    }

    public void GoToNextScene() => StartCoroutine(GoToNextSceneAnimation());
    public void GoToTargetScene(string sceneName) => StartCoroutine(GoToTargetSceneAnimation(sceneName));
    public void QuitGame() => StartCoroutine(QuitGameAnimation());

    IEnumerator GoToNextSceneAnimation()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.1f);
        if(gm != null)
        {
            if(gm.GetLevelIndex() < 4) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else if(gm.GetLevelIndex() == 4) SceneManager.LoadScene(0);
        }
        else if(gm == null) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator GoToTargetSceneAnimation(string sceneName)
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator QuitGameAnimation()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.1f);
        Debug.Log("Quit");
        Application.Quit();
    }
}
