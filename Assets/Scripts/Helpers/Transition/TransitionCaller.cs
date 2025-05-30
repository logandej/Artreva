using UnityEngine;

public class TransitionCaller : MonoBehaviour
{
    public GameObject targetObject;
    public Transform targetTransform;
    public float transitionDuration = 1f;

    public void TriggerTransition()
    {
        Transition(targetObject, targetTransform, transitionDuration);
    }

    public void Transition(GameObject obj, Transform transform, float duration)
    {   
        TransitionManager.ChangeTransform(obj, transform, duration);
    }
}
