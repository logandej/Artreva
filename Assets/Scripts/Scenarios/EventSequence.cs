using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSequence : MonoBehaviour
{

    public List<UnityEvent> steps = new();
    private int currentIndex = 0;

    public void NextEvent()
    {
        steps[currentIndex]?.Invoke();
        currentIndex++;
    }

}
