using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAidPeel : MonoBehaviour
{
    [SerializeField] private ObjectControl objectControl;

    public void Peel()
    {
        objectControl.ChangeIsAnimatingValue();
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(PeelAnimation());
        StartCoroutine(WaitForRotateAndMove(1.2f));
    }   

    IEnumerator PeelAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
    }

    IEnumerator WaitForRotateAndMove(float time) 
    {
        yield return new WaitForSeconds(time); 
        objectControl.Animate();
    } 
}
