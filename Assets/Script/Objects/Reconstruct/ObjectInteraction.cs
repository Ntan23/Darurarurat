using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interaction with Itself
/// </summary>
public abstract class ObjectInteraction : MonoBehaviour
{
    protected GameManager gm;
    protected Animator animator;
    // protected IEnumerator coroutineSave;
    // protected bool isCorotineActive;
    protected virtual void Start()
    {
        gm = GameManager.instance;
        animator = GetComponent<Animator>();
    }
    
    // protected virtual void StopActiveCoroutine()
    // {
    //     if(isCorotineActive)
    //     {
    //         StopCoroutine(coroutineSave);
    //         isCorotineActive = false;
    //     }
        
    // }
    protected virtual void StopAllCoroutine()
    {
        StopAllCoroutine();
    }
}

