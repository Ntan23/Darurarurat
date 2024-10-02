using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualInteraction : MonoBehaviour, IHover
{
    [SerializeField] protected SkinnedMeshRenderer[] meshRenderer;
    [SerializeField] protected Material[] hoverMaterial;
    [SerializeField] protected Material[] originalMaterial;
    protected bool shouldBreak;//Tanda utk break fornya

    /// <summary>
    /// Change all render material to originalMaterial
    /// </summary>
    public void HideHoverVisual()
    {
        if(shouldBreak)shouldBreak = false;
        for(int i = 0; i < meshRenderer.Length; i++)
        {
            HideHover(i);
            if(shouldBreak)
            {
                break;
            }
        }
    }
    protected abstract void HideHover(int idx);

    /// <summary>
    /// Change all render material to hoverMaterial
    /// </summary>
    public void ShowHoverVisual()
    {
        if(shouldBreak)shouldBreak = false;
        for(int i = 0; i < meshRenderer.Length; i++)
        {
            ShowHover(i);
            if(shouldBreak)
            {
                break;
            }
        }
    }

    protected abstract void ShowHover(int idx);
    
}
