using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class ObjectControl : MonoBehaviour
{
    // ||Ganti ga ya|| mau dijadiin ke kelas kecil kecil
    #region enum
    //Ntr kuganti lg
    public enum Object{
        Antiseptic, BandAid, Petroleum, Wipes, Cream, GauzePad, Bandage
    }

    [SerializeField] private Object objectType;
    #endregion

    [SerializeField] private LocalizedStringTable table;

    #region VectorVariables
    private Vector3 offset;
    private Vector3 mousePos;
    private Vector3 beforeAnimatePosition;
    private Vector3 beforeInspectPosition;
    [Header("Position")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Vector3 inspectPosition;
    private Vector3 intialRotation;
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
    private bool canHide = true;
    private bool canShowEffect = true;
    private bool isSelect;
    private bool canBeUse;
    #endregion

    #region OtherVariables
    [Header("Other Variables")]
    private IHover hoverControl;
    // [SerializeField]bool useHoverControl;
    // [SerializeField] private SkinnedMeshRenderer[] meshRenderer;
    // [SerializeField] private Material[] hoverMaterial;
    // [SerializeField] private Material[] originalMaterial;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform takenParent;
    private Collider objCollider;
    private Rigidbody rb;
    private FirstAidBox firstAidBox;
    private GameManager gm;
    private AudioManager am;
    #endregion

    void Start() 
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        firstAidBox = GetComponentInParent<FirstAidBox>();
        hoverControl = GetComponent<IHover>();

        objCollider.enabled = false;
        SetBeforeAnimatePosition();
        HideAllButtons();
    }


    void Update() 
    {
        if(rb.velocity.x != 0 || rb.velocity.y != 0 || rb.velocity.z != 0) rb.velocity = Vector3.zero;
    
        if(gm.IsPausing() && canHide)
        {
            HideAllButtons();

            hoverControl?.HideHoverVisual();
            // if(useHoverControl)hoverControl?.HideHoverVisual();
            // else if(!useHoverControl)HideHoverEffect();

            canHide = false;
        }
        else if(!gm.IsPausing() && !canHide) canHide = true;
    }

    #region MouseInput
    void OnMouseDown()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            ///summary
            ///     If item Clicked, Get firstaidbox to initial place
            ///summary  
            if(firstAidBox.IsInTheMiddle()) firstAidBox.MoveBox();

            if(isInTheBox) HideAllButtons();
            ///summary
            ///     Parent change from first aid to target parent
            ///summary  
            if(transform.parent != takenParent) transform.parent = takenParent;
            
            ///summary
            ///     Get the correct z of the item
            ///summary 
            cameraZPos = Camera.main.WorldToScreenPoint(transform.position).z;
            
            ///summary
            ///     Get distance between mouse and item
            ///summary  
            offset = transform.position - GetMouseWorldPos(); 
            ///summary
            ///     If not animating/moving, get item pos rn
            ///summary
            if(!gm.GetIsAnimating()) SetBeforeAnimatePosition();

            ///summary
            ///     If not inspecting and in the box, get it out
            ///summary 
            if(!gm.GetIsInInspectMode() && isInTheBox && !isAnimating) 
            {
                LeanTween.moveY(gameObject, 5.0f, 0.8f).setEaseSpring().setOnComplete(() => SetBeforeAnimatePosition());
                
                ///summary
                ///     Rotating the item to the correct rotation
                ///summary 
                if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.Bandage && objectType != Object.GauzePad) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
                if(objectType == Object.Petroleum) LeanTween.rotate(gameObject, new Vector3(270.0f, 0.0f, 180.0f), 0.3f);
                if(objectType == Object.Bandage) LeanTween.rotate(gameObject, new Vector3(270.0f, -90.0f, 0.0f), 0.3f);
                if(objectType == Object.GauzePad) LeanTween.rotateY(gameObject, 180.0f, 0.3f);

                if(rb.isKinematic) rb.isKinematic = false;
            }
            
            ///summary
            ///     If not inspecting and outside the box, we can move around the item
            ///summary 
            if(!gm.GetIsInInspectMode() && !gm.GetIsAnimating()) canMove = true;
        }
    }

    void OnMouseEnter()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            ///summary
            ///     HoverEffect if it's not inspect and inside box
            ///summary
            if(!isDragging && canExamine && !gm.GetIsInInspectMode())hoverControl?.ShowHoverVisual();
            // {
            //     if(useHoverControl)hoverControl?.ShowHoverVisual();
            //     else if(!useHoverControl)ShowHoverEffect();
            // }
            

            ///summary
            ///     Show buttons to examine, open etc, if outside box
            ///summary
            if(!isDragging && canExamine && !isInTheBox && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) buttons[0].SetActive(true);

            if(buttons.Length == 2)
            {
                if(!isProcedureFinished && !isDragging && canExamine && !isInTheBox && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) buttons[1].SetActive(true);
            }

            if(buttons.Length == 3)
            {
                if(!isProcedureFinished && !isDragging && canExamine && !isInTheBox && !gm.GetIsAnimating() && !gm.GetIsInInspectMode()) 
                {
                    buttons[1].SetActive(true);
                    buttons[2].SetActive(false);
                }

                if(isProcedureFinished && !isDragging && canExamine && !isInTheBox && !gm.GetIsAnimating() && !gm.GetIsInInspectMode())
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
            if(canShowEffect)hoverControl?.HideHoverVisual();
            // {
            //     if(useHoverControl)hoverControl?.HideHoverVisual();
            //     else if(!useHoverControl)HideHoverEffect();
            // }

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
            isSelect = false;

            if(isInTheBox) isInTheBox = false;
            if(isDragging) isDragging = false;
        }
    }

    void OnMouseDrag() 
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            ///summary
            ///     if it's selected, outside box, and not inspect mode, we can drag around based on boundaries; if it's exit boundaries pull it back
            ///summary
            isSelect = true;
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

            ///summary
            ///     inspect mode
            ///summary
            if(canRotate && gm.GetIsInInspectMode())
            {
                xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.Rotate(Vector3.down, xAxisRotation);
                transform.Rotate(Vector3.right, yAxisRotation);
            }
        }
    }

    #endregion
    ///summary
    ///     GetMousePos but the z is the same as item.
    ///summary
    private Vector3 GetMouseWorldPos()
    {
        mousePos = Input.mousePosition;

        mousePos.z = cameraZPos;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    #region Doing Something with Item
    ///summary
    ///     If the item got close to the body, do something based on the item function
    ///summary
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            if(!isSelect) LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);
            if(isSelect) isInside = true;
            
            if(objectIndex == gm.GetProcedureIndex())
            {
                LeanTween.move(gameObject, targetPosition, 0.5f);
                
                if(isProcedureFinished) 
                {
                    if(objectType == Object.BandAid) StartCoroutine(Plester());

                    if(objectType == Object.Antiseptic) StartCoroutine(Antiseptic());

                    if(objectType == Object.Petroleum) PetroleumAnimation();

                    if(objectType == Object.Wipes) StartCoroutine(Wipes());

                    if(objectType == Object.Cream) HydrocortisoneAnimation();

                    if(objectType == Object.GauzePad) StartCoroutine(GauzePadAnimation());

                    if(objectType == Object.Bandage) StartCoroutine(GetComponent<BandageAnimation>().WrapMode());
                }
                ///summary
                ///     Wrong procedure
                ///summary

                if(!isProcedureFinished) 
                {
                    am.PlayWrongProcedureSFX();
                    if(objectType == Object.BandAid) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongBandAidKey").GetLocalizedString());
                    
                    if(objectType == Object.Antiseptic) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongAntisepticKey").GetLocalizedString());
                    
                    if(objectType == Object.Petroleum) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongPetroleumKey").GetLocalizedString());

                    if(objectType == Object.Wipes) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongWipesKey").GetLocalizedString());

                    if(objectType == Object.Cream) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongCreamKey").GetLocalizedString());

                    if(objectType == Object.GauzePad) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongGauzeKey").GetLocalizedString());

                    if(objectType == Object.Bandage) gm.ShowWrongProcedureUIForProceduralObjects(table.GetTable().GetEntry("WrongBandageKey").GetLocalizedString());

                    LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring();
                }  
            }
            else if(objectIndex > gm.GetProcedureIndex() || objectIndex < gm.GetProcedureIndex())
            {
                am.PlayWrongProcedureSFX();
                LeanTween.move(gameObject, beforeAnimatePosition, 0.8f);

                for(int i = 0; i < gm.neededObjects.Length; i++)
                {
                    if(objectIndex == gm.neededObjects[i].GetComponent<ObjectControl>().objectIndex) 
                    {
                        gm.ShowWrongProcedureUI();
                        break;
                    }
                    else
                    {
                        if(i == gm.neededObjects.Length - 1 && objectIndex != gm.neededObjects[i].GetComponent<ObjectControl>().objectIndex) 
                        {
                            gm.ShowWrongProcedureUIV2();  
                        }
                    }
                }
            }
        }
    }
    ///summary
    ///     item not close to body anymore
    ///summary
    void OnTriggerExit(Collider other)
    {
        if(isInside) isInside = false;
    }
    ///summary
    ///     After finishing what it's doing make it go back
    ///summary
    public void AfterAnimate()
    {
        if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.GauzePad && objectType != Object.Bandage) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        if(objectType == Object.Petroleum) LeanTween.rotate(gameObject, new Vector3(270.0f, 0.0f, 180.0f), 0.3f);
        if(objectType == Object.Wipes) LeanTween.rotate(gameObject, new Vector3(90.0f, 0.0f, 0.0f), 0.3f);
        if(objectType == Object.GauzePad) LeanTween.rotate(gameObject, new Vector3(-90.0f, 180.0f, 0.0f), 0.3f);
        if(objectType == Object.Bandage) LeanTween.rotate(gameObject, new Vector3(270.0f, -90.0f, 0.0f), 0.3f);

        LeanTween.move(gameObject, beforeAnimatePosition, 0.8f).setEaseSpring().setOnComplete(() => 
        {
            gm.ChangeIsAnimatingValue(false);
            canShowEffect = true;
            GetComponent<Collider>().enabled = true;
        });

        if(rb.isKinematic) rb.isKinematic = false;
     }

    ///summary
    ///     inspect pos
    ///summary
    public void Inspect() 
    {
        hoverControl?.HideHoverVisual();
        // if(useHoverControl)hoverControl?.HideHoverVisual();
        // else if(!useHoverControl)HideHoverEffect();

        intialRotation = transform.rotation.eulerAngles;

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

        LeanTween.rotate(gameObject, intialRotation,  0.3f);
        // if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.GauzePad && objectType == Object.Bandage) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        // if(objectType == Object.Petroleum) LeanTween.rotate(gameObject, new Vector3(0.0f, -180.0f, 0.0f), 0.3f);
        // if(objectType == Object.Wipes) LeanTween.rotate(gameObject, new Vector3(90.0f, 0.0f, 0.0f), 0.3f);
        // if(objectType == Object.GauzePad) LeanTween.rotate(gameObject, new Vector3(-90.0f, 180.0f, 0.0f), 0.3f);
        // if(objectType == Object.Bandage) LeanTween.rotate(gameObject, new Vector3(270.0f, -90.0f, 0.0f), 0.3f);

        LeanTween.move(gameObject, beforeInspectPosition, 0.8f).setEaseSpring();

        if(rb.isKinematic) rb.isKinematic = false;
        
        canRotate = false;
        canExamine = true;
        canMove = true;
        canShowEffect = true;
    }

    #endregion
    public void EnableCollider() => objCollider.enabled = true;

    ///summary
    ///     Hide All Buttons; Examine, Open
    ///summary  
    private void HideAllButtons() 
    {
        foreach(GameObject go in buttons)
        {
            go.SetActive(false);
        }
    }

    ///summary
    ///     Get Starting Pos of Item Before Animation
    ///summary  
    public void SetBeforeAnimatePosition() => beforeAnimatePosition = transform.position;

    public Vector3 GetBeforeAnimatePosition()
    {
        return beforeAnimatePosition;
    }

    public void ChangeIsProcedureFinishedValue() => isProcedureFinished = !isProcedureFinished;
    // public void ChangeIsAnimatingValue() => isAnimating = !isAnimating;
    
    public void ChangeCanShowEffectValue(bool value) => canShowEffect = value;

    public void CheckWinCondition()
    {
        if(isProcedureFinished) StartCoroutine(CheckCondition());
    }

    IEnumerator CheckCondition()
    {
        gm.AddProcedureObjectIndex();
        yield return new WaitForSeconds(0.1f);
        gm.CheckWinCondition();
    }

    IEnumerator Plester()
    {
        GetComponent<Collider>().enabled = false;
        canShowEffect = false;
        yield return new WaitForSeconds(0.5f);
        LeanTween.move(gameObject, new Vector3(9.66f, 1.4f, 7.44f), 0.5f);
        LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
        yield return new WaitForSeconds(0.7f);
        CheckWinCondition();
    }

    IEnumerator Antiseptic()
    {
        canShowEffect = false;
        GetComponent<Collider>().enabled = false;
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, targetPosition, 0.8f).setEaseSpring();
        LeanTween.rotateY(gameObject, -90.0f, 0.3f);
        yield return new WaitForSeconds(1.0f);
        GetComponent<Animator>().Play("Pour");
        yield return new WaitForSeconds(1.2f);
        CheckWinCondition();
    }

    private void PetroleumAnimation() => GetComponent<PetroleumJellyAnimation>().PlayAnimation();
    
    IEnumerator Wipes()
    {
        canShowEffect = false;
        GetComponent<Collider>().enabled = false;
        gm.ChangeIsAnimatingValue(true);
        LeanTween.moveY(gameObject, 1.66f, 0.3f).setDelay(0.6f);
        LeanTween.moveY(gameObject, targetPosition.y, 0.3f).setDelay(1.2f);
        LeanTween.moveY(gameObject, 1.66f, 0.3f).setDelay(1.8f);
        LeanTween.moveY(gameObject, targetPosition.y, 0.3f).setDelay(2.4f);
        yield return new WaitForSeconds(2.8f);
        GetComponent<WipesAnimation>().ChangeHandTexture();
        AfterAnimate();
        CheckWinCondition();
    }

    private void HydrocortisoneAnimation() => GetComponent<CreamAnimation>().PlayAnimation();

    IEnumerator GauzePadAnimation()
    {
        GetComponent<Collider>().enabled = false;
        canShowEffect = false;
        gm.ChangeIsAnimatingValue(true);
        yield return new WaitForSeconds(0.5f);
        // LeanTween.move(gameObject, new Vector3(6.0f, 6.0f, 3.0f), 0.5f).setOnComplete(() =>
        // {
        LeanTween.rotate(gameObject, new Vector3(-34.8f, 180.0f, 0.0f), 0.3f).setOnComplete(()  =>
        {
            transform.rotation = Quaternion.Euler(-34.8f, 180.0f, 0.0f);
            LeanTween.move(gameObject, new Vector3(6.0f, 6.0f, 4.0f), 0.5f).setOnComplete(() =>
            {
                gameObject.transform.localScale = Vector3.zero;
                gm.ChangeIsAnimatingValue(false);
                CheckWinCondition();
                GetComponent<WipesAnimation>().PlayGauzePadAnimation();
            });
        });
        // });
    } 
}
