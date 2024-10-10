using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject profitOrLossIndicator;
    [SerializeField] private GameObject background;
    [SerializeField] private ParticleSystem completeFX;
    [SerializeField] private bool isCompleteType;
    
    void UpdateAlpha(float alpha) => background.GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenUI() => StartCoroutine(OpenAnimation());

    IEnumerator OpenAnimation()
    {
        LeanTween.value(background, UpdateAlpha, 0.0f, 0.6f, 0.8f);
        yield return new WaitForSeconds(0.3f);
        LeanTween.moveLocalY(text, 0.0f, 0.8f).setEaseSpring();
        LeanTween.moveY(profitOrLossIndicator, 300.0f, 0.8f).setEaseSpring();
        if(isCompleteType) completeFX.Play();
    }
}
