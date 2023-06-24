using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNeedUI : MonoBehaviour
{
    // [SerializeField] private Button openCloseButton;
    // private bool isOpen = true;

    // public void OpenClose()
    // {
    //     if(isOpen) 
    //     {
    //         LeanTween.moveLocalY(gameObject, 638.0f, 0.8f).setEaseSpring();
    //         LeanTween.rotateZ(openCloseButton.gameObject, 0.0f, 0.3f);
    //     }
    //     if(!isOpen) 
    //     {
    //         LeanTween.moveLocalY(gameObject, 450.0f, 0.8f).setEaseSpring();
    //         LeanTween.rotateZ(openCloseButton.gameObject, 180.0f, 0.3f);
    //     }

    //     isOpen = !isOpen;
    // }

    [SerializeField] private Button button;
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    public void Open()
    {
        gm.ChangeGameState(true);
        LeanTween.moveLocalY(gameObject, 0.0f, 0.8f).setEaseSpring();
        button.interactable = false;
    }

    public void Close() => StartCoroutine(CloseAnimation());
    
    IEnumerator CloseAnimation()
    {
        LeanTween.moveLocalY(gameObject, -1000.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(1.0f);
        gm.ChangeGameState(false);
        button.interactable = true;
    }
}
