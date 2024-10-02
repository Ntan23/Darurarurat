using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bandage_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private Bandage__ObjIntUsable bandage__ObjIntUsable;
    [SerializeField]private bool isFeet;//utk sekarang ini dulu penentunya, ntr ubah
    [SerializeField]private Animator patientBodyAnimator;
    private void Start() {
        bandage__ObjIntUsable = GetComponent<Bandage__ObjIntUsable>();
        bandage__ObjIntUsable.OnWrapping += bandage__ObjIntUsable_OnWrapping;
    }

    private void bandage__ObjIntUsable_OnWrapping(object sender, Bandage__ObjIntUsable.WrappingEventArgs e)
    {
        WrapFeetAnimation(e.countWrap);
    }

    public override void InteractionWithPatient()
    {
        StartCoroutine(bandage__ObjIntUsable.WrapMode());
    }

    public void WrapFeetAnimation(int idx)
    {
        if(isFeet)
        {
            if(idx == 1)patientBodyAnimator.Play("BottomWrap");
            else if(idx == 2)patientBodyAnimator.Play("MiddleWrap");
            else if(idx == 3)patientBodyAnimator.Play("TopWrap");
        }
    }
}
