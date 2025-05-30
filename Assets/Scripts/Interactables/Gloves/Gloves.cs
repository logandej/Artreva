using UnityEngine;
using UnityEngine.Events;

public class Gloves : MonoBehaviour
{
    public UnityEvent eventTriggered = new();
    public void Show()
    {

    }

    public void Hide()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hand"))
        {
            eventTriggered?.Invoke();
        }
    }
}
