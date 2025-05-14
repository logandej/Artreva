using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(MyCustomSubtitle))]
[TrackClipType(typeof(MyPlayableAssetScript))]
public class MyCustomTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var playable = ScriptPlayable<MyMixerBehaviour>.Create(graph, inputCount);
        var mixer = playable.GetBehaviour();

        var director = go.GetComponent<PlayableDirector>();
        if (director != null)
        {
            var binding = director.GetGenericBinding(this) as MyCustomSubtitle;
            mixer.subtitleTarget = binding;
        }

        return playable;
    }
}
