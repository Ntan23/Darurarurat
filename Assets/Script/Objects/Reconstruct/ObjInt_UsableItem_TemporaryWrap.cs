using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjInt_UsableItem_TemporaryWrap : ObjectInteraction_UsableItem, IHaveTemporaryWrapping
{
    /// <summary>
    /// Do this if you want the item to show instruction to open wrapping
    /// </summary>
    
    [Header("Open Cap Variable")]
    [SerializeField]protected Vector3 targetPosition_OpenClose = new Vector3(0.0f, 8.0f, 2.0f);
    [SerializeField]protected float moveToTarget_Duration = 0.5f;
    protected float moveWait_Duration = 0.6f;
    public override void DoShowInstruction()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        ShowInstruction();
    }    
    public override void ShowInstruction()
    {
        // coroutineSave = OpenMoveRotateAnimation();
        StartCoroutine(OpenAnimation());
    }
    protected IEnumerator OpenAnimation()
    {
        LeanTween.move(gameObject, targetPosition_OpenClose, moveToTarget_Duration).setEaseSpring();
        yield return new WaitForSeconds(moveToTarget_Duration);
        ShowInstructionNow();
    }
    protected abstract void ShowInstructionNow();
    public abstract void OpenWrapping();
}
