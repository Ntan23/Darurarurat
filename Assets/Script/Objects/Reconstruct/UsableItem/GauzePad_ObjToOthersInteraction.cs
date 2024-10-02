using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GauzePad_ObjToOthersInteraction : ObjectToOthersInteraction
{
    [SerializeField] private Animator bodyPatientAnimator;
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
        StartCoroutine(GauzePadWiping());
    }
    IEnumerator GauzePadWiping()
    {
        coll.enabled = false;
        objControl.ChangeCanShowEffectValue(false);
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
                objControl.CheckWinCondition();
                PlayGauzePadAnimation();
            });
        });
    }
    public void PlayGauzePadAnimation() => bodyPatientAnimator.Play("ShowGauzePad");
}
