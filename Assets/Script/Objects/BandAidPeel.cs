using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAidPeel : MonoBehaviour
{
    
    private Vector3 mousePosition;
    private bool canAnimate;
    private bool isLeftPeeled;
    private bool isRightPeeled;
    [SerializeField] private ObjectControl objectControl;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject[] bandAidPeels;

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) mousePosition = Input.mousePosition;

        if(Input.GetMouseButtonUp(0) && canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x)
            {
                if(isRightPeeled) bandAidPeels[1].SetActive(false);

                animator.Play("Left Peel");
                canAnimate = false;
                StartCoroutine(Wait("Left"));
            }
            else if(Input.mousePosition.x > mousePosition.x)
            {
                if(isLeftPeeled) bandAidPeels[0].SetActive(false);

                animator.Play("Right Peel");
                canAnimate = false;
                StartCoroutine(Wait("Right"));
            }
            else return;
        }

        if(isLeftPeeled && isRightPeeled) StartCoroutine(AfterAnimate());
        else return;
    }

    public void Peel()
    {
        objectControl.SetBeforeAnimatePosition();
        objectControl.ChangeIsAnimatingValue();
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(PeelAnimation());
    }   

    IEnumerator PeelAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        // objectControl.Animate();
        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator Wait(string direction)
    {
        yield return new WaitForSeconds(1.0f);
        if(direction == "Left") 
        {
            isLeftPeeled = true;
            bandAidPeels[0].SetActive(false);
        }
        if(direction == "Right") 
        {
            isRightPeeled = true;
            bandAidPeels[1].SetActive(false);
        }
        canAnimate = true;
    }

    IEnumerator AfterAnimate()
    {
        yield return new WaitForSeconds(0.2f);
        objectControl.AfterAnimate();
        this.enabled = false;
    }
}
