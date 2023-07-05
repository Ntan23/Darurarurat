using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionCompleteUI : MonoBehaviour
{
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject background;
    [SerializeField] private ParticleSystem completeFX;
    
    void UpdateAlpha(float alpha) => background.GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenUI() => StartCoroutine(OpenAnimation());

    IEnumerator OpenAnimation()
    {
        LeanTween.value(background, UpdateAlpha, 0.0f, 0.6f, 0.8f);
        yield return new WaitForSeconds(0.3f);
        LeanTween.moveLocalY(text, 0.0f, 0.8f).setEaseSpring();
        completeFX.Play();
    }
}
