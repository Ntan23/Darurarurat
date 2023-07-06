using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipesAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private bool canAnimate;
    private ObjectControl objectControl;
    private GameManager gm;
    private Animator animator;
    [SerializeField] private GameObject instructionArrow;

    void Start()
    {
        gm = GameManager.instance;

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
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);

        instructionArrow.SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x) StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        instructionArrow.SetActive(false);
        
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objectControl.AfterAnimate();
        canAnimate = false;
    }
}