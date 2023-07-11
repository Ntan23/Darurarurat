using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUI : MonoBehaviour
{
    private bool isOpen;
    [SerializeField] private Button button;
    private GameManager gm;

    void Start() => gm = GameManager.instance;

    public void Open()
    {
        isOpen = true;
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
        isOpen = false;
    }

    public void PeekAnimation() 
    {
        if(!isOpen) LeanTween.moveLocalY(gameObject, -950.0f, 0.5f).setEaseSpring();
    }

    public void ClosePeekAnimation()
    {
        if(!isOpen) LeanTween.moveLocalY(gameObject, -1000.0f, 0.5f).setEaseSpring();
    }
}
