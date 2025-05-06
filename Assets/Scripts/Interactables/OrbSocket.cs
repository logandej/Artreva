using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OrbSocket : MonoBehaviour
{
    private List<Orb> allowedOrbs = new List<Orb>();

    private Orb currentAttachedOrb;

    [SerializeField] MeshRenderer mesh;
    private Color defaultMeshColor;

    public UnityEvent eventValidate = new();
    public UnityEvent eventRefuse = new();

    public UnityEvent eventOrbInSocket = new();
    public UnityEvent eventOrbOutSocket = new();

    #region XREvents

    private void Start()
    {
        defaultMeshColor = mesh.material.color;   
    }

    public void HoverEntered(HoverEnterEventArgs args)
    {
        if(IsOrb(args.interactableObject,out Orb orb) && currentAttachedOrb==null)
        {
            //ChangeLocalScale(Vector3.one/2);
            ObjectHelper.ChangeLocalScale(this.gameObject, Vector3.one / 2);
        }
    }

    public void HoverExited(HoverExitEventArgs args)
    {
        if(IsOrb(args.interactableObject, out Orb orb) && currentAttachedOrb==null)
        {
            ObjectHelper.ChangeLocalScale(this.gameObject, Vector3.one / 3);
        }
    }

    public void SelectEntered(SelectEnterEventArgs args)
    {
        if (IsOrb(args.interactableObject, out Orb orb))
        {
            //OnSelectOrb();
            currentAttachedOrb = orb;
            eventOrbInSocket?.Invoke();
        }
    }

    public void SelectExited(SelectExitEventArgs args)
    {
        if (IsOrb(args.interactableObject, out Orb orb))
        {
            currentAttachedOrb = null;
            ObjectHelper.ChangeColor(mesh.gameObject, defaultMeshColor);
            eventOrbOutSocket?.Invoke();
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

    public void ShowIfGood()
    {
        if (CheckOrb())
        {
            ValidateOrb();
        }
        else
        {
            RefuseOrb();
        }
    }

    //Check if the orb is a good answer or not
    public bool CheckOrb()
    {
        return allowedOrbs.Contains(currentAttachedOrb);
    }

    //Validate the Orb, which means it was a good answer
    private void ValidateOrb()
    {
        eventValidate?.Invoke();
        ObjectHelper.ChangeColor(mesh.gameObject, Color.green);
    }

    //Refuse the Orb, which means it wasn't a good answer
    private void RefuseOrb()
    {
        eventRefuse?.Invoke();
        //ChangeColor(Color.red);
        ObjectHelper.ChangeColor(mesh.gameObject, Color.red);
    }



    //Does the interactable is an Orb ?
    private bool IsOrb(IXRInteractable interactable, out Orb orb)
    {
        return interactable.transform.gameObject.TryGetComponent(out orb);
    }
}
