using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimationControl : MonoBehaviour
{
    private Animator objAnimator;
    [SerializeField] private ObjectControl objectControl;
    
    void Start() => objAnimator = GetComponent<Animator>();
        
    public void PlayAnimation() => objAnimator.SetTrigger("Animate");
    public void EnableAnimator() => objAnimator.enabled = true;
    public void DisableAnimator() => objAnimator.enabled = false;
    public void AfterAnimate() => objectControl.AfterAnimate();
}
