using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntisepticAnimation : MonoBehaviour
{
    private bool canAnimate;
    private bool isOpen;
    private Vector3 mousePosition;
    private Animator animator;
    private ObjectControl objectControl;
    private GameManager gm;
    [SerializeField] private GameObject cap;
    [SerializeField] private Transform targetParent;
    [SerializeField] private Transform originalParent;
    private Collider objectCollider;
    [SerializeField] private Collider capCollider;

    void Start() 
    {
        gm = GameManager.instance;
        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();
        objectCollider = GetComponent<Collider>();
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        if(canAnimate)
        {
            if(Input.mousePosition.x > mousePosition.x && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

            if(Input.mousePosition.x < mousePosition.x && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
        }
    }

    public void Open()
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        objectCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(true));
    }

    public void Close()
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        objectCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(false));
    }

    IEnumerator OpenMoveRotateAnimation(bool hasCap)
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.3f);

        if(!hasCap) cap.transform.parent = originalParent;

        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator OpenCap()
    {
        animator.Play("Open");
        yield return new WaitForSeconds(1.8f);
        cap.transform.parent = targetParent;
        yield return new WaitForSeconds(0.5f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;
        canAnimate = false;
        isOpen = true;
    }

    IEnumerator CloseCap()
    {
        animator.Play("Close");
        yield return new WaitForSeconds(1.8f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;
        canAnimate = false;
        isOpen = false;
    }
}
