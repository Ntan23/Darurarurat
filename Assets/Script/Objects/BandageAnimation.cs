using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private float mouseDistanceX;
    private int count;
    private bool canAnimate;
    private bool wrapMode;
    private ObjectControl objectControl;
    private Animator animator;
    [SerializeField] private Animator feetAnimator;
    [SerializeField] private GameObject[] instructionArrows;
    [SerializeField] private GameObject arrowParent;
    private GameManager gm;
    private AudioManager am;
    
    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();

        foreach(GameObject go in instructionArrows) go.SetActive(false);

        arrowParent.SetActive(false);
    }

    public void Open()
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(OpenAnimation());
    } 

    IEnumerator OpenAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        arrowParent.SetActive(true);
        instructionArrows[0].SetActive(true);

        animator.enabled = true;
        canAnimate = true; 
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;

        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && !wrapMode) StartCoroutine(PlayAnimation());

            if(Input.mousePosition.x > mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && wrapMode) StartCoroutine(WrapAnimation());
        }
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
