using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChangedCapsMesh
{
    SkinnedMeshRenderer capSkinnedMeshRenderer {get;}
    Material normalCapMaterial {get;}
    Material transparentCapMaterial {get;}

    void UpdateAlpha();
    void CapMeshChange();
}
