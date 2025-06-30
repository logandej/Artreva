using UnityEngine;
using UnityEditor;
using System.IO;

public class MissingScriptAssetScanner
{
    [MenuItem("Tools/Find Missing Scripts In Assets")]
    public static void FindMissingScriptsInAssets()
    {
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();
        int missingCount = 0;

        foreach (string path in assetPaths)
        {
            if (!path.StartsWith("Assets/")) continue;

            Object asset = AssetDatabase.LoadMainAssetAtPath(path);
            if (asset is GameObject go)
            {
                Component[] components = go.GetComponentsInChildren<Component>(true);
                for (int i = 0; i < components.Length; i++)
                {
                    if (components[i] == null)
                    {
                        Debug.LogWarning($"[Missing Script] In asset: {path}, GameObject: {go.name}");
                        missingCount++;
                        break;
                    }
                }
            }
        }

        Debug.Log($"Scan complet : {missingCount} asset(s) contenant des scripts manquants.");
    }
}