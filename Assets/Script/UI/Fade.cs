using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    private GameManager gm;
    private DialogueManager dm;
    private StoryManager sm;
    private TimeManager tm;
    [SerializeField] private bool needToStopTime;

    void Start()
    {
        gm = GameManager.instance;
        dm = DialogueManager.instance;
        sm = StoryManager.instance;
        tm = TimeManager.instance;
    }

    void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void FadeIn() 
    {
        StartCoroutine(FadeInAnimation());
    }

    public void FadeOut() 
    {
        if(gm != null) gm.ChangeCanPauseValue(false);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f);
    }

    IEnumerator FadeInAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        if(tm != null) tm.canStart = false;     

        if(gm != null)
        {
            if(!gm.CanSkip() && dm != null) dm.DeactivateSkipButton();
            else if(gm.CanSkip() && dm != null) dm.DisableSkipButton();

            if(!gm.CanSkip() && sm != null) sm.DisableSkipButton();
            else if(gm.CanSkip() && sm != null) sm.EnableSkipButton();
        }

        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 1.5f).setOnComplete(() => 
        {
            if(tm != null) 
            {
                if(!needToStopTime) tm.canStart = true;
                else if(needToStopTime && PlayerPrefs.GetInt("TipsShowed") == 1 && PlayerPrefs.GetInt("TipsReception") == 1 && SceneManager.GetActiveScene().name == "Level 1") tm.canStart = true;
                else if(needToStopTime && PlayerPrefs.GetInt("IsFirstimePlaying", 0) == 1 && SceneManager.GetActiveScene().name == "PatientReception") tm.canStart = true;
                else tm.canStart = false;
            }
            if(gm != null) gm.ChangeCanPauseValue(true);
            if(sm != null) GetComponent<CanvasGroup>().blocksRaycasts = false;
        });  

        yield return new WaitForSeconds(2.0f);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(gm != null) 
        {
            if(gm.CanSkip() && dm != null) dm.EnableSkipButton();
        }
    }
}
