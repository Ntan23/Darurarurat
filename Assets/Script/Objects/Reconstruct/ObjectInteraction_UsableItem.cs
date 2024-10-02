using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteraction_UsableItem : ObjectInteraction, IGetMousePositionDistance, IHaveInstruction
{
    protected AudioManager am;
    protected ObjectControl objControl;
    /// <summary>
    /// IGetMousePosition
    /// </summary>
    protected bool canAnimate;
    public bool CanAnimate{get {return canAnimate;}}
    protected enum MouseDistance_Direction
    {
        Horizontal, Vertical
    }
    [SerializeField]protected MouseDistance_Direction mouseCheckDirection;
    protected Vector3 mousePosition;
    protected float mouseDistance;
    [SerializeField]protected float mouseDistanceTargetMinimal = 50f;

    /// <summary>
    /// Checking if the item(cap or wrapper) is already open or not 
    /// </summary>
    protected bool isOpen;
    public bool IsOpen{get {return isOpen;}}

    /// <summary>
    /// I Have Instruction
    /// </summary>
    [Header("Instructions")]
    [SerializeField] protected GameObject[] instructionArrows;
    [SerializeField] protected GameObject instructionArrowParent;
    protected override void Start() 
    {
        base.Start();

        am = AudioManager.instance;
        objControl = GetComponent<ObjectControl>();

        HideAllInstruction();
    }
    #region OnMouse
    private void OnMouseDown() => CheckMousePos();
    private void OnMouseUp() 
    {
        CheckMouseDistance_StartToEnd();
        if(canAnimate)if(Mathf.Abs(mouseDistance) >= mouseDistanceTargetMinimal)DoThingsBasedOnMousePosition();
        
    }
    #endregion

    #region MousePosition
    /// <summary>
    /// Check Distance secara horizontal atau x; atau vertikal atau y
    /// </summary>
    public void CheckMouseDistance_StartToEnd()
    {
        if(mouseCheckDirection == MouseDistance_Direction.Horizontal) CheckMouseDistance_StartToEnd_Horizontal();
        else if(mouseCheckDirection == MouseDistance_Direction.Vertical) CheckMouseDistance_StartToEnd_Vertical();
    }
    public virtual void CheckMouseDistance_StartToEnd_Horizontal()
    {
        mouseDistance = Input.mousePosition.x - mousePosition.x;
    }
    public virtual void CheckMouseDistance_StartToEnd_Vertical()
    {
        mouseDistance = Input.mousePosition.x - mousePosition.y;
    }

    public virtual void CheckMousePos()
    {
        mousePosition = Input.mousePosition;
    }
    public abstract void DoThingsBasedOnMousePosition();
    #endregion
    #region  Instructions

    public abstract void DoShowInstruction();

    public abstract void ShowInstruction();
    public void HideAllInstruction()
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);
        instructionArrowParent.SetActive(false);
    }
    #endregion

}
