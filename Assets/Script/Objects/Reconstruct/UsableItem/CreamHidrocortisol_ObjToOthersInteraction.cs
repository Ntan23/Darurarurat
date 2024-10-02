using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamHidrocortisol_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private CreamHidrocortisol__ObjIntUsable creamHidrocortisol__ObjIntUsable;
    private void Start() 
    {
        creamHidrocortisol__ObjIntUsable = GetComponent<CreamHidrocortisol__ObjIntUsable>();
    }
    public override void InteractionWithPatient()
    {
        GetCream();
    }
    public void GetCream()
    {
        creamHidrocortisol__ObjIntUsable.GrabCream();
    }
}
