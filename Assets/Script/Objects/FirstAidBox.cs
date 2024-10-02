using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidBox : MonoBehaviour
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool isOpen;
    private bool canBeClicked = true; // I can say this as caninteract
    private bool isInTheMiddle;
    private int clickCount;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    private Vector3 intialPosition;
    private Vector3 initialRotation;
    private BoxCollider boxCollider;
    ///summary
    ///    Hover Visual
    ///summary
    private IHover hoverControl; //Controlling Hover Visual
    private StoryManager sm;
    [SerializeField] private PauseMenuUI pauseMenuUI;
    //private DialogueManager dm;

    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;
        sm = StoryManager.instance;
        //dm = DialogueManager.instance;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        hoverControl = GetComponent<IHover>();

        ///summary
        ///    Get initial pos of firstaid box
        ///summary
        intialPosition.x = -13.0f;
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
        if(canBeClicked && !gm.GetPauseMenuIsAnimating() && !pauseMenuUI.GetIsOpen()) hoverControl.ShowHoverVisual();
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
        if(!isOpen && canBeClicked && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating() && !sm.GetIsOpen())
        {
            clickCount++;

            canBeClicked = false;
            ///summary
            ///    Move it to the target place
            ///summary  
            if(clickCount == 1)
            {
                isInTheMiddle = true;
                am.PlayBoxMoveSFX();
                LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring().setOnComplete(() => canBeClicked = true);
                LeanTween.rotate(gameObject, targetRotation, 0.4f);
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
        if(isOpen && !isInTheMiddle && canBeClicked && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) 
        {
            am.PlayBoxMoveSFX();

            LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring();
            
            StartCoroutine(WaitTheBox(0.8f, false));
        }
    }

    #endregion

    ///summary
    ///    Move it to the initial place
    ///summary
    public void MoveBoxToInitialPosition()
    {
        am.PlayBoxMoveBackSFX();
        LeanTween.move(gameObject, intialPosition, 0.8f).setEaseSpring();
        StartCoroutine(WaitTheBox(0.5f, true));
    }

    public bool IsInTheMiddle()
    {
        return isInTheMiddle;
    }

    public void SetCanInteract(bool canBeClick) => canBeClicked = canBeClick;
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
        canBeClicked = true;
    }

    IEnumerator WaitTheBox(float time, bool enable)
    {
        yield return new WaitForSeconds(time);
        boxCollider.enabled = enable;
        isInTheMiddle = !enable;
    }
}
