using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRSimpleNoParent : MonoBehaviour
{
    private Transform originalParent;

    void Awake()
    {
        originalParent = transform.parent;
    }

    void LateUpdate()
    {
        if (transform.parent != originalParent)
            transform.SetParent(originalParent, true);
    }
}
