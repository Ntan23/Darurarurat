using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipesAnimation : MonoBehaviour
{
    private enum objectType{
        gauzePad, wipes
    }

    [SerializeField] private objectType type;
    private GameManager gm;// Sama
    private AudioManager am;// Sama
    private Animator animator;// Sama
    private bool canAnimate;// Sama
    private ObjectControl objectControl;// Sama
    private Vector3 mousePosition;// Sama
    private float mouseDistanceX;// Sama
    private bool isOpen; //only temporary cap
    [SerializeField] private Animator feetAnimator;//feet animator
    [SerializeField] private MeshRenderer handMesh;
    [SerializeField] private Material cleanMaterial;
    [SerializeField] private GameObject instructionArrow;// Sama
    [SerializeField] private GameObject arrowParent;// Sama

    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();

        arrowParent.SetActive(false);

        instructionArrow.SetActive(false);
    }
    void OnMouseDown() => mousePosition = Input.mousePosition;// Sama

    void OnMouseUp()// Sama Beda Courotine; Ganti jd Fungsi Kepisah aja ||Want Change||
    {
        mouseDistanceX = Input.mousePosition.x - mousePosition.x;

        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && type == objectType.wipes) StartCoroutine(PlayAnimation());

            if(Input.mousePosition.x > mousePosition.x && Mathf.Abs(mouseDistanceX) >= 50.0f && type == objectType.gauzePad) StartCoroutine(PlayAnimation());
        }
    }

    public void Open()
    {
        objectControl.SetBeforeAnimatePosition();
        gm.ChangeIsAnimatingValue(true);
        objectControl.ChangeIsProcedureFinishedValue();
        StartCoroutine(OpenAnimation());
    } 

    IEnumerator OpenAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f).setEaseSpring();
        yield return new WaitForSeconds(0.6f);
        if(type == objectType.wipes) LeanTween.rotateX(gameObject, 90.0f, 0.3f);
        if(type == objectType.gauzePad) LeanTween.rotateX(gameObject, -90.0f, 0.3f);
        yield return new WaitForSeconds(0.2f);

        arrowParent.SetActive(true);
        instructionArrow.SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }


    IEnumerator PlayAnimation()
    {
        arrowParent.SetActive(false);
        instructionArrow.SetActive(false);
        
        am.PlayTearPaperSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objectControl.AfterAnimate();
        canAnimate = false;
        isOpen = true;
    }
    //away to make yg dibwh ga nyangkut di sini


    //Hand texture????
    public void ChangeHandTexture() => handMesh.material = cleanMaterial;

    public void PlayGauzePadAnimation() => feetAnimator.Play("ShowGauzePad");

    public bool IsOpen() 
    {
        return isOpen;
    }
}
