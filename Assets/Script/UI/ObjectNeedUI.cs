using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectNeedUI : MonoBehaviour
{
    [SerializeField] private Button openCloseButton;
    private bool isOpen = true;

    public void OpenClose()
    {
        if(isOpen) 
        {
            LeanTween.moveLocalY(gameObject, 638.0f, 0.8f).setEaseSpring();
            LeanTween.rotateZ(openCloseButton.gameObject, 0.0f, 0.3f);
        }
        if(!isOpen) 
        {
            LeanTween.moveLocalY(gameObject, 450.0f, 0.8f).setEaseSpring();
            LeanTween.rotateZ(openCloseButton.gameObject, 180.0f, 0.3f);
        }

        isOpen = !isOpen;
    }
}
