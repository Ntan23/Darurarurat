using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjInt_UsableItem_PermanentCap : ObjectInteraction_UsableItem, IHavePermanentCap
{
    [Header("Permanent Cap Collider")]
    protected Collider objCollider;  
    [SerializeField] protected Collider capCollider;

    /// <summary>
    /// Save Courotine so you can stop
    /// </summary>
    

    [Header("Open Cap Variable")]
    [SerializeField]protected Vector3 targetPosition_OpenClose = new Vector3(0.0f, 8.0f, 2.0f);
    [SerializeField]protected float rotateXPosition_OpenClose = -60.0f;
    [SerializeField]protected float moveToTarget_Duration = 0.5f;
    protected float moveWait_Duration = 0.8f;
    [SerializeField]protected float rotateToTarget_Duration = 0.3f;
    protected float rotateWait_Duration = 0.5f;
    
    protected override void Start() 
    {
        base.Start();
        
        objCollider = GetComponent<Collider>();//Beda - Sama Tutup
    }

    //Ini yang buat di button utk animasi dsb
    public override void DoShowInstruction()
    {   
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        objCollider.enabled = false;
        capCollider.enabled = true;
        ShowInstruction();
    }
    public override void ShowInstruction()
    {
        // coroutineSave = OpenMoveRotateAnimation();
        StartCoroutine(OpenMoveRotateAnimation());
    }
    protected IEnumerator OpenMoveRotateAnimation()
    {
        LeanTween.move(gameObject, targetPosition_OpenClose, moveToTarget_Duration).setEaseSpring();
        yield return new WaitForSeconds(moveWait_Duration);
        LeanTween.rotateX(gameObject, rotateXPosition_OpenClose, rotateToTarget_Duration);
        yield return new WaitForSeconds(rotateWait_Duration);
        ShowInstructionNow();
    }
    protected abstract void ShowInstructionNow();

    public virtual void OpenCap()
    {
        HideAllInstruction();
        StartCoroutine(OpenCapEnumerator());
    }
    protected abstract IEnumerator OpenCapEnumerator();

    public virtual void CloseCap()
    {
        HideAllInstruction();
        StartCoroutine(CloseCapEnumerator());
    }
    protected abstract IEnumerator CloseCapEnumerator();
}

