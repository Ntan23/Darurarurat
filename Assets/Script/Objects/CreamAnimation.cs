using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreamAnimation : MonoBehaviour
{
    private Vector3 mousePosition;
    private float mouseDistanceX;
    [SerializeField] private Vector3 animationPosition;
    [SerializeField] private GameObject playerArm;
    private Vector3 beforeAnimatePosition;
    private bool canAnimate;
    private bool isOpen;
    [SerializeField] private Collider objectCollider;
    [SerializeField] private Collider capCollider;
    [SerializeField] private GameObject capMesh;
    [SerializeField] private GameObject cream;
    private SkinnedMeshRenderer capSkinnedMeshRenderer;
    [SerializeField] private Material normalCapMaterial;
    [SerializeField] private Material transparentCapMaterial;
    private Animator animator;
    private Animator playerArmAnimator;
    private ObjectControl objectControl;
    private GameManager gm;
    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;
    [SerializeField] private GameObject arrowParent;
    private AudioManager am;
    
    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();
        playerArmAnimator = playerArm.GetComponent<Animator>();

        foreach(GameObject go in instructionArrows) go.SetActive(false);
    
        arrowParent.SetActive(false);
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;

        if(canAnimate)
        {
            if(Mathf.Abs(mouseDistanceX) >= 50.0f)
            {
                if(Input.mousePosition.x < mousePosition.x && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.x > mousePosition.x && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
            }
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

    public void PlayAnimation() => StartCoroutine(GrabCreamAnimation());

    private void UpdateAlpha(float alpha) => capSkinnedMeshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);

    IEnumerator OpenMoveRotateAnimation(bool hasCap)
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        arrowParent.SetActive(true);

        if(hasCap) instructionArrows[1].SetActive(true);
        if(!hasCap) 
        {
            LeanTween.value(capMesh, UpdateAlpha, 0.0f, 1.0f, 0.8f);
            yield return new WaitForSeconds(1.0f);
            capSkinnedMeshRenderer.material = normalCapMaterial;
            instructionArrows[0].SetActive(true);
        }
        
        animator.enabled = true;
        canAnimate = true;
    }

    IEnumerator OpenCap()
    {
        arrowParent.SetActive(false);

        foreach(GameObject go in instructionArrows) go.SetActive(false);

        objectControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Open");
        canAnimate = false;
        yield return new WaitForSeconds(2.1f);
        capSkinnedMeshRenderer.material = transparentCapMaterial;
        LeanTween.value(capMesh, UpdateAlpha, 1.0f, 0.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;
        
        isOpen = true;
    }

    IEnumerator CloseCap()
    {
        arrowParent.SetActive(false);

        foreach(GameObject go in instructionArrows) go.SetActive(false);

        objectControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Close");
        canAnimate = false;
        yield return new WaitForSeconds(2.2f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;

        isOpen = false;
    }

    IEnumerator GrabCreamAnimation()
    {
        gm.ChangeIsAnimatingValue(true);
        playerArmAnimator.Play("Take Rash Cream");
        yield return new WaitForSeconds(0.1f);
        LeanTween.move(gameObject, animationPosition, 1.0f).setEaseSpring();
        LeanTween.rotateZ(gameObject, -120.0f, 0.5f);
        yield return new WaitForSeconds(2.0f);
        animator.Play("Squeze");
        yield return new WaitForSeconds(0.2f);
        LeanTween.scale(cream, new Vector3(0.1f, 0.1f, 0.1f), 0.8f);
        LeanTween.move(cream, new Vector3(0.95f, 12.5f, 0.6f), 0.8f).setOnComplete(() => cream.SetActive(false));
        yield return new WaitForSeconds(1.0f);
        beforeAnimatePosition = objectControl.GetBeforeAnimatePosition();
        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
        LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(1.2f);
        playerArm.GetComponent<PlayerHand>().ChangeCanInteract();
        gm.ChangeIsAnimatingValue(false);
    }

    public bool CanAnimate()
    {
        return canAnimate;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
