using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectFeedbacks : HoverFeedbacks, ISelectable
{
    [Header("SELECT")]
    public UnityEvent onSelectEnter;
    public UnityEvent onSelectExit;

    [Header("AudioSettings")]
    [SerializeField] AudioClip selectEnterClip;
    [SerializeField] AudioClip selectExitClip;

    protected override void Awake()
    {
        base.Awake();
        onSelectEnter.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, selectEnterClip));
        onSelectExit.AddListener(() => AudioManager.Instance.PlaySoundEffect(source, selectExitClip));
    }

    public virtual void OnSelectEnter(SelectEnterEventArgs args)  {
        if (AudioManager.Instance != null)
        {
            print("audio...");
        }
        onSelectEnter?.Invoke(); 
        print(onSelectEnter.GetPersistentEventCount()); 
    }
    public virtual void OnSelectExit(SelectExitEventArgs args) {
        if (AudioManager.Instance != null)
        {
            print("audio...");
        }
        onSelectExit?.Invoke(); 
    }
}
