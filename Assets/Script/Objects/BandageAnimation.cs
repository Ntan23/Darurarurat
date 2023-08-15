using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private float mouseDistanceY;
    private bool canAnimate;
    private bool wrapMode;
    private ObjectControl objectControl;
    private Animator animator;
    [SerializeField] private Animator feetAnimator;
    [SerializeField] private GameObject instructionArrow;
    private GameManager gm;
    private AudioManager am;
    
    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();

        instructionArrow.SetActive(false);
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
        instructionArrow.SetActive(true);

        animator.enabled = true;
        canAnimate = true; 
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        mouseDistanceY = Input.mousePosition.y - mousePosition.y;

        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x && Mathf.Abs(mouseDistanceY) <= 15.0f && Mathf.Abs(Vector3.Distance(Input.mousePosition, mousePosition)) >= 80.0f && !wrapMode) StartCoroutine(PlayAnimation());

            if(Input.mousePosition.x > mousePosition.x && Mathf.Abs(mouseDistanceY) <= 15.0f && Mathf.Abs(Vector3.Distance(Input.mousePosition, mousePosition)) >= 80.0f && wrapMode) 
            {

            }
        }
    }

    IEnumerator PlayAnimation()
    {
        instructionArrow.SetActive(false);
        
        //am.PlayTearPaperSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objectControl.AfterAnimate();
        canAnimate = false;
    }

    public void WrapMode()
    {
        LeanTween.move(gameObject, new Vector3(4.3f, 6.37f, 4.15f), 0.5f).setOnComplete(() =>
        {
            LeanTween.rotate(gameObject, new Vector3(230.0f, 0.0f, -90.0f), 0.3f).setOnComplete(() =>
            {
                instructionArrow.SetActive(true);
                canAnimate = true;
                wrapMode = true;
            });
        });
    }
}
