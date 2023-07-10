using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    [SerializeField] private GameObject creditText;
    [SerializeField] private float textTimeToReachTheEnd;

    public void OpenCredits() => StartCoroutine(OpenAnimation());

    public void CloseCredits() 
    {
        LeanTween.cancel(creditText); 
        LeanTween.moveLocalX(gameObject, 2075.0f, 0.8f).setEaseSpring();
        creditText.transform.localPosition = new Vector3(0.0f, -1266.0f, 0.0f);
    }

    IEnumerator OpenAnimation()
    {
        LeanTween.moveLocalX(gameObject, 0.0f, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.moveLocalY(creditText, 1266.0f, textTimeToReachTheEnd).setLoopClamp();
    }
}

