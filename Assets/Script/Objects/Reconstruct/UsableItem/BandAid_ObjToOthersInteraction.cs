using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAid_ObjToOthersInteraction : ObjectToOthersInteraction
{
    private ObjectControl objControl;
    private Collider coll;
    private void Start()
    {
        objControl = GetComponent<ObjectControl>();
        coll = GetComponent<Collider>();
    }
    public override void InteractionWithPatient()
    {
        StartCoroutine(BandageNow());
    }
    IEnumerator BandageNow()
    {
        coll.enabled = false;
        objControl.ChangeCanShowEffectValue(false);
        yield return new WaitForSeconds(0.5f);
        LeanTween.move(gameObject, new Vector3(9.66f, 1.4f, 7.44f), 0.5f);
        LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0.3f), 0.5f);
        yield return new WaitForSeconds(0.7f);
        objControl.CheckWinCondition();
    }
}
