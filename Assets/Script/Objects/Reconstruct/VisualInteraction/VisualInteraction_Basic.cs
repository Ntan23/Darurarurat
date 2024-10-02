using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualInteraction_Basic : VisualInteraction
{
    protected override void HideHover(int idx)
    {
        meshRenderer[idx].material = originalMaterial[idx];
    }

    protected override void ShowHover(int idx)
    {
        meshRenderer[idx].material = hoverMaterial[idx];
    }
}
