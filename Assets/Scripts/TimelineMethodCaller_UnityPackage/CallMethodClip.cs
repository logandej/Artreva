
using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class CallMethodClip : PlayableAsset
{
    public ExposedReference<GameObject> targetObject;
    public string methodName;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CallMethodBehaviour>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.targetObject = targetObject.Resolve(graph.GetResolver());
        behaviour.methodName = methodName;
        return playable;
    }
}
