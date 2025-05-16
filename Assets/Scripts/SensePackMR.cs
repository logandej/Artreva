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
    
    public void ActiveMR(bool active)
    {
        PXR_Manager.EnableVideoSeeThrough = active;
    }
}
