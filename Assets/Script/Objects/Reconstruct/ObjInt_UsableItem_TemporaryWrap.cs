using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjInt_UsableItem_TemporaryWrap : InteractableObj_UsableItem, IHaveTemporaryWrapping
{
    /// <summary>
    /// Do this if you want the item to show instruction to open wrapping
    /// </summary>
    public override void DoShowInstruction()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        ShowInstruction();
    }
    public abstract void OpenWrapping();
}
