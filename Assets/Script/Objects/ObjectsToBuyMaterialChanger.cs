using UnityEngine;

public class ObjectsToBuyMaterialChanger : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] meshrenderer;

    public void UpdateMaterial(Material[] materials)
    {
        if(materials.Length > 1)
        {
            for(int i = 0; i < meshrenderer.Length; i++)
            {
                meshrenderer[i].material = materials[i];
            }
        }

        if(materials.Length == 1)
        {
            for(int i = 0; i < meshrenderer.Length; i++)
            {
                meshrenderer[i].material = materials[0];
            }
        }
    }
}
