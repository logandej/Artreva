using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrbSocket : MonoBehaviour
{
    private List<Orb> allowedOrbs = new List<Orb>();

    //private Orb currentAttachedOrb;

    [SerializeField] MeshRenderer mesh;
    private Color defaultMeshColor;

    public UnityEvent eventValidate = new();
    public UnityEvent eventRefuse = new();

    #region XREvents

    private void Start()
    {
        defaultMeshColor = mesh.material.color;   
    }

    public void HoverEntered(HoverEnterEventArgs args)
    {
        if(IsOrb(args.interactableObject,out Orb orb))
        {
            ChangeLocalScale(Vector3.one/2);
        }
    }

    public void HoverExited(HoverExitEventArgs args)
    {
        if(IsOrb(args.interactableObject, out Orb orb))
        {
            ChangeLocalScale(Vector3.one / 3);
        }
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        if (IsOrb(args.interactableObject, out Orb orb))
        {
            OnSelectOrb(orb);
        }
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        if (IsOrb(args.interactableObject, out Orb orb))
        {
            ChangeColor(defaultMeshColor);
        }
    }
    #endregion

    public void SetAllowedOrb(Orb orb)
    {
        allowedOrbs.Clear();
        allowedOrbs.Add(orb);
    }

    public void SetAllowedOrbs(List<Orb> allowedOrbs)
    {
        this.allowedOrbs = allowedOrbs;
    }

    private void OnSelectOrb(Orb orb)
    {
        if (CheckOrb(orb))
        {
            ValidateOrb();
        }
        else
        {
            RefuseOrb();
        }
    }

    //Check if the orb is a good answer or not
    private bool CheckOrb(Orb orb)
    {
        return allowedOrbs.Contains(orb);
    }

    //Validate the Orb, which means it was a good answer
    private void ValidateOrb()
    {
        eventValidate?.Invoke();
        ChangeColor(Color.green);
    }

    //Refuse the Orb, which means it wasn't a good answer
    private void RefuseOrb()
    {
        eventRefuse?.Invoke();
        ChangeColor(Color.red);
    }

    private void ChangeColor(Color color)
    {
        mesh.material.color =color;
    }

    private void ChangeLocalScale(Vector3 scale)
    {
        mesh.transform.localScale = scale;
    }

    //Does the interactable is an Orb ?
    private bool IsOrb(IXRInteractable interactable, out Orb orb)
    {
        return interactable.transform.gameObject.TryGetComponent(out orb);
    }
}
