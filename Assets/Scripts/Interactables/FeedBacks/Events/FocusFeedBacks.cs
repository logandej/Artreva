using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class FocusFeedBacks : HoverFeedbacks, IFocusable
{
    [Header("FOCUS")]
    public UnityEvent onFoucsEnter;
    public UnityEvent onFocusExit;

    [Header("AudioSettings")]
    [SerializeField] AudioClip focusEnterClip;
    [SerializeField] AudioClip focusExitClip;

    private void Start()
    {
        onFoucsEnter.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, focusEnterClip));
        onFocusExit.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, focusExitClip));
    }

    public virtual void OnFocusEnter(FocusEnterEventArgs args) => onFoucsEnter?.Invoke();
    public virtual void OnFocusExit(FocusExitEventArgs args) => onFocusExit?.Invoke();
}
