using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wipes__ObjIntUsable : ObjInt_UsableItem_TemporaryWrap
{
    private enum objectType{
        gauzePad, nonAlcoholWipes
    }

    [SerializeField] private objectType type;
    public override void DoThingsBasedOnMousePosition()
    {
        if(Input.mousePosition.x < mousePosition.x && type == objectType.nonAlcoholWipes) OpenWrapping();

        if(Input.mousePosition.x > mousePosition.x && type == objectType.gauzePad) OpenWrapping();
    }

    protected override void ShowInstructionNow()
    {
        if(type == objectType.nonAlcoholWipes) LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        if(type == objectType.gauzePad) LeanTween.rotateX(gameObject, -90.0f, 0.3f);
        StartCoroutine(ShowInstructionEnumerator());
        
    }
    IEnumerator ShowInstructionEnumerator()
    {
        yield return new WaitForSeconds(0.2f);

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
        isOpen = true;
    }

}
