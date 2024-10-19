using UnityEngine;

public class ObjectsToBuyMaterialChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] meshrenderer;
    [SerializeField] private Material[] hoverMaterial;
    [SerializeField] private Material[] unhoverMaterial;
    [SerializeField] private BuyPanelUI buyPanelUI;

    void OnMouseEnter()
    {
        if(!buyPanelUI.GetIsOpen()) ChangeToHoverMaterial();
    }

    void OnMouseExit()
    {
        if(!buyPanelUI.GetIsOpen()) 
        {
            ChangeToUnhoverMaterial();
            UpdateMaterialColor(Color.black);
        }
    }

    void OnMouseDown()
    {
        for(int i = 0; i < meshrenderer.Length; i++) meshrenderer[i].material = hoverMaterial[i];
    }

    public void ChangeToUnhoverMaterial()
    {
        for(int i = 0; i < meshrenderer.Length; i++) meshrenderer[i].material = unhoverMaterial[i];   
    }

    public void ChangeToHoverMaterial() 
    {
        for(int i = 0; i < meshrenderer.Length; i++) meshrenderer[i].material =     hoverMaterial[i]; 
    }

    public void UpdateMaterialColor(Color32 color)
    {
        for(int i = 0; i < meshrenderer.Length; i++) meshrenderer[i].material.color = color;
    }
}
