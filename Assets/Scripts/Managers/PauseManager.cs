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

    public static void Pause(PauseReason reason)
    {
        if (activePauseReasons.Add(reason))
        {
            UpdatePauseState();
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
        if (IsPaused)
        {
            //Time.timeScale = 0f;
            ScenarioManager.Instance.PauseTimeline();
        }
        else
        {
            Time.timeScale = 1f;
            ScenarioManager.Instance.ResumeTimeline();
        }
    }
}
