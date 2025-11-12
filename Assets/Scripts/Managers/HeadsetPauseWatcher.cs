using UnityEngine;

public class HeadsetPauseWatcher : MonoBehaviour
{
    private float pauseStartRealtime = -1f;
    private float graceSeconds = 10;

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            // mémoriser l’heure (en temps réel, insensible au timeScale)
            pauseStartRealtime = Time.realtimeSinceStartup;
        }
        else
        {
            CheckPause();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        // Certains firmwares renvoient plutôt focus. Même logique :
        if (hasFocus)
        {
            CheckPause();
        }
    }

    void CheckPause()
    {
        if (pauseStartRealtime > 0f)
        {
            float pausedDuration = Time.realtimeSinceStartup - pauseStartRealtime;
            if (pausedDuration >= graceSeconds)
            {
                TriggerReset();
            }
            pauseStartRealtime = -1f;
        }
    }

    private void TriggerReset()
    {
        GameManager.Instance.ResetGame();
    }
}