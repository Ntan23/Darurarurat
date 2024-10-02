using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BandAid__ObjIntUsable : ObjInt_UsableItem_TemporaryWrap
{
    private bool isLeftPeeled;// Beda
    private bool isRightPeeled;// Beda
    [SerializeField] private GameObject[] bandAidPeels;// Beda
    protected override void Start() 
    {
        base.Start();
        animator = GetComponentInChildren<Animator>();
    }
    public override void DoThingsBasedOnMousePosition()
    {
        OpenWrapping();
    }

    protected override void ShowInstructionNow()
    {
        StartCoroutine(RotateItem());
    }
    IEnumerator RotateItem()
    {
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        // objectControl.Animate();

        instructionArrowParent.SetActive(true);
        foreach(GameObject go in instructionArrows) go.SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }
    public override void OpenWrapping()
    {
        if(Input.mousePosition.x < mousePosition.x)
        {
            am.PlayPeelSFX();
            instructionArrows[0].SetActive(false);

            if(isRightPeeled) bandAidPeels[1].SetActive(false);

            if(isRightPeeled && isLeftPeeled) instructionArrowParent.SetActive(false);

            animator.Play("Left Peel");
            canAnimate = false;
            StartCoroutine(Wait("Left"));
        }
        else if(Input.mousePosition.x > mousePosition.x)
        {
            am.PlayPeelSFX();
            instructionArrows[1].SetActive(false);
            
            if(isLeftPeeled) bandAidPeels[0].SetActive(false);

            if(isRightPeeled && isLeftPeeled) instructionArrowParent.SetActive(false);

            animator.Play("Right Peel");
            canAnimate = false;
            StartCoroutine(Wait("Right"));
        }
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
        instructionArrowParent.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        objControl.AfterAnimate();
        Destroy(this);
    }
}
