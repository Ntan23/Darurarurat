using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WipesAnimation : MonoBehaviour
{
    private enum objectType{
        gauzePad, wipes
    }

    [SerializeField] private objectType type;
    private Vector3 mousePosition;
    private float mouseDistanceY;
    private bool canAnimate;
    private bool isOpen;
    private ObjectControl objectControl;
    private GameManager gm;
    private Animator animator;
    [SerializeField] private Animator feetAnimator;
    [SerializeField] private MeshRenderer handMesh;
    [SerializeField] private Material cleanMaterial;
    [SerializeField] private GameObject instructionArrow;
    private AudioManager am;

    void Start()
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        animator = GetComponent<Animator>();
        objectControl = GetComponent<ObjectControl>();

        instructionArrow.SetActive(false);
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

        instructionArrow.SetActive(true);

        animator.enabled = true;
        canAnimate = true;
    }

    void OnMouseDown() => mousePosition = Input.mousePosition;

    void OnMouseUp()
    {
        mouseDistanceY = Input.mousePosition.y - mousePosition.y;

        if(canAnimate)
        {
            if(Input.mousePosition.x < mousePosition.x && Mathf.Abs(mouseDistanceY) <= 15.0f && Mathf.Abs(Vector3.Distance(Input.mousePosition, mousePosition)) >= 80.0f && type == objectType.wipes) StartCoroutine(PlayAnimation());

            if(Input.mousePosition.x > mousePosition.x && Mathf.Abs(mouseDistanceY) <= 15.0f && Mathf.Abs(Vector3.Distance(Input.mousePosition, mousePosition)) >= 80.0f && type == objectType.gauzePad) StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        instructionArrow.SetActive(false);
        
        am.PlayTearPaperSFX();
        animator.Play("Open");
        yield return new WaitForSeconds(1.6f);
        objectControl.AfterAnimate();
        canAnimate = false;
        isOpen = true;
    }

    public void ChangeHandTexture() => handMesh.material = cleanMaterial;

    public void PlayGauzePadAnimation() => feetAnimator.Play("ShowGauzePad");

    public bool IsOpen() 
    {
        return isOpen;
    }
}
