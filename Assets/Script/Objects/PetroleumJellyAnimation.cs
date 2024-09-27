using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumJellyAnimation : MonoBehaviour
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool canAnimate;// Sama
    private ObjectControl objControl;
    private Vector3 mousePosition;// Sama
    private float mouseDistanceY;// Sama
    private bool isOpen; //Another Open open
    private Collider objCollider;// Beda - Sama Tutup
    [SerializeField] private Collider capCollider;// ada tutup tp bedanya di sini ga pake mesh
    [Header("Player Hand")]
    [SerializeField] private Vector3 animationPosition;
    private Vector3 beforeAnimatePosition;
    private Animator playerHandAnimator;// Another Hand
    [SerializeField] private GameObject playerHand;//another hand
    [Header("For Instruction Arrow")]
    [SerializeField] private GameObject[] instructionArrows;// Sama
    [SerializeField] private GameObject arrowParent;// Sama

    void Start() 
    {
        gm = GameManager.instance;// Sama
        am = AudioManager.instance;// Sama

        animator = GetComponent<Animator>();// Sama
        objControl = GetComponent<ObjectControl>();// Sama
        objCollider = GetComponent<Collider>();// Beda
        playerHandAnimator = playerHand.GetComponent<Animator>();// Beda

        arrowParent.SetActive(false);// Sama

        foreach(GameObject go in instructionArrows) go.SetActive(false);// Sama
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;// Sama
    
    void OnMouseUp()// Sama Beda Courotine; Ganti jd Fungsi Kepisah aja ||Want Change||
    {   
        mouseDistanceY = Input.mousePosition.y - mousePosition.y;

        if(canAnimate)// Sam
        {
            if(Mathf.Abs(mouseDistanceY) >= 50.0f)
            {
                if(Input.mousePosition.y > mousePosition.y && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.y < mousePosition.y && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
            }
        }   
    }
    #region OpenCloseCap
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
    #endregion
    
    #region  GrabCream
    public void PlayAnimation() => StartCoroutine(GrabVaselineAnimation());

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
    #endregion
}
