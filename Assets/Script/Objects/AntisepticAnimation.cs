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

    [Header("For Object Animation")]
    [SerializeField] private GameObject cap;
    [SerializeField] private GameObject capMesh;
    private Collider objectCollider;
    [SerializeField] private Collider capCollider;
    private SkinnedMeshRenderer capSkinnedMeshRenderer;
    [SerializeField] private Material normalCapMaterial;
    [SerializeField] private Material transparentCapMaterial;
    [SerializeField] private ParticleSystem liquidParticleSystem;

    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;

    void Start() 
    {
        gm = GameManager.instance;
        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();
        objectCollider = GetComponent<Collider>();
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();

        foreach(GameObject go in instructionArrows) go.SetActive(false);
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

    private void UpdateAlpha(float alpha) => capSkinnedMeshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    
    IEnumerator OpenMoveRotateAnimation(bool hasCap)
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

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
        foreach(GameObject go in instructionArrows) go.SetActive(false);

        animator.Play("Open");
        yield return new WaitForSeconds(1.8f);
        // cap.transform.parent = targetParent;
        capSkinnedMeshRenderer.material = transparentCapMaterial;
        LeanTween.value(capMesh, UpdateAlpha, 1.0f, 0.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;

        canAnimate = false;
        isOpen = true;
    }

    IEnumerator CloseCap()
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);

        animator.Play("Close");
        yield return new WaitForSeconds(1.8f);
        objectControl.AfterAnimate();
        objectCollider.enabled = true;
        capCollider.enabled = false;
        canAnimate = false;
        isOpen = false;
    }

    public void PlayLiquidParticleSystem() => liquidParticleSystem.Play();
}
