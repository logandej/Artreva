using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Playables;

public class SubtitleTrackMixer : PlayableBehaviour 
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        string currentText = "";
        float currentAlpha = 0;
        TMP_Text text = playerData as TMP_Text;
        if (!text) { return; }
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if(inputWeight > 0f)
            {
                ScriptPlayable<SubtitleBehaviour> inputPlayable = (ScriptPlayable<SubtitleBehaviour>)playable.GetInput(i);
                
                SubtitleBehaviour input = inputPlayable.GetBehaviour();
                currentText = input.sutbtitleText;
                currentAlpha = inputWeight;
            }
        }
        text.text = currentText;
        text.color = new Color(1, 1, 1, currentAlpha);
    }
}
