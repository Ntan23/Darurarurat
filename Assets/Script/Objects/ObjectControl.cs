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
    private bool isInInspectMode;
    private bool isAnimating;
    private bool canRotate;
    private bool isInside;
    private bool isInTheBox = true;
    private bool alreadyUsed;
    #endregion

    #region OtherVariables
    [Header("Other Variables")]
    [SerializeField] private GameObject examineButton;
    [SerializeField] private Transform takenParent;
    private Collider objCollider;
    private Rigidbody rb;
    private Animator animator;
    private FirstAidBox firstAidBox;
    private GameManager gm;
    #endregion

    void Start() 
    {
        gm = GameManager.instance;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        objCollider = GetComponent<Collider>();

        firstAidBox = GetComponentInParent<FirstAidBox>();

        animator.enabled = false;
        objCollider.enabled = false;
        beforeAnimatePosition = transform.position;
        examineButton.SetActive(false);
    }

    void OnMouseDown()
    {
        if(firstAidBox.IsInTheMiddle()) firstAidBox.MoveBox();

        if(isInTheBox) examineButton.SetActive(false);

        if(transform.parent != takenParent) transform.parent = takenParent;
        
        cameraZPos = Camera.main.WorldToScreenPoint(transform.position).z;
        
        offset = transform.position - GetMouseWorldPos();
        beforeAnimatePosition = transform.position;

        if(!gm.GetIsInInspectMode() && isInTheBox) 
        {
            LeanTween.move(gameObject, new Vector3(transform.position.x, 5.0f, 0.0f), 0.3f);
            LeanTween.rotateY(gameObject, 0.0f, 0.3f);
        }
        
        if(!gm.GetIsInInspectMode() && !isAnimating) canMove = true;
    }

    void OnMouseEnter()
    {
        if(!isDragging && canExamine && !isInTheBox && !gm.GetIsInInspectMode()) examineButton.SetActive(true);
    }

    void OnMouseExit()
    {
        examineButton.SetActive(false);
        isDragging = false;
        if(!gm.GetIsInInspectMode()) canExamine = true;
    }

    void OnMouseUp()
    {
        if(isInside)
        {
            if(objectIndex == gm.GetProcedureIndex())
            {
                StartCoroutine(Wait());

                if(gm.GetProcedureIndex() <= gm.objects.Length) gm.AddProcedureObjectIndex();
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
        if(canMove && !isInTheBox)
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

    private void AfterAnimation()
    {
        animator.enabled = false;
        isAnimating = false;
        LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);
    }

    IEnumerator Wait()
    {
        canMove = false;
        isAnimating = true;
        yield return new WaitForSeconds(0.5f);
        animator.enabled = true;
        animator.SetTrigger("Animate");
        canExamine = false;
    }

    public void Inspect() 
    {
        firstAidBox.SetCanBeClicked(false);
        examineButton.SetActive(false);
        beforeInspectPosition = transform.position;
        LeanTween.move(gameObject, inspectPosition, 0.5f);

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
}
