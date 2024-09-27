using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroleumCream : MonoBehaviour
{
    //Cream is on the hand
    [SerializeField] private GameObject creamObj;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material transparentMaterial;
    private MeshRenderer meshRenderer;

    void Start() => meshRenderer = creamObj.GetComponent<MeshRenderer>();

    private void UpdateAlpha(float alpha) => meshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);

    IEnumerator ShowCream()
    {
        LeanTween.value(creamObj, UpdateAlpha, 0.0f, 1.0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        meshRenderer.material = originalMaterial;
    }

    public void UnshowCream()
    {
        meshRenderer.material = transparentMaterial;
        LeanTween.value(creamObj, UpdateAlpha, 1.0f, 0.0f, 0.8f);
    }
}
