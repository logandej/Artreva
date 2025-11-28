using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

public class FindUnusedMaterials
{
    [MenuItem("Tools/Find Unused Materials")]
    public static void Find()
    {
        // Tous les assets Material
        var allMaterialsGUIDs = AssetDatabase.FindAssets("t:Material");
        var used = new HashSet<string>();

        // Racines : scènes + prefabs + scriptable objects
        var sceneGUIDs = AssetDatabase.FindAssets("t:Scene");
        var prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        var scriptableGUIDs = AssetDatabase.FindAssets("t:ScriptableObject");

        string[] roots = sceneGUIDs
            .Concat(prefabGUIDs)
            .Concat(scriptableGUIDs)
            .ToArray();

        foreach (var guid in roots)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var deps = AssetDatabase.GetDependencies(path, true);

            foreach (var d in deps)
                used.Add(d);
        }

        int unusedCount = 0;

        foreach (var guid in allMaterialsGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            if (!used.Contains(path))
            {
                Debug.Log("Unused Material: " + path, AssetDatabase.LoadAssetAtPath<Material>(path));
                unusedCount++;
            }
        }

        Debug.Log("Scan terminé. Matériaux inutilisés : " + unusedCount);
    }
}