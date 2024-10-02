using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteraction_BasicInteraction : ObjectInteraction
{
    protected bool canInteract;
    public void SetCanInteract(bool change) => canInteract = change;
}
