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
    [SerializeField] private bool is2D;
    private GameManager gm;
    
    void Start() 
    {
        gm = GameManager.instance;

        fade.FadeIn();

        if(is2D) QualitySettings.SetQualityLevel(3);
        else if(!is2D) QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel", 2));
    }

    public void Play()
    {
        if(PlayerPrefs.GetInt("IsTeaTime", 0) == 1) GoToTargetScene("TeaTime");
        if(PlayerPrefs.GetInt("IsUpgrade", 0) == 1) GoToTargetScene("UpgradeScene");
        if(PlayerPrefs.GetInt("IsTeaTime", 0) == 0 && PlayerPrefs.GetInt("IsUpgrade", 0) == 0) GoToNextScene();
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
            if(gm.GetLevelIndex() < 5) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else if(gm.GetLevelIndex() == 5) SceneManager.LoadScene(0);
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

    public void StartGame()
    {
        PlayerPrefs.SetInt("IsFirstimePlaying", 0); // Matikan ini nanti
        if(PlayerPrefs.GetInt("IsFirstimePlaying", 0) == 0)
        {
            // PlayerPrefs.SetInt("IsFirstimePlaying", 1);
            PlayerPrefs.SetString("ShowComicTitle", "OpeningGameComic");
            PlayerPrefs.SetString("NextSceneNameAfterComic", "PatientReception");
            GoToTargetScene("Comic Dialogue Scene");
        }
        else
        {
            GoToNextScene();
        }
    }
}
