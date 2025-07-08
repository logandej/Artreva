using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OutlineEffect : MonoBehaviour
{
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.03f;

    private const string OUTLINE_OBJECT_NAME = "Outline_";

    void OnEnable()
    {
        RemoveOutlines();
        CreateOutlines();
    }

    void OnDisable()
    {
        RemoveOutlines();
    }

    void CreateOutlines()
    {
        Shader shader = Shader.Find("Custom/Outline");
        if (shader == null)
        {
            Debug.LogError("Shader 'Custom/SimpleOutline' not found.");
            return;
        }

        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            var mf = mr.GetComponent<MeshFilter>();
            if (!mf || !mf.sharedMesh) continue;

            var outlineObj = new GameObject(OUTLINE_OBJECT_NAME + mr.name);
            outlineObj.transform.SetParent(mr.transform, false);

            var outlineMF = outlineObj.AddComponent<MeshFilter>();
            var outlineMR = outlineObj.AddComponent<MeshRenderer>();

            outlineMF.sharedMesh = mf.sharedMesh;

            var mat = new Material(shader);
            mat.SetColor("_OutlineColor", outlineColor);
            mat.SetFloat("_OutlineWidth", outlineWidth);

            outlineMR.sharedMaterial = mat;

            outlineObj.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
        }
    }

    void RemoveOutlines()
    {
        foreach (var mr in GetComponentsInChildren<MeshRenderer>())
        {
            foreach (Transform child in mr.transform)
            {
                if (child.name.StartsWith(OUTLINE_OBJECT_NAME))
                {
#if UNITY_EDITOR
                    DestroyImmediate(child.gameObject);
#else
                    Destroy(child.gameObject);
#endif
                }
            }
        }
    }
}
