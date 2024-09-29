using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJellyAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool canAnimate;
    private bool isOpen;
    private float mouseDistanceY;
    private Animator animator;
    private Animator playerHandAnimator;
    private Collider objCollider;
    [SerializeField] private Collider capCollider;
    [SerializeField] private Vector3 animationPosition;
    private Vector3 beforeAnimatePosition;
    [Header("Player Hand")]
    [SerializeField] private GameObject playerHand;
    [Header("For Instruction Arrow")]
    [SerializeField] private GameObject[] instructionArrows;
    [SerializeField] private GameObject arrowParent;
    private ObjectControl objControl;
    private GameManager gm;
    private AudioManager am;

    void Start() 
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objCollider = GetComponent<Collider>();
        objControl = GetComponent<ObjectControl>();
        if(playerHand != null) playerHandAnimator = playerHand.GetComponent<Animator>();

        arrowParent.SetActive(false);

        foreach(GameObject go in instructionArrows) go.SetActive(false);
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;
    
    void OnMouseUp()
    {   
        mouseDistanceY = Input.mousePosition.y - mousePosition.y;

        if(canAnimate)
        {
            if(Mathf.Abs(mouseDistanceY) >= 50.0f)
            {
                if(Input.mousePosition.y > mousePosition.y && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.y < mousePosition.y && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
            }
        }   
    }

    public void Open()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        objCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(false));
    }

    public void Close()
    {
        objControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objControl.ChangeIsProcedureFinishedValue();
        objCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(true));
    }

    public void PlayAnimation() => StartCoroutine(GrabVaselineAnimation());
    
    IEnumerator OpenMoveRotateAnimation(bool isOpen)
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, -60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        arrowParent.SetActive(true);

        if(isOpen) instructionArrows[1].SetActive(true);
        else if(!isOpen) instructionArrows[0].SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator OpenCap()
    {
        arrowParent.SetActive(false);

        foreach(GameObject go in instructionArrows) go.SetActive(false);

        am.PlayOpenVaselineSFX();
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
        arrowParent.SetActive(false);

        foreach(GameObject go in instructionArrows) go.SetActive(false);

        am.PlayCloseVaselineSFX();
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
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, animationPosition, 0.8f).setEaseSpring();
        LeanTween.rotate(gameObject, new Vector3(180.0f, 0.0f, -180.0f), 0.3f);
        yield return new WaitForSeconds(0.8f);
        playerHandAnimator.Play("Grab Vaseline");
        yield return new WaitForSeconds(3.1f);
        playerHand.GetComponent<PlayerHand>().ChangeCanInteract();
        beforeAnimatePosition = objControl.GetBeforeAnimatePosition();
        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
        gm.ChangeIsAnimatingValue(false);
    }
}
