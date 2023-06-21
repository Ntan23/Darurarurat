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
    private Animator animator;
    [SerializeField] private GameObject[] bandAidPeels;
    [SerializeField] private GameObject instructionArrowParent;
    [SerializeField] private GameObject[] instructionArrow;
    private GameManager gm;

    void Start() 
    {
        gm = GameManager.instance;

        animator = GetComponentInChildren<Animator>();

        foreach(GameObject go in instructionArrow)
        {
            go.SetActive(false);
        }
    }

    public void Peel()
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(PeelAnimation());
    }   

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x)
            {
                instructionArrow[0].SetActive(false);

                if(isRightPeeled) bandAidPeels[1].SetActive(false);

                animator.Play("Left Peel");
                canAnimate = false;
                StartCoroutine(Wait("Left"));
            }
            else if(Input.mousePosition.x > mousePosition.x)
            {
                instructionArrow[1].SetActive(false);
                
                if(isLeftPeeled) bandAidPeels[0].SetActive(false);

                animator.Play("Right Peel");
                canAnimate = false;
                StartCoroutine(Wait("Right"));
            }
        }
    }

    IEnumerator PeelAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        // objectControl.Animate();
        foreach(GameObject go in instructionArrow)
        {
            go.SetActive(true);
        }

        instructionArrowParent.transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));

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

        if(isLeftPeeled && isRightPeeled) StartCoroutine(AfterAnimate());
        else yield return null;

        canAnimate = true;
    }

    IEnumerator AfterAnimate()
    {
        yield return new WaitForSeconds(0.2f);
        objectControl.AfterAnimate();
        Destroy(this);
    }
}
