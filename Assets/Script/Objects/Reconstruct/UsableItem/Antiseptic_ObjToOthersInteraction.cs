using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antiseptic_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private GameManager gm;
    private Antiseptic_ObjIntUsable antiseptic_ObjIntUsable;
    private ObjectControl objControl;
    private Collider coll;
    [SerializeField]private Vector3 targetHandPosition;
    private void Start()
    {
        gm = GameManager.instance;
        antiseptic_ObjIntUsable = GetComponent<Antiseptic_ObjIntUsable>();
        objControl = GetComponent<ObjectControl>();
        coll = GetComponent<Collider>();
    }
    public override void InteractionWithPatient()
    {
        StartCoroutine(AntisepticWithPatientHand());
    }
    IEnumerator AntisepticWithPatientHand()
    {
        objControl.ChangeCanShowEffectValue(false);
        coll.enabled = false;
        gm.ChangeIsAnimatingValue(true);
        LeanTween.move(gameObject, targetHandPosition, 0.8f).setEaseSpring();
        LeanTween.rotateY(gameObject, -90.0f, 0.3f);
        yield return new WaitForSeconds(1.0f);
        antiseptic_ObjIntUsable.PourAntiseptic();
        yield return new WaitForSeconds(1.2f);
        objControl.CheckWinCondition();
    }
}
