using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("FirstEnigma")]
    [SerializeField] List<OrbEnigma> orbEnigma;
    void Start()
    {
        //foreach (var enigma in orbEnigma)
        //{
        //    enigma.StartEnigma();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
