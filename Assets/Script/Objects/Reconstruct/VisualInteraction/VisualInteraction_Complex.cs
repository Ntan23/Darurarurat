using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInteraction_Complex : VisualInteraction_Basic
{
    private GameManager gm;
    // [SerializeField]private bool IsCheckingAnimating;
    
    [SerializeField]private ObjectControl.Object objType;
    private void Start() 
    {
        gm = GameManager.instance;
    }
    protected override void HideHover(int idx)
    {
        if(objType == ObjectControl.Object.Antiseptic) 
        {
            AntisepticAnimation antisepticAnimation = GetComponent<AntisepticAnimation>();

            if(gm.GetIsAnimating()) 
            {
                if(!antisepticAnimation.IsOpen()) base.HideHover(idx);
                else if(antisepticAnimation.IsOpen()) 
                {
                    if(idx == 0) base.HideHover(idx);   
                    if(idx == 1 && antisepticAnimation.CanAnimate()) base.HideHover(idx);
                }
            }
            if(!gm.GetIsAnimating())
            {
                if(antisepticAnimation.IsOpen())
                {
                    if(idx == 0) base.HideHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!antisepticAnimation.IsOpen()) base.HideHover(idx);
            }
        }
        else if(objType == ObjectControl.Object.Cream)
        {
            CreamAnimation creamAnimation = GetComponent<CreamAnimation>();

            if(gm.GetIsAnimating()) 
            {
                if(!creamAnimation.IsOpen()) base.HideHover(idx);
                else if(creamAnimation.IsOpen()) 
                {
                    if(idx == 0) base.HideHover(idx);
                    if(idx == 1 && creamAnimation.CanAnimate()) base.HideHover(idx);
                }
            }
            if(!gm.GetIsAnimating())
            {
                if(creamAnimation.IsOpen())
                {
                    if(idx == 0) base.HideHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!creamAnimation.IsOpen()) base.HideHover(idx);
            }
        }
        else if(objType == ObjectControl.Object.Wipes || objType == ObjectControl.Object.GauzePad)
        {
            WipesAnimation wipesAnimation = GetComponent<WipesAnimation>();

            if(!wipesAnimation.IsOpen())
            {
                // if(idx == 0) continue;
                if(idx > 0) base.HideHover(idx);
            }
            else if(wipesAnimation.IsOpen()) 
            {
                if(idx == 0) base.HideHover(idx);
                else shouldBreak = true; //Break for loop
            }
        }
    }

    protected override void ShowHover(int idx)
    {
        if(objType == ObjectControl.Object.Antiseptic) 
        {
            AntisepticAnimation antisepticAnimation = GetComponent<AntisepticAnimation>();
            
            if(gm.GetIsAnimating())
            {
                if(antisepticAnimation.CanAnimate())
                {
                    // if(idx == 0) continue;
                    if(idx == 1)  
                    {
                        // Debug.Log("Why?" + idx);
                        base.ShowHover(idx);
                    }
                }
            }
            else if(!gm.GetIsAnimating())
            {
                if(antisepticAnimation.IsOpen())
                {
                    if(idx == 0) base.ShowHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!antisepticAnimation.IsOpen())
                {
                    // Debug.Log("Why2?" + idx);
                    base.ShowHover(idx);
                }
            }
        }
        else if(objType == ObjectControl.Object.Cream)
        {
            CreamAnimation creamAnimation = GetComponent<CreamAnimation>();
            
            if(gm.GetIsAnimating())
            {
                if(creamAnimation.CanAnimate())
                {
                    // if(idx == 0) continue;
                    if(idx == 1) base.ShowHover(idx);
                }
            }
            else if(!gm.GetIsAnimating())
            {
                if(creamAnimation.IsOpen())
                {
                    if(idx == 0) base.ShowHover(idx);
                    else if(idx == 1) shouldBreak = true; //Break for loop
                }
                else if(!creamAnimation.IsOpen()) base.ShowHover(idx);
            }
        }
        else if(objType == ObjectControl.Object.Wipes || objType == ObjectControl.Object.GauzePad)
        {
            WipesAnimation wipesAnimation = GetComponent<WipesAnimation>();

            if(!wipesAnimation.IsOpen())
            {
                // if(idx == 0) continue;
                if(idx > 0) base.ShowHover(idx);
            }
            else if(wipesAnimation.IsOpen()) 
            {
                if(idx == 0) base.ShowHover(idx);
                else shouldBreak = true; //Break for loop
            }
        }
        
    }
}
