using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreamAnimation : MonoBehaviour, IHaveCream
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool canAnimate;// Sama
    private ObjectControl objectControl;// Sama
    private Vector3 mousePosition;// Sama
    private float mouseDistanceX;// Sama
    private bool isOpen;//Beda - Can Open Cap 
    [SerializeField] private Vector3 animationPosition;//Beda - For GrabCream
    private Vector3 beforeAnimatePosition;//Beda - Ini buat?
    [SerializeField] private Collider objectCollider;// Beda - Sama Tutup
    [SerializeField] private Collider capCollider;// Beda - Sama Tutup
    [SerializeField] private GameObject capMesh;//Beda
    private SkinnedMeshRenderer capSkinnedMeshRenderer;// Beda - Sama Tutup
    [SerializeField] private Material normalCapMaterial;// Beda - Sama Tutup
    [SerializeField] private Material transparentCapMaterial;// Beda - Sama Tutup
    [SerializeField] private GameObject cream;//Beda - It's something that come out of the item
    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;// Sama
    [SerializeField] private GameObject arrowParent;// Sama

    public event EventHandler OnGettingCream;
    public event EventHandler OnCreamReady;

    void Start()
    {
        gm = GameManager.instance;// Sama
        am = AudioManager.instance;// Sama

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();
        // if(playerArm != null) playerArmAnimator = playerArm.GetComponent<Animator>();

        foreach(GameObject go in instructionArrows) go.SetActive(false);// Sama
    
        arrowParent.SetActive(false);// Sama
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;// Sama

    void OnMouseUp()// Sama Beda Courotine; Ganti jd Fungsi Kepisah aja ||Want Change||
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;

        if(canAnimate)// Sam
        {
            if(Mathf.Abs(mouseDistanceX) >= 50.0f)
            {
                if(Input.mousePosition.x < mousePosition.x && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.x > mousePosition.x && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
            }
        }
    }
    #region OpenCloseCap
    public void Open()// Sama - Beda Tutup (Sama Isi)
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        objectCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(true));
    }

    public void Close()// Sama - Beda Tutup (Sama Isi)
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        objectCollider.enabled = false;
        capCollider.enabled = true;
        StartCoroutine(OpenMoveRotateAnimation(false));
    }


    private void UpdateAlpha(float alpha) => capSkinnedMeshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);// Sama - Beda Tutup (Sama Isi)

    IEnumerator OpenMoveRotateAnimation(bool hasCap)// Sama - Beda Tutup 
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        //Bagian ini Beda
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
        //Bagian ini Beda
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
    #endregion

    #region GrabCream
    ///summary
    ///    Grab Cream
    ///summary 
    public void PlayAnimation() => StartCoroutine(GrabCreamAnimation());
    IEnumerator GrabCreamAnimation()
    {
        gm.ChangeIsAnimatingValue(true);
        // playerArmAnimator.Play("Take Rash Cream");
        OnGettingCream?.Invoke(this, EventArgs.Empty);

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
        // playerArm.GetComponent<PlayerHand>().ChangeCanInteract();
        OnCreamReady?.Invoke(this, EventArgs.Empty);
        gm.ChangeIsAnimatingValue(false);
    }
    #endregion
    public bool CanAnimate()
    {
        return canAnimate;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
