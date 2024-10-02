using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandage__ObjIntUsable : ObjInt_UsableItem_TemporaryWrap
{
    [Header("Bandage Wrapping")]
    private bool wrapMode;
    private int countWrap;
    public event EventHandler<WrappingEventArgs> OnWrapping;
    public class WrappingEventArgs : EventArgs
    {
        public int countWrap;

        public WrappingEventArgs(int value)
        {
            countWrap = value;
        }
    }
    public override void DoThingsBasedOnMousePosition()
    {
        if(Input.mousePosition.x < mousePosition.x && !wrapMode) OpenWrapping();

        if(Input.mousePosition.x > mousePosition.x && wrapMode) DoBandageWrapping();
    }

    protected override void ShowInstructionNow()
    {
        instructionArrowParent.SetActive(true);
        instructionArrows[0].SetActive(true);

        animator.enabled = true;
        canAnimate = true; 
    }
    public override void OpenWrapping()
    {
        StartCoroutine(OpenWrapperEnumerator());
    }
    IEnumerator OpenWrapperEnumerator()
    {
        instructionArrowParent.SetActive(false);
        instructionArrows[0].SetActive(false);
        
        am.PlayTearPaperSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objControl.AfterAnimate();
        canAnimate = false;
    }
    public void DoBandageWrapping()
    {
        StartCoroutine(WrapAnimation());
    }
    #region WrappingBandage
    public IEnumerator WrapMode()
    {
        gm.ChangeIsAnimatingValue(true);
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        // LeanTween.move(gameObject, new Vector3(4.3f, 6.37f, 4.15f), 0.5f).setOnComplete(() =>
        // {
        LeanTween.rotate(gameObject, new Vector3(230.0f, 0.0f, -90.0f), 0.3f).setOnComplete(() =>
        {
            GetComponent<Collider>().enabled = true;
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            instructionArrows[1].transform.rotation = Quaternion.Euler(45f, 0.0f, 0.0f);
            canAnimate = true;
            wrapMode = true;
        });
        // });
    }

    IEnumerator WrapAnimation()
    {
        countWrap++;

        if(countWrap == 1)
        {
            instructionArrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            // feetAnimator.Play("BottomWrap");
            OnWrapping?.Invoke(this, new WrappingEventArgs(countWrap));
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        if(countWrap == 2)
        {
            instructionArrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            OnWrapping?.Invoke(this, new WrappingEventArgs(countWrap));
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        if(countWrap == 3)
        {
            instructionArrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            OnWrapping?.Invoke(this, new WrappingEventArgs(countWrap));
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            instructionArrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            objControl.AfterAnimate();
            objControl.CheckWinCondition();
        }
        #endregion
    }


}
