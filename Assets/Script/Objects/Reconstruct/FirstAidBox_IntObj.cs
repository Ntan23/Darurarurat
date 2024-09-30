using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAidBox_IntObj : ObjectInteraction
{
    private AudioManager am;// Sama
        private bool isOpen;
    private bool caninteract = true; // can be interacted/clicked
    private bool isInTheMiddle;
    private int clickCount;
    [Header("Target Moving and Rotating Position")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 targetRotation;
    private Vector3 intialPosition;
    private Vector3 initialRotation;
    private BoxCollider boxCollider;
    ///summary
    ///    Hover Visual
    ///summary
    private SkinnedMeshRenderer boxRenderer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material hoverMaterial;
    private StoryManager sm;
    [SerializeField] private PauseMenuUI pauseMenuUI;
    //private DialogueManager dm;
    public bool IsInTheMiddle {get {return isInTheMiddle;} private set { isInTheMiddle = value;} }

    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;
        sm = StoryManager.instance;
        //dm = DialogueManager.instance;

        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

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
        if(caninteract && !gm.GetPauseMenuIsAnimating() && !pauseMenuUI.GetIsOpen()) boxRenderer.material = hoverMaterial;
    }
    void OnMouseExit()
    {
        boxRenderer.material = originalMaterial;
    }

    void OnMouseDown()
    {
        ///summary
        ///    unlocking the box
        ///summary
        if(!isOpen && caninteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating() && !sm.GetIsOpen())
        {
            clickCount++;

            caninteract = false;
            ///summary
            ///    Move it to the target place
            ///summary  
            if(clickCount == 1)
            {
                isInTheMiddle = true;
                am.PlayBoxMoveSFX();
                LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring().setOnComplete(() => caninteract = true);
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
        if(isOpen && !isInTheMiddle && caninteract && gm.IsPlaying() && !gm.GetPauseMenuIsAnimating()) 
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
    public void MoveBox()
    {
        am.PlayBoxMoveBackSFX();
        LeanTween.move(gameObject, intialPosition, 0.8f).setEaseSpring();
        StartCoroutine(WaitTheBox(0.5f, true));
    }

    public void SetCanInteract(bool canInteract) => caninteract = canInteract;
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
        caninteract = true;
    }

    IEnumerator WaitTheBox(float time, bool enable)
    {
        yield return new WaitForSeconds(time);
        boxCollider.enabled = enable;
        isInTheMiddle = !enable;
    }
}
