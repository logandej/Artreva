using System.Collections.Generic;
using UnityEngine;

public class MyMaterialOverride : MonoBehaviour
{
    public Material whiteMaterial;

    private Dictionary<Renderer, Material[]> originalMaterials;

    private void Start()
    {
        ApplyWhite();
    }

    public void ApplyWhite()
    {
        originalMaterials = new Dictionary<Renderer, Material[]>();

        foreach (var rend in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.InstanceID))
        {
            originalMaterials[rend] = rend.sharedMaterials;
            var mats = new Material[rend.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
                mats[i] = whiteMaterial;

            rend.sharedMaterials = mats;
        }

        Debug.Log("White materials applied");
    }

    public void Restore()
    {
        if (originalMaterials == null) return;

        foreach (var kvp in originalMaterials)
        {
            if (kvp.Key != null)
                kvp.Key.sharedMaterials = kvp.Value;
        }

        Debug.Log("Original materials restored");
    }
}