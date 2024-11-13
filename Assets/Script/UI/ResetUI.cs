using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUI : MonoBehaviour
{
    private void UpdateAlpha(float alpha) => GetComponent<CanvasGroup>().alpha = alpha;

    public void OpenUI() => LeanTween.value(this.gameObject, UpdateAlpha, 0.0f, 1.0f, 1.0f).setOnComplete(() => StartCoroutine(CloseUI()));

    private IEnumerator CloseUI() 
    {
        yield return new WaitForSeconds(1.0f);
        LeanTween.value(this.gameObject, UpdateAlpha, 1.0f, 0.0f, 1.0f);
    }
}
