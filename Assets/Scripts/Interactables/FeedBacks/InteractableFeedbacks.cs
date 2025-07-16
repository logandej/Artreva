using UnityEngine;
using UnityEngine.Events;

public class InteractableFeedbacks : MonoBehaviour, IInteractableFeedbacks
{
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;
    public UnityEvent onActivateStart;
    public UnityEvent onActivateEnd;

    public virtual void OnHoverEnter() => onHoverEnter?.Invoke();
    public virtual void OnHoverExit() => onHoverExit?.Invoke();
    public virtual void OnActivateStart() => onActivateStart?.Invoke();
    public virtual void OnActivateEnd() => onActivateEnd?.Invoke();



}
