using UnityEngine;

public class FarArtInteractable : MonoBehaviour
{
    [Header("Activation Settings")]
    public float activationDelay = 1f;
    public bool canDeactivate = true;
    public float deactivationDelay = 1f;

    private bool isFocusing = false;
    private bool isActive = false;
    private float focusTimer = 0f;

    void Update()
    {
        if (isFocusing)
        {
            if (!isActive)
            {
                focusTimer += Time.deltaTime;
                ObjectHelper.ChangeColorLerp(this.gameObject, Color.blue, Color.green, focusTimer/activationDelay);
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

    public void Deactive()
    {
        isFocusing = false;
        if (!isActive)
        {
            focusTimer = 0f;
            DeactivateNow();
        }
    }

    private void ActivateNow()
    {
        isActive = true;
        ObjectHelper.ChangeColor(gameObject, Color.yellow);
        if(canDeactivate)
        {
            Invoke(nameof(DeactivateNow), deactivationDelay);
        }
    }

    private void DeactivateNow()
    {
        isActive = false;
        focusTimer = 0f;
        ObjectHelper.ChangeColor(gameObject, Color.red);
    }
}
