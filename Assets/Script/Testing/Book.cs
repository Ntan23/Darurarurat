using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;

    public void NextPage(int index)
    {
        Debug.Log("Next");
        LeanTween.rotateZ(pages[index], 180.0f, 0.8f).setEaseLinear();
    }

    public void PreviousPage(int index)
    {
        Debug.Log("Previous");
        LeanTween.rotateZ(pages[index], 1.0f, 0.8f).setEaseLinear();
    }
}                       
