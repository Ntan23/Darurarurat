using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditText;
    [SerializeField] private float textTimeToReachTheEnd;
    private bool canScroll;

    void Update()
    {
        if(canScroll) StartCoroutine(ScrollAnimation());
    }

    public void OpenCredits()
    {
        LeanTween.moveLocalX(gameObject, 0.0f, 0.8f).setEaseSpring();
        canScroll = true;
    }

    public void CloseCredits() 
    {
        LeanTween.cancel(creditText); 
        LeanTween.moveLocalX(gameObject, 2075.0f, 0.8f).setEaseSpring();
        creditText.transform.localPosition = new Vector3(0.0f, -1166.0f, 0.0f);
        canScroll = false;
    }

    IEnumerator ScrollAnimation()
    {
        canScroll = false;
        LeanTween.moveLocalY(creditText, 1166.0f, textTimeToReachTheEnd);
        yield return new WaitForSeconds(textTimeToReachTheEnd + 0.1f);
        creditText.transform.localPosition = new Vector3(0.0f, -1166.0f, 0.0f);
        yield return new WaitForSeconds(0.1f);
        canScroll = true;
    }
}

