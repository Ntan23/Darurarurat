using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntisepticAnimation : MonoBehaviour
{
    private bool canAnimate;
    private bool isOpen;
    private float mouseDistanceY;
    private Vector3 mousePosition;
    private Animator animator;
    private ObjectControl objectControl;
    private GameManager gm;

    [Header("For Object Animation")]
    [SerializeField] private GameObject capMesh;
    private Collider objectCollider;
    [SerializeField] private Collider capCollider;
    private SkinnedMeshRenderer capSkinnedMeshRenderer;
    [SerializeField] private Material normalCapMaterial;
    [SerializeField] private Material transparentCapMaterial;
    [SerializeField] private ParticleSystem liquidParticleSystem;

    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;

    private AudioManager am;

    void Start() 
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();
        objectCollider = GetComponent<Collider>();
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();

        foreach(GameObject go in instructionArrows) go.SetActive(false);
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        mouseDistanceY = Input.mousePosition.y - mousePosition.y;
        
        if(canAnimate)
        {
            if(Mathf.Abs(mouseDistanceY) <= 20.0f && Mathf.Abs(Vector3.Distance(Input.mousePosition, mousePosition)) >= 80.0f)
            {
                if(Input.mousePosition.x > mousePosition.x && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.x < mousePosition.x && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
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

    private void UpdateAlpha(float alpha) => capSkinnedMeshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    
    IEnumerator OpenMoveRotateAnimation(bool hasCap)
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        if(hasCap) 
        {
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }

        if(!hasCap) 
        {
            LeanTween.value(capMesh, UpdateAlpha, 0.0f, 1.0f, 0.8f).setOnComplete(() => 
            {
                capSkinnedMeshRenderer.material = normalCapMaterial;
                instructionArrows[0].SetActive(true);
                canAnimate = true;
            });
        }

        animator.enabled = true;
    }

    IEnumerator OpenCap()
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);
        
        objectControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Open");
        canAnimate = false;
        yield return new WaitForSeconds(1.8f);
        // cap.transform.parent = targetParent;
        capSkinnedMeshRenderer.material = transparentCapMaterial;
        LeanTween.value(capMesh, UpdateAlpha, 1.0f, 0.0f, 0.8f).setOnComplete(() =>
        {
            objectControl.AfterAnimate();
            objectCollider.enabled = true;
            capCollider.enabled = false;

            isOpen = true;
        });
    }

    IEnumerator CloseCap()
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);

        objectControl.ChangeCanShowEffectValue(false);
        am.PlayOpenCloseSFX();
        animator.Play("Close");
        canAnimate = false;
        yield return new WaitForSeconds(1.8f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;
        isOpen = false;
    }

    public void PlayLiquidParticleSystem() => liquidParticleSystem.Play();

    public bool IsOpen()
    {
        return isOpen;
    }

    public bool CanAnimate()
    {
        return canAnimate;
    }
}
