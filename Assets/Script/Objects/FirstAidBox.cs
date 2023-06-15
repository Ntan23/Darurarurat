using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidBox : MonoBehaviour
{
    private bool isOpen;
    private bool canBeClicked = true;
    private bool isInTheMiddle;
    private int clickCount;
    private int objectIndex;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    private Vector3 intialPosition;
    private Vector3 initialRotation;
    private Animator animator;
    private BoxCollider boxCollider;
    private GameManager gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();

        intialPosition.x = -14.0f;
        intialPosition.y = targetPosition.y;
        intialPosition.z = transform.position.z;

        initialRotation = transform.rotation.eulerAngles;
    }

    void OnMouseDown()
    {
        if(!isOpen && canBeClicked)
        {
            clickCount++;

            canBeClicked = false;
            if(clickCount == 1)
            {
                isInTheMiddle = true;
                LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring();
                LeanTween.rotate(gameObject, targetRotation, 0.5f);
                StartCoroutine(Wait(0.6f));
            }
            if(clickCount == 2)
            {
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.4f));
            } 
            if(clickCount == 3)
            {
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.4f));
            }
            if(clickCount == 4) 
            {
                StartCoroutine(DelayAnimation(1.4f));
                StartCoroutine(Wait(1.6f));
            }
        }

        if(isOpen && !isInTheMiddle && canBeClicked) 
        {
            LeanTween.move(gameObject, targetPosition, 0.5f);

            // if(gm.objects.Length > 1 && objectIndex < gm.objects.Length) 
            // {
            //     gm.objects[objectIndex].GetComponent<ObjectControl>().EnableCollider();
            //     if(objectIndex < gm.objects.Length) objectIndex++;
            // }
            
            StartCoroutine(WaitTheBox(0.8f, false));
        }
    }

    public void MoveBox()
    {
        LeanTween.move(gameObject, intialPosition, 0.5f);
        StartCoroutine(WaitTheBox(0.5f, true));
    }

    public bool IsInTheMiddle()
    {
        return isInTheMiddle;
    }

    public void SetCanBeClicked(bool canBeClick) => canBeClicked = canBeClick;

    IEnumerator DelayAnimation(float time)
    {
        animator.enabled = true;
        animator.Play("Open");
        yield return new WaitForSeconds(time);
        animator.enabled = false;
        if(clickCount == 4) 
        {
            isOpen = true;
            boxCollider.enabled = false;
            gm.EnableCollider();
            // gm.objects[objectIndex].GetComponent<ObjectControl>().EnableCollider();
            // objectIndex++;
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canBeClicked = true;
    }

    IEnumerator WaitTheBox(float time, bool enable)
    {
        yield return new WaitForSeconds(time);
        boxCollider.enabled = enable;
        isInTheMiddle = !enable;
    }
}
