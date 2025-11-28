using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRDebug : MonoBehaviour
{
    public static VRDebug Instance;

    [Header("UI Text cible pour afficher les logs")]
    public TMP_Text debugText;

    [Header("Nombre max de lignes à afficher")]
    public int maxLines = 200;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }
        if (debugText == null)
        {
            Debug.LogError("Aucun Text UI assigné pour le VRDebugConsole !");
            enabled = false;
            return;
        }

    }
    public void Log(string str)
    {
        debugText.text += str + "\n";
    }
}