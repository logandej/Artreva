using UnityEngine;
using UnityEngine.Events;

public class FarArtInteractable : MonoBehaviour
{
    [Header("Activation Settings")]
    [SerializeField] private float activationDelay = 1f;
    [SerializeField] private bool canDeactivate = true;
    [SerializeField] private float deactivationDelay = 1f;

    private bool isFocusing = false;
    protected bool isActive = false;
    private float focusTimer = 0f;

    public UnityEvent eventActivated = new();
    protected virtual void Update()
    {
        if (isFocusing)
        {
            if (!isActive)
            {
                focusTimer += Time.deltaTime;
                ObjectHelper.ChangeColorLerp(this.gameObject, Color.blue, Color.yellow, focusTimer/activationDelay);
                if (focusTimer >= activationDelay)
                {
                    ActivateNow();
                }
            }
        }
    }

    public void Active()
    {
        isFocusing = true;
    }

    public virtual void Deactive()
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
        ObjectHelper.ChangeColor(gameObject, Color.yellow);
        if(canDeactivate)
        {
            Invoke(nameof(DeactivateNow), deactivationDelay);
        }
        eventActivated?.Invoke();
    }

    public virtual void DeactivateNow()
    {
        isActive = false;
        focusTimer = 0f;
        ObjectHelper.ChangeColor(gameObject, Color.red);
    }
}
