using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("FirstEnigma")]
    [SerializeField] OrbSocket OrbDateSocket;
    [SerializeField] Orb orbDate;
    void Start()
    {
        OrbDateSocket.SetAllowedOrb(orbDate);       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
