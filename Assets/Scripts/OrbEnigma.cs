using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OrbEnigma : MonoBehaviour
{
    [SerializeField] List<OrbSocket> orbSockets = new();
    [SerializeField] List<Orb> goodOrbs = new();
    [SerializeField] List<Orb> badOrbs = new();
    [SerializeField] bool startEnigmaOnStart = false;
    //[SerializeField] Button confirmButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public UnityEvent eventGoodAnswer = new();
    public UnityEvent eventBadAnswer = new();
    private int counterOrbInSocket = 0;

    private bool enigmaActive = false;
    void Start()
    {
        HideConfirmButton();
        foreach (var orbSocket in orbSockets)
        {
            orbSocket.gameObject.SetActive(false);
        }

        if (startEnigmaOnStart)
        {
            StartEnigma();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEnigma()
    {
        if (enigmaActive) return;
        foreach (var orb in goodOrbs)
        {
            orb.StartOrbForEnigma();
        }

        foreach (var orb in badOrbs)
        {
            orb.StartOrbForEnigma();
        }
        foreach (var orbSocket in orbSockets)
        {
            orbSocket.gameObject.SetActive(true);
            orbSocket.SetAllowedOrbs(goodOrbs);
            orbSocket.eventOrbInSocket.AddListener(AddOrbInSocket);
            orbSocket.eventOrbOutSocket.AddListener(RemoveOrbInSocket);
        }

        enigmaActive = true;


        //confirmButton.onClick.AddListener(CheckEachSocket);
    }

    private void AddOrbInSocket()
    {
        counterOrbInSocket++;
        if(counterOrbInSocket == orbSockets.Count)
        {
            //ShowConfirmButton();
            CheckEachSocket();
        }
    }
    private void RemoveOrbInSocket()
    {
        counterOrbInSocket--;
        HideConfirmButton();
        if(counterOrbInSocket < 0)
        {
            Debug.LogError("Shouldn't happening");
        }
    }

    private void ShowConfirmButton()
    {
        //confirmButton.gameObject.SetActive(true);
    }

    private void HideConfirmButton()
    {
        //confirmButton.gameObject.SetActive(false);
    }

    private void CheckEachSocket()
    {
        int correct = 0;
        foreach (var orb in orbSockets)
        {
            if(orb.CheckOrb()) correct++;
        }

        if(correct == orbSockets.Count)
        {
            GoodAnswer();
        }
        else
        {
            BadAnswer();
        }
    }

    private void GoodAnswer()
    {
        eventGoodAnswer?.Invoke();

        foreach (var orb in GetAllOrbs())
        {
            orb.Invoke(nameof(orb.StopEnigma),2);
        }

        foreach (var orb in orbSockets)
        {
            orb.StopEnigma();
        }

        enigmaActive = false;

    }

    private List<Orb> GetAllOrbs()
    {
        return new List<Orb> (badOrbs.Concat(goodOrbs));
    }

    private void BadAnswer()
    {
        eventBadAnswer?.Invoke();

        StartCoroutine(BadAnswerCoroutine());
    }

    IEnumerator BadAnswerCoroutine()
    {
        yield return new WaitForSeconds(1);
        foreach (var orbSocket in orbSockets)
        {
            orbSocket.CanTakeOrb(true);
        }
    }
}
