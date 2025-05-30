using UnityEngine;
using UnityEngine.Events;

public class InteractableFeedbacks : MonoBehaviour, IInteractableFeedbacks
{
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    public UnityEvent onActivateStart;
    public UnityEvent onActivateEnd;

    public void OnHoverEnter() => onHoverEnter?.Invoke();
    public void OnHoverExit() => onHoverExit?.Invoke();
    public void OnActivateStart() => onActivateStart?.Invoke();
    public void OnActivateEnd() => onActivateEnd?.Invoke();
}
