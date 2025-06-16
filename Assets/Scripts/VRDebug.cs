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

    private Queue<string> logLines = new Queue<string>();

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

        //Application.logMessageReceived += HandleLog;
    }

    //void OnDestroy()
    //{
    //    Application.logMessageReceived -= HandleLog;
    //}

    //void HandleLog(string logString, string stackTrace, LogType type)
    //{
    //    if (logLines.Count >= maxLines)
    //        logLines.Dequeue();

    //    logLines.Enqueue(logString);
    //    debugText.text = string.Join("\n", logLines);
    //}

    public void Log(string str)
    {
        debugText.text += str + "\n";
    }
}