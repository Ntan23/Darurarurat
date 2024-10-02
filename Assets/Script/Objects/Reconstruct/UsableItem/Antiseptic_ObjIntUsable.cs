using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antiseptic_ObjIntUsable : ObjInt_UsableItem_PermanentCap_ChangeCapMesh
{
    [Header("Antiseptic Liquid")]
    [SerializeField] private ParticleSystem liquidParticleSystem;

    public override void DoThingsBasedOnMousePosition()
    {
        if(Input.mousePosition.x > mousePosition.x && !isOpen && capCollider.enabled) OpenCap();

        if(Input.mousePosition.x < mousePosition.x && isOpen && capCollider.enabled) CloseCap();
    }
    

    protected override void ShowInstructionNow()
    {
        if(!isOpen)
        {
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        else
        {
            StartCoroutine(CapMeshChange());
        }
        animator.enabled = true;
    }
    IEnumerator CapMeshChange()
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
        yield return new WaitForSeconds(1.8f);
        // cap.transform.parent = targetParent;
        ChangeCapMeshToTransparent();
        LeanTween.value(capMesh, UpdateAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() =>
        {
            objControl.AfterAnimate();
            objCollider.enabled = true;
            capCollider.enabled = false;

            isOpen = true;
        });
    }

    protected override IEnumerator CloseCapEnumerator()
    {
        objControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Close");
        canAnimate = false;
        yield return new WaitForSeconds(1.8f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;
        isOpen = false;
    }
    #endregion
    
    public void PourAntiseptic() => animator.Play("Pour");
    public void PlayLiquidParticleSystem() => liquidParticleSystem.Play();

}
