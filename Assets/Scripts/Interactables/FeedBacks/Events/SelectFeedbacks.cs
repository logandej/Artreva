using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectFeedbacks : HoverFeedbacks, ISelectable
{
    [Header("SELECT")]
    public UnityEvent onSelectEnter = new();
    public UnityEvent onSelectExit = new();

    [Header("AudioSettings")]
    [SerializeField] AudioClip selectEnterClip;
    [SerializeField] AudioClip selectExitClip;

    private void Start()
    {
        onSelectEnter.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, selectEnterClip));
        onSelectExit.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, selectExitClip));
    }

    public virtual void OnSelectEnter(SelectEnterEventArgs args) => onSelectEnter?.Invoke();
    public virtual void OnSelectExit(SelectExitEventArgs args) => onSelectExit?.Invoke();
}
