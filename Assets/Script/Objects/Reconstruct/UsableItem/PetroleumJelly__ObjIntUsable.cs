using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJelly__ObjIntUsable : ObjInt_UsableItem_PermanentCap, IHaveCream
{

    public event EventHandler OnGettingCream;
    public event EventHandler OnCreamReady;
    [Header("I Have Cream")]
    [SerializeField] private Vector3 animationPosition;
    private Vector3 beforeAnimatePosition;


    public override void DoThingsBasedOnMousePosition()
    {
        if(Input.mousePosition.y > mousePosition.y && !isOpen && capCollider.enabled) OpenCap();

        if(Input.mousePosition.y < mousePosition.y && isOpen && capCollider.enabled) CloseCap();
    }
    
    protected override void ShowInstructionNow()
    {
        instructionArrowParent.SetActive(true);

        if(isOpen) instructionArrows[1].SetActive(true);
        else if(!isOpen) instructionArrows[0].SetActive(true);

        animator.enabled = true;
        canAnimate = true;
        
    }

    #region Permanent Cap
    
    protected override IEnumerator OpenCapEnumerator()
    {
        am.PlayOpenVaselineSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.2f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = true;
        canAnimate = false;
    }

    protected override IEnumerator CloseCapEnumerator()
    {
        am.PlayCloseVaselineSFX();
        animator.Play("Close");
        yield return new WaitForSeconds(1.2f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = false;
        canAnimate = false;
    }

    #endregion
    
    #region GrabCream
    ///summary
    ///    Grab Cream
    ///summary 
    public void GrabCream() => StartCoroutine(GrabVaselineAnimation());

    IEnumerator GrabVaselineAnimation()
    {
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, animationPosition, 0.8f).setEaseSpring();
        LeanTween.rotate(gameObject, new Vector3(180.0f, 0.0f, -180.0f), 0.3f);
        yield return new WaitForSeconds(0.8f);
        // playerHandAnimator.Play("Grab Vaseline");
        OnGettingCream?.Invoke(this, EventArgs.Empty);

        yield return new WaitForSeconds(3.1f);
        // playerHand.GetComponent<PlayerHand>().ChangeCanInteract();
        OnCreamReady?.Invoke(this, EventArgs.Empty);
        beforeAnimatePosition = objControl.GetBeforeAnimatePosition();
        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
        LeanTween.rotateX(gameObject, 270.0f, 0.3f);
        gm.ChangeIsAnimatingValue(false);
    }
    #endregion
}
