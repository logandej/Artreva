using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;

public class AkamiEnigma : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] List<FarArtInteractable> pillars;


    public UnityEvent eventDone = new();

    private int pillarCount = 0;

    private void Start()
    {
        Invoke(nameof(StartEnigma),5);
    }

    private void StartEnigma()
    {

        foreach (var pillar in pillars)
        {
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

        pillar.GetComponentInChildren<AudioSource>().Play();
        pillar.GetComponentInChildren<ParticleSystem>().Play();
        //pillar.transform.Find("Socle").

        if (pillarCount == pillars.Count)
        {
            Nice();
        }
    }

    public void Nice()
    {
        eventDone?.Invoke();
    }
}
