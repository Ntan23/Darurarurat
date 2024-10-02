using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInteraction_Complex : VisualInteraction_Basic
{
    private GameManager gm;
    [SerializeField]private bool IsHavePermanentCap;
    
    // [SerializeField]private ObjectControl.Object objType;
    private ObjectInteraction_UsableItem objInteraction_UsableItem;
    private void Start() 
    {
        gm = GameManager.instance;
        objInteraction_UsableItem = GetComponent<ObjectInteraction_UsableItem>();
    }
    protected override void HideHover(int idx)
    {
        if(IsHavePermanentCap)
        {
            if(gm.GetIsAnimating()) 
            {
                if(!objInteraction_UsableItem.IsOpen) base.HideHover(idx);
                else if(objInteraction_UsableItem.IsOpen) 
                {
                    if(idx == 0) base.HideHover(idx);   
                    if(idx == 1 && objInteraction_UsableItem.CanAnimate) base.HideHover(idx);
                }
            }
            if(!gm.GetIsAnimating())
            {
                if(objInteraction_UsableItem.IsOpen)
                {
                    if(idx == 0) base.HideHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!objInteraction_UsableItem.IsOpen) base.HideHover(idx);
            }
        }
        else 
        {
            if(!objInteraction_UsableItem.IsOpen)
            {
                // if(idx == 0) continue;
                if(idx > 0) base.HideHover(idx);
            }
            else if(objInteraction_UsableItem.IsOpen) 
            {
                if(idx == 0) base.HideHover(idx);
                else shouldBreak = true; //Break for loop
            }
        }
    }

    protected override void ShowHover(int idx)
    {
        if(IsHavePermanentCap) 
        {
            if(gm.GetIsAnimating())
            {
                if(objInteraction_UsableItem.CanAnimate)
                {
                    if(idx == 1) base.ShowHover(idx);
                }
            }
            else if(!gm.GetIsAnimating())
            {
                if(objInteraction_UsableItem.IsOpen)
                {
                    if(idx == 0) base.ShowHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!objInteraction_UsableItem.IsOpen) base.ShowHover(idx);
            }
        }
        else
        {
            if(!objInteraction_UsableItem.IsOpen)
            {
                // if(idx == 0) continue;
                if(idx > 0) base.ShowHover(idx);
            }
            else if(objInteraction_UsableItem.IsOpen) 
            {
                if(idx == 0) base.ShowHover(idx);
                else shouldBreak = true; //Break for loop
            }
        }
        
    }
}
