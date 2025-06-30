using UnityEngine;
using UnityEditor;

public class MissingScriptFinder
{
    [MenuItem("Tools/Find Missing Scripts In Scene")]
    public static void FindMissingScripts()
    {
        GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>(true);
        int goCount = 0;
        int missingCount = 0;

        foreach (GameObject go in gameObjects)
        {
            Component[] components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                Component c = components[i];
                if (c == null)
                {
                    string path = GetGameObjectPath(go);
                    Debug.LogWarning($"[Missing Script] GameObject: '{path}' has a missing script at component index {i}", go);
                    missingCount++;
                }
            }
            goCount++;
        }

        Debug.Log($"Analyse terminée : {goCount} objets scannés — {missingCount} scripts manquants détectés.");
    }

    private static string GetGameObjectPath(GameObject obj)
    {
        string path = obj.name;
        Transform current = obj.transform;
        while (current.parent != null)
        {
            current = current.parent;
            path = current.name + "/" + path;
        }
        return path;
    }
}