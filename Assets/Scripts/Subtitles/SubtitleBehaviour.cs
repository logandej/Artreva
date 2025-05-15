using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SubtitleBehaviour : PlayableBehaviour
{
    public string customText;
    public MyCustomSubtitle subtitleTarget;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (subtitleTarget != null)
        {
            subtitleTarget.SetText(customText);
            Debug.Log("Subtitle started: " + customText);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (subtitleTarget != null)
        {
            subtitleTarget.SetText("");
            subtitleTarget.HideSubtitle();
        }
    }
}
