using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidBox_ObjIntB : ObjectInteraction_BasicInteraction
{
    private AudioManager am;// Sama   
    private StoryManager sm;
    [SerializeField] private PauseMenuUI pauseMenuUI;
    private IHover hoverControl; //Controlling Hover Visual

    
    private bool isOpen;
    private int clickCount;//How many clicks rn to open the box
    [Header("Target Move Position")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    private const float INITIAL_POSITION_X = -13.0f;
    private Vector3 intialPosition;
    private Vector3 initialRotation;
    private bool isInTheMiddle;
    private BoxCollider boxCollider;

    
    [Header("Lean Tween Duration")]
    [SerializeField]private float movingDuration = 0.8f;
    [SerializeField]private float rotateDuration = 0.4f;

    protected override void Start()
    {
        base.Start();
        canInteract = true;
        am = AudioManager.instance;
        sm = StoryManager.instance;

        boxCollider = GetComponent<BoxCollider>();
        hoverControl = GetComponent<IHover>();

        ///summary
        ///    Get initial pos of firstaid box
        ///summary
        intialPosition.x = INITIAL_POSITION_X;
        intialPosition.y = targetPosition.y;
        intialPosition.z = transform.position.z;

        initialRotation = transform.rotation.eulerAngles;
    }

    #region MouseInput
    ///summary
    ///    Hover UnHover
    ///summary
    
    void OnMouseOver()
    {
        if(canInteract && !gm.GetPauseMenuIsAnimating() && !pauseMenuUI.GetIsOpen()) hoverControl.ShowHoverVisual();
    }
    void OnMouseExit()
    {
        hoverControl.HideHoverVisual();
    }

    void OnMouseDown()
    {
        ///summary
        ///    unlocking the box
        ///summary
        if(!isOpen && canInteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating() && !sm.GetIsOpen())
        {
            clickCount++;

            canInteract = false;
            ///summary
            ///    Move it to the target place
            ///summary  
            if(clickCount == 1)
            {
                isInTheMiddle = true;
                am.PlayBoxMoveSFX();
                MoveBoxToMiddle();
            }
            ///summary
            ///    Unlock 1
            ///summary             
            if(clickCount == 2)
            {
                am.PlayBoxOpenSFX();
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.3f));
            } 
            ///summary
            ///    Unlock 2
            ///summary  
            if(clickCount == 3)
            {
                am.PlayBoxOpenSFX();
                StartCoroutine(DelayAnimation(0.3f));
                StartCoroutine(Wait(0.3f));
            }
            ///summary
            ///    Open Box
            ///summary  
            if(clickCount == 4) 
            {
                StartCoroutine(DelayAnimation(1.4f));
                StartCoroutine(Wait(1.5f));
            }
        }

        ///summary
        ///    Move it to the target place
        ///summary
        if(isOpen && !isInTheMiddle && canInteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) 
        {
            MoveBoxToMiddle();
        }
    }

    #endregion

    ///summary
    ///    Move it to the initial place
    ///summary
    public void MoveBoxToInitialPosition()
    {
        am.PlayBoxMoveBackSFX();
        LeanTween.move(gameObject, intialPosition, movingDuration).setEaseSpring();
        StartCoroutine(WaitTheBox(0.5f, true));
    }
    public void MoveBoxToMiddle()
    {
        am.PlayBoxMoveSFX();
        if(clickCount == 1)
        {
            LeanTween.move(gameObject, targetPosition, movingDuration).setEaseSpring().setOnComplete(() => canInteract = true);
            LeanTween.rotate(gameObject, targetRotation, rotateDuration);
        }
        else
        {
            LeanTween.move(gameObject, targetPosition, movingDuration).setEaseSpring();
            StartCoroutine(WaitTheBox(0.8f, false)); //||Want Change||
        }
    }

    public bool IsInTheMiddle()
    {
        return isInTheMiddle;
    }

    // public void SetCanBeClicked(bool canBeClick) => canBeClicked = canBeClick;
    ///summary
    ///    Open Box
    ///summary
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
    ///summary
    ///    Box can't be spam clicked
    ///summary
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        canInteract = true;
    }

    IEnumerator WaitTheBox(float time, bool enable)
    {
        yield return new WaitForSeconds(time);
        boxCollider.enabled = enable;
        isInTheMiddle = !enable;
    }
}
