using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventDelayed : MonoBehaviour
{
    public UnityEvent events = new();

    public void Launch(int delay)
    {
        StartCoroutine(InvokeAfterDelay(delay));
    }
    private IEnumerator InvokeAfterDelay(int delay)
    {
        yield return new WaitForSeconds(delay);
        events?.Invoke();
    }
}
