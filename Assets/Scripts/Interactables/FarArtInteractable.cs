using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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

    [SerializeField] protected Slider slider;
    public float ActivePercent => focusTimer/activationDelay;

    private void Awake()
    {
        feedbacks = GetComponentInChildren<IInteractableFeedbacks>();
        slider.gameObject.SetActive(false);
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            slider.transform.position = renderer.bounds.center;
        }
        else
        {
            slider.transform.position = transform.position; // fallback
        }

    }

    protected virtual void Update()
    {
        if (isFocusing)
        {
            if (!isActive)
            {
                focusTimer += Time.deltaTime;
                slider.value = ActivePercent;
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
            slider.gameObject.SetActive(true);
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

    protected virtual void ActivateNow()
    {
        isActive = true;
        //ObjectHelper.ChangeColor(gameObject, Color.yellow);
        if(deactivateAfterAutoDelay)
        {
            Invoke(nameof(DeactivateNow), deactivationDelay);
        }
        feedbacks?.OnActivateStart();
        slider.gameObject.SetActive(false);
        eventActivated?.Invoke();

    }

    public virtual void DeactivateNow()
    {
        isActive = false;
        focusTimer = 0f;
        //ObjectHelper.ChangeColor(gameObject, Color.red);
        feedbacks?.OnActivateEnd();
        feedbacks?.OnHoverExit();
        slider.gameObject.SetActive(false);
    }
    
    
}
