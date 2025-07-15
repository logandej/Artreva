using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverFeedbacks : MonoBehaviour, IHoverable
{
    [Header("HOVER")]
    public UnityEvent onHoverEnter;
    public UnityEvent onHoverExit;

    [Header("AudioSettings")]
    [SerializeField] protected AudioSource source;
    [SerializeField] AudioClip hoverEnterClip;
    [SerializeField] AudioClip hoverExitClip;

    private void Start()
    {
        onHoverEnter.AddListener(()=>AudioManager.Instance.PlaySoundEffect(source,hoverEnterClip));
        onHoverExit.AddListener(()=>AudioManager.Instance.PlaySoundEffect(source,hoverExitClip));
    }
    public virtual void OnHoverEnter(HoverEnterEventArgs args) => onHoverEnter?.Invoke();
    public virtual void OnHoverExit(HoverExitEventArgs args) => onHoverExit?.Invoke();
}
