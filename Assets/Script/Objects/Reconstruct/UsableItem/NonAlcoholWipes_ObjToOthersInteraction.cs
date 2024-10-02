using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class NonAlcoholWipes_ObjToOthersInteraction : ObjectToOthersInteraction
{
    [SerializeField] private MeshRenderer skinMesh;
    [SerializeField] private Material cleanMaterial;
    [SerializeField]private Vector3 targetWipePosition;
    private GameManager gm;
    private ObjectControl objControl;
    private Collider coll;
    private void Start()
    {
        gm = GameManager.instance;
        objControl = GetComponent<ObjectControl>();
        coll = GetComponent<Collider>();
    }
    public override void InteractionWithPatient()
    {
        StartCoroutine(Wiping());
    }
    IEnumerator Wiping()
    {
        objControl.ChangeCanShowEffectValue(false);
        coll.enabled = false;
        gm.ChangeIsAnimatingValue(true);
        LeanTween.moveY(gameObject, 1.66f, 0.3f).setDelay(0.6f);
        LeanTween.moveY(gameObject, targetWipePosition.y, 0.3f).setDelay(1.2f);
        LeanTween.moveY(gameObject, 1.66f, 0.3f).setDelay(1.8f);
        LeanTween.moveY(gameObject, targetWipePosition.y, 0.3f).setDelay(2.4f);
        yield return new WaitForSeconds(2.8f);
        ChangeToCleanTexture();
        objControl.AfterAnimate();
        objControl.CheckWinCondition();
    }
    private void ChangeToCleanTexture()=> skinMesh.material = cleanMaterial;
}
