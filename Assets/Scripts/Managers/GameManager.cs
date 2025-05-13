using Unity.XR.CoreUtils;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public XROrigin Player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }
    }

}
