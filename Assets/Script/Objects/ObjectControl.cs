using System.Collections;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    #region EnumVariables
    private enum Type
    {
        BandAid, Betadine
    }

    [SerializeField] private Type objectType;
    #endregion

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
    private bool alreadyAnimated;
    private bool canRotate;
    private bool isInside;
    private bool isInTheBox = true;
    private bool isPeeled;
    [SerializeField] private bool needToMoveBack;
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
    #endregion

    void Start() 
    {
        gm = GameManager.instance;

        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        firstAidBox = GetComponentInParent<FirstAidBox>();

        objCollider.enabled = false;
        beforeAnimatePosition = transform.position;
        HideAllButtons();
    }

    void OnMouseDown()
    {
        if(firstAidBox.IsInTheMiddle()) firstAidBox.MoveBox();

        if(isInTheBox) HideAllButtons();

        if(transform.parent != takenParent) transform.parent = takenParent;
        
        cameraZPos = Camera.main.WorldToScreenPoint(transform.position).z;
        
        offset = transform.position - GetMouseWorldPos();
        beforeAnimatePosition = transform.position;

        if(!gm.GetIsInInspectMode() && isInTheBox) 
        {
            LeanTween.move(gameObject, new Vector3(transform.position.x, 5.0f, 0.0f), 0.8f).setEaseSpring();
            LeanTween.rotateY(gameObject, 0.0f, 0.3f);
        }
        
        if(!gm.GetIsInInspectMode() && !isAnimating) canMove = true;
    }

    void OnMouseEnter()
    {
        if(!isDragging && canExamine && !isInTheBox  && !isAnimating && !gm.GetIsInInspectMode()) buttons[0].SetActive(true);

        if(objectType == Type.BandAid)
        {
            if(!isPeeled && !isDragging && canExamine && !isInTheBox  && !isAnimating && !gm.GetIsInInspectMode()) buttons[1].SetActive(true);
        }
    }

    void OnMouseExit()
    {
        HideAllButtons();
        isDragging = false;
        if(!gm.GetIsInInspectMode()) canExamine = true;
    }

    void OnMouseUp()
    {
        if(isInside)
        {
            if(objectIndex == gm.GetProcedureIndex())
            {
                LeanTween.move(gameObject, targetPosition, 0.5f);
                
                if(!alreadyAnimated && objectType != Type.BandAid) Animate();

                if(objectType == Type.BandAid)
                {
                    if(isPeeled) LeanTween.move(gameObject, new Vector3(9.3f, 1.0f, 9.3f), 0.5f);
                    if(!isPeeled) 
                    {
                        Debug.Log("Kamu Harus Peel Plesternya Terlebih Dahulu");
                        LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);
                    }

                    if(gm.GetProcedureIndex() <= gm.objects.Length && isPeeled) gm.AddProcedureObjectIndex();
                }
                else if(objectType != Type.BandAid)
                {
                    if(gm.GetProcedureIndex() <= gm.objects.Length) gm.AddProcedureObjectIndex();
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

    void OnMouseDrag() 
    {
        if(canMove && !isInTheBox && !isAnimating)
        {
            isDragging = true;
            canExamine = false;
            
            transform.position = GetMouseWorldPos() + offset;

            if(transform.position.y <= 5.0f || transform.position.y > 5.0f) transform.position = new Vector3(transform.position.x, 5.0f, transform.position.z);

            if(transform.position.x <= xBoundaries[0]) transform.position = new Vector3(xBoundaries[0], transform.position.y, transform.position.z);
            if(transform.position.x >= xBoundaries[1]) transform.position = new Vector3(xBoundaries[1], transform.position.y, transform.position.z);
            if(transform.position.z >= zBoundaries[0]) transform.position = new Vector3(transform.position.x, transform.position.y, zBoundaries[0]);
            if(transform.position.z <= zBoundaries[1]) transform.position = new Vector3(transform.position.x, transform.position.y, zBoundaries[1]); 
        }

        if(canRotate)
        {
            xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.down, xAxisRotation);
            transform.Rotate(Vector3.right, yAxisRotation);
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

    private void Animate()
    {
        canMove = false;
        isAnimating = true;
        objectAnimationControl.EnableAnimator();
        objectAnimationControl.PlayAnimation();
        canExamine = false;
    }

    public void AfterAnimate()
    {
        isAnimating = false;
        LeanTween.rotate(gameObject, Vector3.zero, 0.5f);
        objectAnimationControl.DisableAnimator();
        if(needToMoveBack) LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);

        alreadyAnimated = true;
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
        LeanTween.move(gameObject, beforeInspectPosition, 0.5f);
        LeanTween.rotate(gameObject, Vector3.zero, 0.5f);

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

    //For Band Aid Only
    public void Peel()
    {
        isPeeled = true;
        StartCoroutine(PeelAnimation());
        StartCoroutine(WaitForRotateAndMove(2.0f));
    }   

    IEnumerator PeelAnimation()
    {
        LeanTween.move(gameObject, new Vector3(0.0f, 8.0f, 2.0f), 0.5f);
        yield return new WaitForSeconds(0.6f);
        LeanTween.rotateX(gameObject, 90.0f, 0.3f);
    }

    IEnumerator WaitForRotateAndMove(float time) 
    {
        yield return new WaitForSeconds(time); 
        Animate();
    } 
}
