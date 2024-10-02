using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjInt_UsableItem_PermanentCap_ChangeCapMesh : ObjInt_UsableItem_PermanentCap, IChangedCapsMesh
{
    [Header("Change Cap Mesh")]
    [SerializeField] protected GameObject capMesh;
    protected SkinnedMeshRenderer capSkinnedMeshRenderer;
    [SerializeField] protected Material normalCapMaterial;
    [SerializeField] protected Material transparentCapMaterial;

    protected override void Start() 
    {
        base.Start();
        capSkinnedMeshRenderer = capMesh.GetComponent<SkinnedMeshRenderer>();
    }
    public virtual void UpdateAlpha(float alpha) => capSkinnedMeshRenderer.material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    public void ChangeCapMeshToNormal()
    {
        capSkinnedMeshRenderer.material = normalCapMaterial;
    }

    public void ChangeCapMeshToTransparent()
    {
        capSkinnedMeshRenderer.material = transparentCapMaterial;
    }

}
