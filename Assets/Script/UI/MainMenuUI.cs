using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // [SerializeField] private GameObject gameTitle;
    // [SerializeField] private GameObject clipoard;
    // [SerializeField] private GameObject settingsUI;
    // [SerializeField] private GameObject instructionUI;\
    private Animator animator;
    private bool isAnimating;
    private bool isOpen;
    private ScenesManager scenesManager;

    void Start() 
    {
        scenesManager = ScenesManager.instance;
        animator = GetComponent<Animator>();
    }

    private void OnMouseDown()
    {
        if(!isOpen && !isAnimating) 
        {
            isOpen = true;
            isAnimating = true;
            LeanTween.moveX(gameObject, 0.0f, 0.5f).setEaseSpring().setOnComplete(() =>
            {
                animator.Play("OpenBook");
            });
        }
    }

    public void OpenSettings() 
    { 
        if(!isAnimating) 
        {
            isAnimating = true;
            animator.Play("ToSettings");
        }
            //LeanTween.moveLocalX(settingsUI, 0.0f, 0.8f).setEaseSpring();
    }

    public void CloseSettings() 
    {
        if(!isAnimating)
        {
            isAnimating = true;
            animator.Play("SettingsToMenu");
        }
        //LeanTween.moveLocalX(settingsUI, 970.0f, 0.8f).setEaseSpring();
    }

    public void OpenInstructionUI() 
    {
        if(!isAnimating)
        {
            isAnimating = true;
            animator.Play("ToInstruction"); 
        }
        //LeanTween.moveLocalX(instructionUI, 0.0f, 0.8f).setEaseSpring();
    }

    public void CloseInstructionUI() 
    {
        if(!isAnimating)
        {
            isAnimating = true;
            animator.Play("InstructionToMenu"); 
        }
        //LeanTween.moveLocalX(instructionUI, 970.0f, 0.8f).setEaseSpring();
    }

    public void QuitGame()
    {
        if(!isAnimating)
        {
            isAnimating = true;
            CloseMainMenuUI();
            scenesManager.QuitGame();
        }
    }

    // IEnumerator StartAnimation()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     LeanTween.moveLocalX(gameTitle, -725.0f, 0.8f).setEaseSpring();
    //     //LeanTween.moveLocalX(clipoard, 527.0f, 0.8f).setEaseSpring();
    // }

    public void CloseMainMenuUI()
    {
        // LeanTween.moveLocalX(gameTitle, -1230.0f, 0.8f).setEaseSpring();
        // LeanTween.moveLocalX(clipoard, 1496.0f, 0.8f).setEaseSpring();
        animator.Play("CloseBook");
    }

    public void SetBackIsAnimating() => isAnimating = false;
}
