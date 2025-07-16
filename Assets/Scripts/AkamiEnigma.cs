using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

public class AkamiEnigma : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<FarArtInteractable> pillars;
    private List<UnityEvent> savedActions;
    public UnityEvent eventDone = new();

    private int pillarCount = 0;

    private void Start()
    {
        StartEnigma();
    }

    private void StartEnigma()
    {

        foreach (var pillar in pillars)
        {
            //Save the normal action for later if it's the food pillar
            savedActions[pillars.IndexOf(pillar)] = pillar.eventActivated;
            pillar.eventActivated.RemoveAllListeners();
            pillar.eventActivated.AddListener(() => CheckIfGoodPillar(pillar));
        }
    }

    private void CheckIfGoodPillar(FarArtInteractable pillar)
    {
        int index = pillars.IndexOf(pillar);
        if (index != pillarCount)
        {
            pillar.DeactivateNow();
            return;
        }
        pillarCount++;
        savedActions[pillars.IndexOf(pillar)]?.Invoke();
        if(pillarCount == pillars.Count)
        {
            Nice();
        }
    }

    public void Nice()
    {
        eventDone?.Invoke();
    }
}
