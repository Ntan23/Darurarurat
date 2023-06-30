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

    public void GoToNextScene()
    {
        StartCoroutine(GoToNextSceneAnimation());
    }

    IEnumerator GoToNextSceneAnimation()
    {
        fade.FadeOut();
        yield return new WaitForSeconds(1.1f);
        if(gm != null)
        {
            if(gm.GetLevelIndex() < 2) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else if(gm.GetLevelIndex() == 2) SceneManager.LoadScene(0);
        }
        else if(gm == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
