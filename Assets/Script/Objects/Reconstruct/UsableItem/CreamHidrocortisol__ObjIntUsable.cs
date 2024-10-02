using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamHidrocortisol__ObjIntUsable : ObjInt_UsableItem_PermanentCap_ChangeCapMesh, IHaveCream
{
    public event EventHandler OnGettingCream;
    public event EventHandler OnCreamReady;
    [Header("I Have Cream")]
    [SerializeField] private GameObject cream;
    [SerializeField] private Vector3 animationPosition;//Beda - For GrabCream
    private Vector3 beforeAnimatePosition;

    public override void DoThingsBasedOnMousePosition()
    {
        if(Input.mousePosition.x < mousePosition.x && !isOpen && capCollider.enabled) OpenCap();

        if(Input.mousePosition.x > mousePosition.x && isOpen && capCollider.enabled) CloseCap();
    }
    
    protected override void ShowInstructionNow()
    {
        

        if(!isOpen)
        {
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        if(isOpen) 
        {
            StartCoroutine(ShowingCapToClose());
        }
        animator.enabled = true;
        
    }
    IEnumerator ShowingCapToClose()
    {
        LeanTween.value(capMesh, UpdateAlpha, 0.0f, 1.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        ChangeCapMeshToNormal();
        instructionArrowParent.SetActive(true);
        instructionArrows[0].SetActive(true);

        canAnimate = true;
    }
    #region Permanent Cap
    
    protected override IEnumerator OpenCapEnumerator()
    {
        objControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Open");
        canAnimate = false;
        yield return new WaitForSeconds(2.1f);
        ChangeCapMeshToTransparent();
        LeanTween.value(capMesh, UpdateAlpha, 1.0f, 0.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;
        
        isOpen = true;
    }

    protected override IEnumerator CloseCapEnumerator()
    {
        objControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Close");
        canAnimate = false;
        yield return new WaitForSeconds(2.2f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = false;
    }

    #endregion
    
    #region GrabCream
    ///summary
    ///    Grab Cream
    ///summary 
    public void GrabCream() => StartCoroutine(GrabCreamAnimation());
    IEnumerator GrabCreamAnimation()
    {
        gm.ChangeIsAnimatingValue(true);
        // playerArmAnimator.Play("Take Rash Cream");
        OnGettingCream?.Invoke(this, EventArgs.Empty);
        
        yield return new WaitForSeconds(0.1f);
        LeanTween.move(gameObject, animationPosition, 1.0f).setEaseSpring();
        LeanTween.rotateZ(gameObject, -120.0f, 0.5f);
        yield return new WaitForSeconds(2.0f);
        animator.Play("Squeze");
        yield return new WaitForSeconds(0.2f);
        LeanTween.scale(cream, new Vector3(0.1f, 0.1f, 0.1f), 0.8f);
        LeanTween.move(cream, new Vector3(0.95f, 12.5f, 0.6f), 0.8f).setOnComplete(() => cream.SetActive(false));
        yield return new WaitForSeconds(1.0f);
        beforeAnimatePosition = objControl.GetBeforeAnimatePosition();
        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
        LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(1.2f);
        // playerArm.GetComponent<PlayerHand>().ChangeCanInteract();
        OnCreamReady?.Invoke(this, EventArgs.Empty);
        gm.ChangeIsAnimatingValue(false);
    }
    #endregion
}
