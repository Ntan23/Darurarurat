using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJelly_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private CreamHidrocortisol__ObjIntUsable creamHidrocortisol__ObjIntUsable;
    public override void InteractionWithPatient()
    {
        GetCream();
    }
    public void GetCream()
    {
        creamHidrocortisol__ObjIntUsable.GrabCream();
    }
}
