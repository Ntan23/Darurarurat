using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageAnimation : MonoBehaviour
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool canAnimate;// Sama
    private ObjectControl objectControl;// Sama
    private Vector3 mousePosition;// Sama
    private float mouseDistanceX;// Sama
    private int count;// Beda
    private bool wrapMode;// Beda
    [SerializeField] private Animator feetAnimator;// Beda
    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;// Sama
    [SerializeField] private GameObject arrowParent;// Sama
    
    void Start()
    {
        gm = GameManager.instance;// Sama
        am = AudioManager.instance;// Sama

        animator = GetComponent<Animator>();// Sama
        objectControl = GetComponent<ObjectControl>();// Sama

        ///summary
        ///    Get All Instruction Arrows & Turn it off
        ///summary
        foreach(GameObject go in instructionArrows) go.SetActive(false);// Sama

        arrowParent.SetActive(false);// Sama
    }

    ///summary
    ///    Mouse Input
    ///summary
    void OnMouseDown() => mousePosition = Input.mousePosition;// Sama

    void OnMouseUp()// Sama Beda Courotine; Ganti jd Fungsi Kepisah aja ||Want Change||
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;

        if(canAnimate)// Sama
        {
            if(Input.mousePosition.x < mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && !wrapMode) StartCoroutine(PlayAnimation());

            if(Input.mousePosition.x > mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && wrapMode) StartCoroutine(WrapAnimation());
        }
    }

    ///summary
    ///    Open Bandage
    ///summary
    public void Open()// Sama, Beda Isi ||Want Change||
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(OpenAnimation());
    } 

    IEnumerator OpenAnimation()// Beda
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        arrowParent.SetActive(true);
        instructionArrows[0].SetActive(true);

        animator.enabled = true;
        canAnimate = true; 
    }

    IEnumerator PlayAnimation()
    {
        arrowParent.SetActive(false);
        instructionArrows[0].SetActive(false);
        
        am.PlayTearPaperSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objectControl.AfterAnimate();
        canAnimate = false;
    }
    ///summary
    ///    Wrap Bandage
    ///summary
    public IEnumerator WrapMode()
    {
        gm.ChangeIsAnimatingValue(true);
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        // LeanTween.move(gameObject, new Vector3(4.3f, 6.37f, 4.15f), 0.5f).setOnComplete(() =>
        // {
            LeanTween.rotate(gameObject, new Vector3(230.0f, 0.0f, -90.0f), 0.3f).setOnComplete(() =>
            {
                GetComponent<Collider>().enabled = true;
                arrowParent.SetActive(true);
                instructionArrows[1].SetActive(true);
                instructionArrows[1].transform.rotation = Quaternion.Euler(45f, 0.0f, 0.0f);
                canAnimate = true;
                wrapMode = true;
            });
        // });
    }

    IEnumerator WrapAnimation()
    {
        count++;

        if(count == 1)
        {
            arrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            feetAnimator.Play("BottomWrap");
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            arrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        if(count == 2)
        {
            arrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            feetAnimator.Play("MiddleWrap");
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            arrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }
        if(count == 3)
        {
            arrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            canAnimate = false;
            animator.Play("FadeOut");
            yield return new WaitForSeconds(0.5f);
            feetAnimator.Play("TopWrap");
            yield return new WaitForSeconds(2.3f);
            animator.Play("FadeIn");
            yield return new WaitForSeconds(0.5f);
            arrowParent.SetActive(false);
            instructionArrows[1].SetActive(false);
            objectControl.AfterAnimate();
            objectControl.CheckWinCondition();
        }
    }
}
