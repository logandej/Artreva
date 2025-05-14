using UnityEngine;
using UnityEngine.Playables;


public class SubtitleMixer : PlayableBehaviour
{
    public MyCustomSubtitle subtitleTarget;

    public override void OnGraphStart(Playable playable)
    {
        // Inject subtitleTarget into all inputs
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            var inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
            var behaviour = inputPlayable.GetBehaviour();
            behaviour.subtitleTarget = subtitleTarget;
        }
    }
}