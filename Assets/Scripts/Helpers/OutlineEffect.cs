using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class OutlineEffect : MonoBehaviour
{
    public Material outlineMaterial;

    private List<Renderer> renderers = new();
    private Dictionary<Renderer, Material[]> originalMats = new();

    private void OnEnable()
    {
        ApplyOutline();
    }

    private void OnDisable()
    {
        RemoveOutline();
    }

    private void ApplyOutline()
    {
        renderers.Clear();
        originalMats.Clear();

        GetComponentsInChildren(true, renderers);

        foreach (var r in renderers)
        {
            if (r == null || outlineMaterial == null) continue;

            var mats = new List<Material>(r.sharedMaterials);
            if (!mats.Contains(outlineMaterial))
            {
                originalMats[r] = r.sharedMaterials;
                mats.Add(outlineMaterial);
                r.sharedMaterials = mats.ToArray();
            }
        }
    }

    private void RemoveOutline()
    {
        foreach (var kvp in originalMats)
        {
            if (kvp.Key != null)
            {
                kvp.Key.sharedMaterials = kvp.Value;
            }
        }
    }
}

