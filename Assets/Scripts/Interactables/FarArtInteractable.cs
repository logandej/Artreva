using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class FarArtInteractable : MonoBehaviour
{
    [Header("Activation Settings")]
    [SerializeField] private float activationDelay = 1f;
    [SerializeField] private bool deactivateAfterAutoDelay = true;
    [SerializeField] private float deactivationDelay = 1f;

    private bool isFocusing = false;
    protected bool isActive = false;
    private float focusTimer = 0f;
    private IInteractableFeedbacks feedbacks;

    public UnityEvent eventActivated = new();

    private void Awake()
    {
        feedbacks = GetComponentInChildren<IInteractableFeedbacks>();
    }

    protected virtual void Update()
    {
        if (isFocusing)
        {
            if (!isActive)
            {
                focusTimer += Time.deltaTime;
                //ObjectHelper.ChangeColorLerp(this.gameObject, Color.blue, Color.yellow, focusTimer/activationDelay);
                if (focusTimer >= activationDelay)
                {
                    ActivateNow();
                }
            }
        }
    }

    public void OnHoverEnter()
    {
        if (!isActive && !isFocusing)
        {
            isFocusing = true;
            feedbacks?.OnHoverEnter();
        }
    }

    public virtual void OnHoverExit()
    {
        isFocusing = false;
        if (!isActive)
        {
           
            focusTimer = 0f;
            DeactivateNow();
        }
    }

    protected void ActivateNow()
    {
        isActive = true;
        //ObjectHelper.ChangeColor(gameObject, Color.yellow);
        if(deactivateAfterAutoDelay)
        {
            Invoke(nameof(DeactivateNow), deactivationDelay);
        }
        feedbacks?.OnActivateStart();
        eventActivated?.Invoke();

    }

    public virtual void DeactivateNow()
    {
        isActive = false;
        focusTimer = 0f;
        //ObjectHelper.ChangeColor(gameObject, Color.red);
        feedbacks?.OnActivateEnd();
        feedbacks?.OnHoverExit();
    }
    
    
}
