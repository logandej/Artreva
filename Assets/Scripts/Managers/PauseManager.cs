using System.Collections.Generic;
using UnityEngine;
public enum PauseReason
{
    SettingsMenu,
    InfoPopup,
    AwaitingPlayerInput
}

public class PauseManager : MonoBehaviour
{

    private static readonly HashSet<PauseReason> activePauseReasons = new HashSet<PauseReason>();

    public static bool IsPaused => activePauseReasons.Count > 0;

    private void Start()
    {
        activePauseReasons.Clear();
        UpdatePauseState();
    }

    public static void ClearOnRestart()
    {
        activePauseReasons.Clear();
    }

    public static void Pause(PauseReason reason)
    {
        if (activePauseReasons.Add(reason))
        {
            UpdatePauseState();
        }
        else
        {
            print (" ALRDEADY A REASON WHY NOT PAUSE "+ reason);
        }
    }

    public static void Resume(PauseReason reason)
    {
        if (activePauseReasons.Remove(reason))
        {
            UpdatePauseState();
        }
    }

    private static void UpdatePauseState()
    {
        if (ScenarioManager.Instance == null)
        {
            print(" SCENARIO MANAGER NULL ");
            return;
        }
        if (IsPaused)
        {
            //Time.timeScale = 0f;
            print("paused");
            ScenarioManager.Instance.PauseTimeline();
        }
        else
        {
            print("Nopaused because " + activePauseReasons.Count);

            Time.timeScale = 1f;
            ScenarioManager.Instance.ResumeTimeline();
        }
    }
}
