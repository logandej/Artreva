
using UnityEngine;
using UnityEngine.Playables;
using System.Reflection;

public class CallMethodBehaviour : PlayableBehaviour
{
    public GameObject targetObject;
    public string methodName;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (targetObject == null || string.IsNullOrEmpty(methodName)) return;

        foreach (var component in targetObject.GetComponents<MonoBehaviour>())
        {
            var method = component.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            if (method != null && method.GetParameters().Length == 0)
            {
                method.Invoke(component, null);
                break;
            }
        }
    }
}
