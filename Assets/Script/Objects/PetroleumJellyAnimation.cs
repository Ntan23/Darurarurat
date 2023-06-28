using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJellyAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool canAnimate;
    private bool isOpen;
    private Animator animator;
    private Animator playerHandAnimator;
    private Collider objCollider;
    [SerializeField] private Collider capCollider;
    [SerializeField] private Vector3 animationPosition;
    private Vector3 beforeAnimatePosition;
    [Header("Player Hand")]
    [SerializeField] private GameObject playerHand;
    [SerializeField] private float targetZPosition;
    private ObjectControl objControl;
    private GameManager gm;

    void Start() 
    {
        animator = GetComponent<Animator>();
        objCollider = GetComponent<Collider>();
        objControl = GetComponent<ObjectControl>();
        playerHandAnimator = playerHand.GetComponent<Animator>();

        gm = GameManager.instance;
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;
    
    void OnMouseUp()
    {
        if(canAnimate)
        {
            if(Input.mousePosition.y > mousePosition.y && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

            if(Input.mousePosition.y < mousePosition.y && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
        }   
    }

    public void Open()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        objCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation());
    }

    public void Close()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        objCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation());
    }

    public void PlayAnimation() => StartCoroutine(GrabVaselineAnimation());
    
    IEnumerator OpenMoveRotateAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, -60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator OpenCap()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(1.2f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = true;
        canAnimate = false;
    }

    IEnumerator CloseCap()
    {
        animator.Play("Close");
        yield return new WaitForSeconds(1.2f);
        objControl.AfterAnimate();
        objCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = false;
        canAnimate = false;
    }

    IEnumerator GrabVaselineAnimation()
    {
        LeanTween.move(gameObject, animationPosition, 0.8f).setEaseSpring();
        yield return new WaitForSeconds(1.0f);
        LeanTween.moveZ(playerHand, targetZPosition, 0.5f);
        yield return new WaitForSeconds(0.3f);
        playerHandAnimator.Play("Grab Vaseline");
        yield return new WaitForSeconds(2.0f);
        beforeAnimatePosition = objControl.GetBeforeAnimatePosition();
        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
    }
}
