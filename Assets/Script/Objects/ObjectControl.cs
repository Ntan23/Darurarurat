using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

public class ObjectControl : MonoBehaviour
{
    #region enum
    private enum Object{
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
    [SerializeField] private SkinnedMeshRenderer[] meshRenderer;
    [SerializeField] private Material[] hoverMaterial;
    [SerializeField] private Material[] originalMaterial;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private Transform takenParent;
    private Collider objCollider;
    private Rigidbody rb;
    private FirstAidBox firstAidBox;
    private GameManager gm;
    private AudioManager am;
    [SerializeField] private ObjectAnimationControl objectAnimationControl;
    #endregion

    void Start() 
    {
        gm = GameManager.instance;
        am = AudioManager.instance;

        rb = GetComponent<Rigidbody>();
        objCollider = GetComponent<Collider>();
        firstAidBox = GetComponentInParent<FirstAidBox>();

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
            HideHoverEffect();
            canHide = false;
        }
        else if(!gm.IsPausing() && !canHide) canHide = true;
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
                LeanTween.move(gameObject, new Vector3(transform.position.x, 5.0f, 0.0f), 0.8f).setEaseSpring().setOnComplete(() => SetBeforeAnimatePosition());
                
                if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.Bandage && objectType != Object.GauzePad) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
                if(objectType == Object.Petroleum) LeanTween.rotateY(gameObject, -180.0f, 0.3f);
                if(objectType == Object.Bandage) LeanTween.rotate(gameObject, new Vector3(270.0f, -90.0f, 0.0f), 0.3f);
                if(objectType == Object.GauzePad) LeanTween.rotateY(gameObject, 180.0f, 0.3f);

                if(rb.isKinematic) rb.isKinematic = false;
            }
            
            if(!gm.GetIsInInspectMode() && !gm.GetIsAnimating()) canMove = true;
        }
    }

    void OnMouseEnter()
    {
        if(gm.IsPlaying() && !gm.GetPauseMenuIsAnimating())
        {
            if(!isDragging && canExamine && !gm.GetIsInInspectMode()) ShowHoverEffect();

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
            if(canShowEffect) HideHoverEffect();

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

            if(canRotate && gm.GetIsInInspectMode())
            {
                xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
                yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

                transform.Rotate(Vector3.down, xAxisRotation);
                transform.Rotate(Vector3.right, yAxisRotation);
            }
        }
    }

    private void ShowHoverEffect()
    {
        for(int i = 0; i < meshRenderer.Length; i++)
        {
            if(objectType == Object.Antiseptic) 
            {
                AntisepticAnimation antisepticAnimation = GetComponent<AntisepticAnimation>();
                
                if(gm.GetIsAnimating())
                {
                    if(antisepticAnimation.CanAnimate())
                    {
                        if(i == 0) continue;
                        if(i == 1) meshRenderer[i].material = hoverMaterial[i];
                    }
                }
                else if(!gm.GetIsAnimating())
                {
                    if(antisepticAnimation.IsOpen())
                    {
                        if(i == 0) meshRenderer[i].material = hoverMaterial[i];
                        else if(i == 1) break;
                    }
                    else if(!antisepticAnimation.IsOpen()) meshRenderer[i].material = hoverMaterial[i];
                }
            }
            else if(objectType == Object.Cream)
            {
                CreamAnimation creamAnimation = GetComponent<CreamAnimation>();
                
                if(gm.GetIsAnimating())
                {
                    if(creamAnimation.CanAnimate())
                    {
                        if(i == 0) continue;
                        if(i == 1) meshRenderer[i].material = hoverMaterial[i];
                    }
                }
                else if(!gm.GetIsAnimating())
                {
                    if(creamAnimation.IsOpen())
                    {
                        if(i == 0) meshRenderer[i].material = hoverMaterial[i];
                        else if(i == 1) break;
                    }
                    else if(!creamAnimation.IsOpen()) meshRenderer[i].material = hoverMaterial[i];
                }
            }
            else if(objectType == Object.Wipes || objectType == Object.GauzePad)
            {
                WipesAnimation wipesAnimation = GetComponent<WipesAnimation>();

                if(!wipesAnimation.IsOpen())
                {
                    if(i == 0) continue;
                    else meshRenderer[i].material = hoverMaterial[i];
                }
                else if(wipesAnimation.IsOpen()) 
                {
                    if(i == 0) meshRenderer[i].material = hoverMaterial[i];
                    else break;
                }
            }
            else meshRenderer[i].material = hoverMaterial[i];
        }
    }

    private void HideHoverEffect()
    {
        for(int i = 0; i < meshRenderer.Length; i++)
        {
            if(objectType == Object.Antiseptic) 
            {
                AntisepticAnimation antisepticAnimation = GetComponent<AntisepticAnimation>();

                if(gm.GetIsAnimating()) 
                {
                    if(!antisepticAnimation.IsOpen()) meshRenderer[i].material = originalMaterial[i];
                    else if(antisepticAnimation.IsOpen()) 
                    {
                        if(i == 0) meshRenderer[i].material = originalMaterial[i];
                        if(i == 1 && antisepticAnimation.CanAnimate()) meshRenderer[i].material = originalMaterial[i];
                    }
                }
                if(!gm.GetIsAnimating())
                {
                    if(antisepticAnimation.IsOpen())
                    {
                        if(i == 0) meshRenderer[i].material = originalMaterial[i];
                        else if(i == 1) break;
                    }
                    else if(!antisepticAnimation.IsOpen()) meshRenderer[i].material = originalMaterial[i];
                }
            }
            else if(objectType == Object.Cream)
            {
                CreamAnimation creamAnimation = GetComponent<CreamAnimation>();

                if(gm.GetIsAnimating()) 
                {
                    if(!creamAnimation.IsOpen()) meshRenderer[i].material = originalMaterial[i];
                    else if(creamAnimation.IsOpen()) 
                    {
                        if(i == 0) meshRenderer[i].material = originalMaterial[i];
                        if(i == 1 && creamAnimation.CanAnimate()) meshRenderer[i].material = originalMaterial[i];
                    }
                }
                if(!gm.GetIsAnimating())
                {
                    if(creamAnimation.IsOpen())
                    {
                        if(i == 0) meshRenderer[i].material = originalMaterial[i];
                        else if(i == 1) break;
                    }
                    else if(!creamAnimation.IsOpen()) meshRenderer[i].material = originalMaterial[i];
                }
            }
            else if(objectType == Object.Wipes || objectType == Object.GauzePad)
            {
                WipesAnimation wipesAnimation = GetComponent<WipesAnimation>();

                if(!wipesAnimation.IsOpen())
                {
                    if(i == 0) continue;
                    else meshRenderer[i].material = originalMaterial[i];
                }
                else if(wipesAnimation.IsOpen()) 
                {
                    if(i == 0) meshRenderer[i].material = originalMaterial[i];
                    else break;
                }
            }
            else meshRenderer[i].material = originalMaterial[i];
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

                    if(objectType == Object.Petroleum) GetComponent<PetroleumJellyAnimation>().PlayAnimation();

                    if(objectType == Object.Wipes) StartCoroutine(Wipes());

                    if(objectType == Object.Cream) GetComponent<CreamAnimation>().PlayAnimation();

                    if(objectType == Object.GauzePad) GauzePadAnimation();

                    if(objectType == Object.Bandage) GetComponent<BandageAnimation>().WrapMode();
                }
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
            else if(objectIndex > gm.GetProcedureIndex())
            {
                am.PlayWrongProcedureSFX();
                LeanTween.move(gameObject, beforeAnimatePosition, 0.5f);
                gm.ShowWrongProcedureUI();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(isInside) isInside = false;
    }

    public void AfterAnimate()
    {
        if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.GauzePad && objectType != Object.Bandage) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        if(objectType == Object.Petroleum) LeanTween.rotate(gameObject, new Vector3(0.0f, -180.0f, 0.0f), 0.3f);
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

    public void Inspect() 
    {
        HideHoverEffect();

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

        if(objectType != Object.Wipes && objectType != Object.Petroleum && objectType != Object.GauzePad && objectType == Object.Bandage) LeanTween.rotate(gameObject, Vector3.zero, 0.3f);
        if(objectType == Object.Petroleum) LeanTween.rotate(gameObject, new Vector3(0.0f, -180.0f, 0.0f), 0.3f);
        if(objectType == Object.Wipes) LeanTween.rotate(gameObject, new Vector3(90.0f, 0.0f, 0.0f), 0.3f);
        if(objectType == Object.GauzePad) LeanTween.rotate(gameObject, new Vector3(-90.0f, 180.0f, 0.0f), 0.3f);
        if(objectType == Object.Bandage) LeanTween.rotate(gameObject, new Vector3(270.0f, -90.0f, 0.0f), 0.3f);

        if(rb.isKinematic) rb.isKinematic = false;
        
        canRotate = false;
        canExamine = true;
        canMove = true;
        canShowEffect = true;
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
    
    public void ChangeCanShowEffectValue(bool value) => canShowEffect = value;

    public void CheckWinCondition()
    {
        if(gm.GetProcedureIndex() <= gm.objects.Length && isProcedureFinished) StartCoroutine(CheckCondition());
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

    private void GauzePadAnimation()
    {
        canShowEffect = false;
        GetComponent<Collider>().enabled = false;
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, new Vector3(6.0f, 7.0f, 3.0f), 0.5f).setOnComplete(() =>
        {
            transform.rotation = Quaternion.Euler(45.0f, 0.0f, -180.0f);
            //LeanTween.rotate(gameObject, new Vector3(45.0f, 0.0f, 0.0f), 0.2f);
            LeanTween.move(gameObject, new Vector3(6.0f, 6.0f, 4.0f), 0.5f).setOnComplete(() =>
            {
                LeanTween.scale(gameObject, Vector3.zero, 0.2f);
                gm.ChangeIsAnimatingValue(false);
                CheckWinCondition();
                GetComponent<WipesAnimation>().PlayGauzePadAnimation();
            });
        });
    }

    
}
