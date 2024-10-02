using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJelly_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private PetroleumJelly__ObjIntUsable petroleumJelly__ObjIntUsable;
    private void Start() 
    {
        petroleumJelly__ObjIntUsable = GetComponent<PetroleumJelly__ObjIntUsable>();
    }
    public override void InteractionWithPatient()
    {
        GetCream();
    }
    public void GetCream()
    {
        petroleumJelly__ObjIntUsable.GrabCream();
    }
}
