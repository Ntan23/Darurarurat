using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand_ObjIntB : ObjectInteraction_BasicInteraction
{
    [SerializeField] private bool isVaseline;
    [SerializeField] private ObjectControl targetObjectControl;
    [SerializeField] private GameObject button;
    [SerializeField] private ParticleSystem particleFX;
    [SerializeField] private IHaveCream iHaveCream;
    
    protected override void Start() 
    {
        base.Start();
        iHaveCream = targetObjectControl.transform.GetComponent<IHaveCream>();
        iHaveCream.OnGettingCream += iHaveCream_OnGettingCream;
        iHaveCream.OnCreamReady += iHaveCream_OnCreamReady;

        button.SetActive(false);
    }

    private void iHaveCream_OnGettingCream(object sender, EventArgs e)
    {
        GrabCream();
    }

    private void iHaveCream_OnCreamReady(object sender, EventArgs e)
    {
        SetCanInteract(true);
        iHaveCream.OnCreamReady -= iHaveCream_OnCreamReady;
    }

    void OnMouseEnter()
    {
        if(canInteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) button.SetActive(true);
    }

    void OnMouseExit()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) button.SetActive(false);
    }
    public void GrabCream()
    {
        if(isVaseline) animator.Play("Grab Vaseline");
        else animator.Play("Take Rash Cream");
    }

    public void ApplyVaseline()
    {
        button.SetActive(false);
        SetCanInteract(false);
        animator.Play("Apply Vaseline");
        StartCoroutine(ApplyEffect());
    }

    IEnumerator ApplyEffect()
    {
        yield return new WaitForSeconds(0.4f);
        particleFX.Play();
        yield return new WaitForSeconds(1.0f);
        particleFX.Pause();
        yield return new WaitForSeconds(0.2f);
        targetObjectControl.CheckWinCondition(); //Keknya jgn di sini ||Want Change||
    }
}
