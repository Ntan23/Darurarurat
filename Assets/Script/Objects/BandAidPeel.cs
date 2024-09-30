using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAidPeel : MonoBehaviour
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;
    private bool canAnimate;// Sama
    [SerializeField] private ObjectControl objectControl;// Sama ? Why this one is not getcomponent?
    private Vector3 mousePosition;// Sama
    private float mouseDistanceX;// Sama
    private bool isLeftPeeled;// Beda
    private bool isRightPeeled;// Beda
    [SerializeField] private GameObject[] bandAidPeels;// Beda
    [SerializeField] private GameObject[] instructionArrow;// Sama
    [SerializeField] private GameObject arrowParent;// Sama

    void Start() 
    {
        gm = GameManager.instance;// Sama
        am = AudioManager.instance;// Sama

        animator = GetComponentInChildren<Animator>();// Sama
        ///summary
        ///    Get All Instruction Arrows & Turn it off
        ///summary
        foreach(GameObject go in instructionArrow) go.SetActive(false);// Sama
        
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
            if(Mathf.Abs(mouseDistanceX) >= 50.0f)
            {
                if(Input.mousePosition.x < mousePosition.x)
                {
                    am.PlayPeelSFX();
                    instructionArrow[0].SetActive(false);

                    if(isRightPeeled) bandAidPeels[1].SetActive(false);

                    if(isRightPeeled && isLeftPeeled) arrowParent.SetActive(false);

                    animator.Play("Left Peel");
                    canAnimate = false;
                    StartCoroutine(Wait("Left"));
                }
                else if(Input.mousePosition.x > mousePosition.x)
                {
                    am.PlayPeelSFX();
                    instructionArrow[1].SetActive(false);
                    
                    if(isLeftPeeled) bandAidPeels[0].SetActive(false);

                    if(isRightPeeled && isLeftPeeled) arrowParent.SetActive(false);

                    animator.Play("Right Peel");
                    canAnimate = false;
                    StartCoroutine(Wait("Right"));
                }
            }
        }
    }

    ///summary
    ///    Peeling Band Aid
    ///summary
    public void Peel()// Beda - Tp bisa jdiin nama open kali.. ?
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(PeelAnimation());
    }   


    IEnumerator PeelAnimation()// Beda
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);
        // objectControl.Animate();

        arrowParent.SetActive(true);
        foreach(GameObject go in instructionArrow) go.SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator Wait(string direction)// Beda
    {
        yield return new WaitForSeconds(1.0f);
        if(direction == "Left") 
        {
            isLeftPeeled = true;
            bandAidPeels[0].SetActive(false);
        }
        if(direction == "Right") 
        {
            isRightPeeled = true;
            bandAidPeels[1].SetActive(false);
        }

        if(isLeftPeeled && isRightPeeled) StartCoroutine(AfterAnimate());
        else yield return null;

        canAnimate = true;
    }

    IEnumerator AfterAnimate()// Beda
    {
        arrowParent.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        objectControl.AfterAnimate();
        Destroy(this);
    }
}
