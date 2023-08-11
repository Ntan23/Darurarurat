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
    private SkinnedMeshRenderer boxRenderer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material hoverMaterial;
    private GameManager gm;
    private AudioManager am;
    //private DialogueManager dm;

    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;
        //dm = DialogueManager.instance;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        intialPosition.x = -13.0f;
        intialPosition.y = targetPosition.y;
        intialPosition.z = transform.position.z;

        initialRotation = transform.rotation.eulerAngles;
    }

    void OnMouseOver()
    {
        if(canBeClicked && !gm.GetPauseMenuIsAnimating()) boxRenderer.material = hoverMaterial;
    }

    void OnMouseDown()
    {
        if(!isOpen && canBeClicked && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            clickCount++;

            canBeClicked = false;
            if(clickCount == 1)
            {
                isInTheMiddle = true;
                am.PlayBoxMoveSFX();
                LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring().setOnComplete(() => canBeClicked = true);
                LeanTween.rotate(gameObject, targetRotation, 0.4f);
            }
            if(clickCount == 2)
            {
                am.PlayBoxOpenSFX();
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.3f));
            } 
            if(clickCount == 3)
            {
                am.PlayBoxOpenSFX();
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.3f));
            }
            if(clickCount == 4) 
            {
                StartCoroutine(DelayAnimation(1.4f));
                StartCoroutine(Wait(1.5f));
            }
        }

        if(isOpen && !isInTheMiddle && canBeClicked && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) 
        {
            am.PlayBoxMoveSFX();

            LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring();
            
            StartCoroutine(WaitTheBox(0.8f, false));
        }
    }

    void OnMouseExit()
    {
        boxRenderer.material = originalMaterial;
    }

    public void MoveBox()
    {
        am.PlayBoxMoveBackSFX();
        LeanTween.move(gameObject, intialPosition, 0.8f).setEaseSpring();
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
