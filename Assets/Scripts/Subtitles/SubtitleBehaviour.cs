using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SubtitleBehaviour : PlayableBehaviour
{
    public string customText;
    public MyCustomSubtitle subtitleTarget;

    private static string lastSubtitleText = "";

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (subtitleTarget != null && customText != lastSubtitleText)
        {
            subtitleTarget.SetText(customText);
            lastSubtitleText = customText;
            Debug.Log("Subtitle started: " + customText);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // Si on est arrivé à la fin du clip (lecture naturelle)
        if (subtitleTarget != null && playable.GetTime() >= playable.GetDuration())
        {
            subtitleTarget.SetText("");
            subtitleTarget.HideSubtitle();
            Debug.Log("Subtitle ended.");
            //lastSubtitleText = "";
        }
    }
}
