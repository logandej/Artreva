using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbEnigma : MonoBehaviour
{
    [SerializeField] List<OrbSocket> orbSockets = new();
    [SerializeField] List<Orb> allowedOrbs = new();
    [SerializeField] List<Orb> notAllowedOrbs = new();

    [SerializeField] Button confirmButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private int counterOrbInSocket;
    void Start()
    {
        HideConfirmButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartEnigma()
    {
        foreach (var orbSocket in orbSockets)
        {
            orbSocket.SetAllowedOrbs(allowedOrbs);
            orbSocket.eventOrbInSocket.AddListener(AddOrbInSocket);
            orbSocket.eventOrbOutSocket.AddListener(RemoveOrbInSocket);
        }
        confirmButton.onClick.AddListener(CheckEachSocket);
    }

    private void AddOrbInSocket()
    {
        counterOrbInSocket++;
        if(counterOrbInSocket == orbSockets.Count)
        {
            ShowConfirmButton();
        }
    }
    private void RemoveOrbInSocket()
    {
        counterOrbInSocket--;
        HideConfirmButton();
        if(counterOrbInSocket <= 0)
        {
            Debug.LogError("Shouldn't happening");
        }
    }

    private void ShowConfirmButton()
    {
        confirmButton.gameObject.SetActive(true);
    }

    private void HideConfirmButton()
    {
        confirmButton.gameObject.SetActive(false);
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
        confirmButton.onClick.RemoveAllListeners();
        HideConfirmButton();
    }

    private void BadAnswer()
    {
        foreach (var orbSocket in orbSockets)
        {
            orbSocket.ShowIfGood();
        }
    }
}
