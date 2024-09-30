using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntisepticAnimation : MonoBehaviour
{
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool canAnimate;// Sama
    private ObjectControl objectControl;// Sama
    private Vector3 mousePosition;// Sama
    private float mouseDistanceX;// Sama
    private bool isOpen;// Beda - Can Open Bottle

    [Header("For Object Animation")]
    private Collider objectCollider;// Beda - Sama Tutup
    [SerializeField] private Collider capCollider;// Beda - Sama Tutup
    [SerializeField] private GameObject capMesh;// Beda - Sama Tutup
    private SkinnedMeshRenderer capSkinnedMeshRenderer;// Beda - Sama Tutup
    [SerializeField] private Material normalCapMaterial;// Beda - Sama Tutup
    [SerializeField] private Material transparentCapMaterial;// Beda - Sama Tutup
    [SerializeField] private ParticleSystem liquidParticleSystem;// Beda

    [Header("For Arrow Instruction UI")]
    [SerializeField] private GameObject[] instructionArrows;// Sama
    [SerializeField] private GameObject instructionArrowParent;// Sama


    void Start() 
    {
        gm = GameManager.instance;// Sama
        am = AudioManager.instance;// Sama

        animator = GetComponent<Animator>();// Sama
        objectControl = GetComponent<ObjectControl>();// Sama
        objectCollider = GetComponent<Collider>();//Beda - Sama Tutup
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();//Beda - Sama Tutup

        ///summary
        ///    Get All Instruction Arrows & Turn it off
        ///summary
        foreach(GameObject go in instructionArrows) go.SetActive(false);// Sama
        //di sini arrowParent not setactive false ||Ganti ga ya||
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;// Sama - mungkin jdiin interface kali(?)

    void OnMouseUp()// Sama Beda Courotine; Ganti jd Fungsi Kepisah aja ||Want Change||
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;
        
        if(canAnimate)// Sama
        {
            if(Mathf.Abs(mouseDistanceX) >= 50.0f)
            {
                if(Input.mousePosition.x > mousePosition.x && !isOpen && capCollider.enabled) StartCoroutine(OpenCap());

                if(Input.mousePosition.x < mousePosition.x && isOpen && capCollider.enabled) StartCoroutine(CloseCap());
            }
        }
    }
    
    #region OpenCloseCap

    /// <summary>
    /// Do the Open or Close Animation
    /// </summary>
    public void Open()// Sama, Beda Isi ||Want Change|| // Sama - Beda Tutup (Sama Isi)
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
    
    /// <summary>
    /// Do it and show InstructionUI
    /// </summary>
    IEnumerator OpenMoveRotateAnimation(bool hasCap)// Sama - Beda Tutup 
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.8f);
        LeanTween.rotateX(gameObject, 60.0f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        //Bagian ini Beda
        if(hasCap) 
        {
            instructionArrowParent.SetActive(true);
            instructionArrows[1].SetActive(true);
            canAnimate = true;
        }

        if(!hasCap) 
        {
            StartCoroutine(CapMeshChange());
        }
        

        animator.enabled = true;
        //Bagian ini Beda
    }

    IEnumerator CapMeshChange()//Tak ada di sebelah
    {
        LeanTween.value(capMesh, UpdateAlpha, 0.0f, 1.0f, 0.8f);
        yield return new WaitForSeconds(1.0f);
        capSkinnedMeshRenderer.material = normalCapMaterial;
        instructionArrowParent.SetActive(true);
        instructionArrows[0].SetActive(true);
        canAnimate = true;
    }

    /// <summary>
    /// Do Open the cap
    /// </summary>
    IEnumerator OpenCap() // Sama - Beda Tutup (Beda urutan, abis yield return jg ada beda dikit)
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);

        instructionArrowParent.SetActive(false);
        
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

    /// <summary>
    /// Do Close the cap
    /// </summary>
    IEnumerator CloseCap() // Sama - Beda Tutup (Neda urutan)
    {
        foreach(GameObject go in instructionArrows) go.SetActive(false);

        instructionArrowParent.SetActive(false);

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
    #endregion

    ///summary
    ///    Particle System Keluar Antiseptic
    ///summary    
    public void PlayLiquidParticleSystem() => liquidParticleSystem.Play();


    public bool IsOpen()//Jadiin yg bool getset aja ||Want Change||
    {
        return isOpen;
    }

    public bool CanAnimate()//Sama mungkin ini semua perlu?? ato ini jadiin itu aja yg bool get set etc ||Want Change||
    {
        return canAnimate;
    }
}
