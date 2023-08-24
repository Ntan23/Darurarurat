using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditText;
    [SerializeField] private float textTimeToReachTheEnd;

    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;
    
    public void OpenCredits()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        LeanTween.value(gameObject, UpdateAlpha, 0.0f, 1.0f, 0.5f).setOnComplete(() =>
        {
            LeanTween.moveLocalY(creditText, 1666.0f, textTimeToReachTheEnd).setLoopClamp();
        });
    }

    public void CloseCredits() 
    {
        LeanTween.cancel(creditText); 
        // LeanTween.moveLocalX(gameObject, 2075.0f, 0.8f).setEaseSpring();
        LeanTween.value(gameObject, UpdateAlpha, 1.0f, 0.0f, 0.5f).setOnComplete(() =>
        {
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        });
        creditText.transform.localPosition = new Vector3(0.0f, -1666.0f, 0.0f);
    }

    IEnumerator OpenAnimation()
    {
        LeanTween.moveLocalX(gameObject, 0.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.moveLocalY(creditText, 1666.0f, textTimeToReachTheEnd).setLoopClamp();
    }
}

