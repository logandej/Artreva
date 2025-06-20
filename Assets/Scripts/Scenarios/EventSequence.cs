using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventGroup
{
    public List<UnityEvent> events;
}

public class EventSequence : MonoBehaviour
{

    public List<UnityEventGroup> unityEventGroups = new();

    private int currentGroupIndex = 0;
    private int currentEventIndex = 0;

    public void NextEvent()
    {
        if (currentGroupIndex >= unityEventGroups.Count) return;

        var group = unityEventGroups[currentGroupIndex];
        if (group == null || currentEventIndex >= group.events.Count) return;

        group.events[currentEventIndex]?.Invoke();
        currentEventIndex++;

    }

    public void SetGroupIndex(int index)
    {
        if (index < 0 || index >= unityEventGroups.Count) return;

        currentGroupIndex = index;
        currentEventIndex = 0;

        print("Index set" + index);
    }

}
