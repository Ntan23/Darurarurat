using System.Collections;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    #region VectorVariables
    private Vector3 offset;
    private Vector3 mousePos;
    private Vector3 beforeAnimatePosition;
    private Vector3 beforeInspectPosition;
    [Header("Position")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 inspectPosition;
    #endregion

    #region IntegerVaribles
    [SerializeField] private int objectIndex;
    #endregion

    #region FloatVariables
    private float cameraZPos;
    [Header("Inspect Rotation")]
    [SerializeField] private float rotationSpeed;
    private float xAxisRotation;
    private float yAxisRotation;
    [Header("Boundaries")]
    [SerializeField] private float[] xBoundaries;
    [SerializeField] private float[] zBoundaries;
    #endregion

    #region BoolVariables
    private bool canMove = true;
    private bool canExamine = true;
    private bool isDragging;
    private bool isAnimating;
    private bool canRotate;
    private bool isInside;
    private bool isInTheBox = true;
    private bool isProcedureFinished;
    #endregion

    #region OtherVariables
    [Header("Other Variables")]
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform takenParent;
    private Collider objCollider;
    private Rigidbody rb;
    private FirstAidBox firstAidBox;
    private GameManager gm;
    [SerializeField] private ObjectAnimationControl objectAnimationControl;
    private GameObject antisepticLiquidParticleSystem;
    #endregion

    void Start() 
    {
        gm = GameManager.instance;

        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        firstAidBox = GetComponentInParent<FirstAidBox>();

        antisepticLiquidParticleSystem = GameObject.FindGameObjectWithTag("Liquid");

        objCollider.enabled = false;
        SetBeforeAnimatePosition();
        HideAllButtons();
    }
    void Update() 
    {
        if(rb.velocity.x != 0 || rb.velocity.y != 0 || rb.velocity.z != 0) rb.velocity = Vector3.zero;
    }

    void OnMouseDown()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            if(firstAidBox.IsInTheMiddle()) firstAidBox.MoveBox();

            if(isInTheBox) HideAllButtons();

            if(transform.parent != takenParent) transform.parent = takenParent;
            
            cameraZPos = Camera.main.WorldToScreenPoint(transform.position).z;
            
            offset = transform.position - GetMouseWorldPos();
            if(!gm.GetIsAnimating()) SetBeforeAnimatePosition();

            if(!gm.GetIsInInspectMode() && isInTheBox && !isAnimating) 
            {
                LeanTween.move(gameObject, new Vector3(transform.position.x, 5.0f, 0.0f), 0.8f).setEaseSpring();
                
                if(gameObject.name == "Petroleum Jelly") LeanTween.rotateY(gameObject, -180.0f, 0.3f);
                else if(gameObject.name != "Tisu Basah Non Alkohol") LeanTween.rotate(gameObject, Vector3.zero, 0.3f);

                if(rb.isKinematic) rb.isKinematic = false;
            }
            
            if(!gm.GetIsInInspectMode() && !gm.GetIsAnimating()) canMove = true;
        }
    }

    void OnMouseEnter()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            if(!isDragging && canExamine && !isInTheBox  && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) buttons[0].SetActive(true);

            if(buttons.Length == 2)
            {
                if(!isProcedureFinished && !isDragging && canExamine && !isInTheBox  && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) buttons[1].SetActive(true);
            }

            if(buttons.Length == 3)
            {
                if(!isProcedureFinished && !isDragging && canExamine && !isInTheBox  && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) 
                {
                    buttons[1].SetActive(true);
                    buttons[2].SetActive(false);
                }

                if(isProcedureFinished && !isDragging && canExamine && !isInTheBox  && !gm.GetIsAnimating() && !gm.GetIsInInspectMode())
                {
                    buttons[1].SetActive(false);
                    buttons[2].SetActive(true);
                }
            }
        }
    }

    void OnMouseExit()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            HideAllButtons();
            isDragging = false;
            if(!gm.GetIsInInspectMode()) canExamine = true;

            if(rb.isKinematic) rb.isKinematic = false;
        }
    }

    void OnMouseUp()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            if(isInside)
            {
                if(objectIndex == gm.GetProcedureIndex())
                {
                    LeanTween.move(gameObject, targetPosition, 0.5f);
                    
                    if(isProcedureFinished) 
                    {
                        if(gameObject.name == "Plester") StartCoroutine(Plester());

                        if(gameObject.name == "Antiseptik") StartCoroutine(Antiseptic());

                        if(gameObject.name == "Petroleum Jelly") GetComponent<PetroleumJellyAnimation>().PlayAnimation();

                        if(gameObject.name == "Tisu Basah Non Alkohol") StartCoroutine(Wipes());
                    }
                    if(!isProcedureFinished) 
                    {
                        if(gameObject.name == "Plester") gm.ShowWrongProcedureUIForProceduralObjects("Kamu Harus Buka Plesternya Terlebih Dahulu");
                        
                        if(gameObject.name == "Antiseptik") gm.ShowWrongProcedureUIForProceduralObjects("Kamu Harus Buka Antiseptiknya Terlebih Dahulu");
                        
                        if(gameObject.name == "Petroleum Jelly") gm.ShowWrongProcedureUIForProceduralObjects("Kamu Harus Buka Petroleum Jellynya Terlebih Dahulu");

                        if(gameObject.name == "Tisu Basah Non Alkohol") gm.ShowWrongProcedureUIForProceduralObjects("Kamu Harus Buka Tisunya Terlebih Dahulu");

                        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
                    }  
                }
                else if(objectIndex > gm.GetProcedureIndex())
                {
                    LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);
                    gm.ShowWrongProcedureUI();
                }
            }

            if(isInTheBox) isInTheBox = false;
            if(isDragging) isDragging = false;
        }
    }


    void OnMouseDrag() 
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            if(canMove && !isInTheBox && !gm.GetIsAnimating() && !gm.GetIsInInspectMode())
            {
                if(!rb.isKinematic) rb.isKinematic = true;

                isDragging = true;
                canExamine = false;
                
                transform.position = GetMouseWorldPos() + offset;

                if(transform.position.y <= 5.0f || transform.position.y > 5.0f) transform.position = new Vector3(transform.position.x, 5.0f, transform.position.z);

                if(transform.position.x <= xBoundaries[0]) transform.position = new Vector3(xBoundaries[0], transform.position.y, transform.position.z);
                if(transform.position.x >= xBoundaries[1]) transform.position = new Vector3(xBoundaries[1], transform.position.y, transform.position.z);
                if(transform.position.z >= zBoundaries[0]) transform.position = new Vector3(transform.position.x, transform.position.y, zBoundaries[0]);
                if(transform.position.z <= zBoundaries[1]) transform.position = new Vector3(transform.position.x, transform.position.y, zBoundaries[1]); 
            }

            if(canRotate && gm.GetIsInInspectMode())
            {
                xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.Rotate(Vector3.down, xAxisRotation);
                transform.Rotate(Vector3.right, yAxisRotation);
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        mousePos = Input.mousePosition;

        mousePos.z = cameraZPos;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) isInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(isInside) isInside = false;
    }

    public void AfterAnimate()
    {
        gm.ChangeIsAnimatingValue(false);

        if(gameObject.name == "Petroleum Jelly") LeanTween.rotate(gameObject, new Vector3(0.0f, -180.0f, 0.0f), 0.3f);
        else if(gameObject.name == "Tisu Basah Non Alkohol") LeanTween.rotate(gameObject, new Vector3(90.0f, 0.0f, 0.0f), 0.3f);
        else if(gameObject.name != "Tisu Basah Non Alkohol") LeanTween.rotate(gameObject, Vector3.zero, 0.3f);

        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();

        if(rb.isKinematic) rb.isKinematic = false;
     }

    public void Inspect() 
    {
        firstAidBox.SetCanBeClicked(false);
        HideAllButtons();
        beforeInspectPosition = transform.position;
        LeanTween.move(gameObject, inspectPosition, 0.8f).setEaseSpring();

        canExamine = false;
        canMove = false;
        canRotate = true;

        gm.OpenInspectUI(objectIndex);
    }

    public void CloseInspect()
    {
        firstAidBox.SetCanBeClicked(true);
        LeanTween.move(gameObject, beforeInspectPosition, 0.8f).setEaseSpring();

        if(gameObject.name == "Petroleum Jelly") LeanTween.rotate(gameObject, new Vector3(0.0f, -180.0f, 0.0f), 0.3f);
        else if(gameObject.name == "Tisu Basah Non Alkohol") LeanTween.rotate(gameObject, new Vector3(90.0f, 0.0f, 0.0f), 0.3f);
        else if(gameObject.name != "Tisu Basah Non Alkohol") LeanTween.rotate(gameObject, Vector3.zero, 0.3f);

        if(rb.isKinematic) rb.isKinematic = false;
        
        canRotate = false;
        canExamine = true;
        canMove = true;
    }

    public void EnableCollider() => objCollider.enabled = true;

    private void HideAllButtons() 
    {
        foreach(GameObject go in buttons)
        {
            go.SetActive(false);
        }
    }

    public void SetBeforeAnimatePosition() => beforeAnimatePosition = transform.position;

    public Vector3 GetBeforeAnimatePosition()
    {
        return beforeAnimatePosition;
    }

    public void ChangeIsProcedureFinishedValue() => isProcedureFinished = !isProcedureFinished;
    // public void ChangeIsAnimatingValue() => isAnimating = !isAnimating;

    IEnumerator CheckCondition()
    {
        gm.AddProcedureObjectIndex();
        yield return new WaitForSeconds(0.1f);
        gm.CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if(gm.GetProcedureIndex() <= gm.objects.Length && isProcedureFinished) StartCoroutine(CheckCondition());
    }

    IEnumerator Plester()
    {
        LeanTween.move(gameObject, new Vector3(9.66f, 1.4f, 7.44f), 0.5f);
        LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
        yield return new WaitForSeconds(0.7f);
        CheckWinCondition();
    }

    IEnumerator Antiseptic()
    {
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, new Vector3(6.6f, 5.0f, 6.15f), 0.8f).setEaseSpring();
        LeanTween.rotateY(gameObject, -90.0f, 0.3f);
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().Play("Pour");
        yield return new WaitForSeconds(1.2f);
        CheckWinCondition();
    }

    IEnumerator Wipes()
    {
        gm.ChangeIsAnimatingValue(true);
        yield return new WaitForSeconds(0.6f);
        LeanTween.moveY(gameObject, 1.66f, 0.3f);
        yield return new WaitForSeconds(0.6f);
        LeanTween.moveY(gameObject, targetPosition.y, 0.3f);
        yield return new WaitForSeconds(0.6f);
        LeanTween.moveY(gameObject, 1.66f, 0.3f);
        yield return new WaitForSeconds(0.6f);
        LeanTween.moveY(gameObject, targetPosition.y, 0.3f);
        yield return new WaitForSeconds(0.8f);
        AfterAnimate();
        CheckWinCondition();
    }
}
