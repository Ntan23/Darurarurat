using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangedCapsMesh
{
    // [SerializeField] private GameObject capMesh;
    // SkinnedMeshRenderer capSkinnedMeshRenderer {get;}
    // Material normalCapMaterial {get;}
    // Material transparentCapMaterial {get;}

    void UpdateAlpha(float alpha);
    void ChangeCapMeshToNormal();
    void ChangeCapMeshToTransparent();
}
