using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHover
{
    /// <summary>
    /// If Mouse Hover, Visual Change
    /// </summary>
    
    // MeshRenderer MeshRenderer { get; }
    // Material OriginalMat {get;}
    // Material HoverMat {get;}
    void ShowHoverVisual();
    void HideHoverVisual();

}
