using UnityEngine;
using UnityEngine.Playables;


public class MyMixerBehaviour : PlayableBehaviour
{
    public MyCustomSubtitle subtitleTarget;

    public override void OnGraphStart(Playable playable)
    {
        // Inject subtitleTarget into all inputs
        int inputCount = playable.GetInputCount();

        for (int i = 0; i < inputCount; i++)
        {
            var inputPlayable = (ScriptPlayable<MyPlayableBehaviour>)playable.GetInput(i);
            var behaviour = inputPlayable.GetBehaviour();
            behaviour.subtitleTarget = subtitleTarget;
        }
    }
}