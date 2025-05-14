using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MyPlayableAssetScript : PlayableAsset, ITimelineClipAsset
{
    [TextArea(3,10)]
    public string customText;

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<MyPlayableBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.customText = customText;
        return playable;
    }
}
