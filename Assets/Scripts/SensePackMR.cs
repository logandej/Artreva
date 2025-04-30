using UnityEngine;
using Unity.XR.PXR;

public class SensePackMR : MonoBehaviour
{
    private void Awake()
    {
        PXR_Manager.EnableVideoSeeThrough = true;
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
            PXR_Manager.EnableVideoSeeThrough = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
