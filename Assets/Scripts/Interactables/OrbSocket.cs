using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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
        if (IsOrb(args.interactableObject, out Orb orb) && currentAttachedOrb==null)
        {
            //OnSelectOrb();
            //ForceSelectExit(args.interactableObject.GetOldestInteractorSelecting(), args.interactableObject);

            currentAttachedOrb = orb;
            eventOrbInSocket?.Invoke();
        }
    }


    public void SelectExited(SelectExitEventArgs args)
    {
        if (IsOrb(args.interactableObject, out Orb orb) && currentAttachedOrb != null)
        {

            currentAttachedOrb = null;
            //ObjectHelper.ChangeColor(mesh.gameObject, defaultMeshColor);
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
        CanTakeOrb(false);
        return allowedOrbs.Contains(currentAttachedOrb);
    }

    //Validate the Orb, which means it was a good answer
    private void ValidateOrb()
    {
        eventValidate?.Invoke();
        //ObjectHelper.ChangeColor(mesh.gameObject, Color.green);
    }

    //Refuse the Orb, which means it wasn't a good answer
    private void RefuseOrb()
    {
        eventRefuse?.Invoke();
        //ChangeColor(Color.red);
        //ObjectHelper.ChangeColor(mesh.gameObject, Color.red);
    }

    public void StopEnigma()
    {
        Invoke(nameof(Hide),1);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    //Does the interactable is an Orb ?
    private bool IsOrb(IXRInteractable interactable, out Orb orb)
    {
        return interactable.transform.gameObject.TryGetComponent(out orb);
    }

    public void CanTakeOrb(bool canTake)
    {
        if (true)//currentAttachedOrb != null)
        {
            //currentAttachedOrb.GetComponent<XRGrabInteractable>().select
            //GetComponent<XRSocketInteractor>().socketActive = canTake;
            VRDebug.Instance.Log("CAN TAKE = "+canTake);
        }
    }



    public void ForceSelectExit(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
    {
        if (interactor == null || interactor.interactablesSelected.Count == 0)
            return;

        // Construction des arguments de l'événement
        var args = new SelectExitEventArgs
        {
            interactorObject = interactor,
            interactableObject = interactable,
            manager = null, // Tu n’as pas d’InteractionManager ici
            isCanceled = true // Tu peux mettre true ou false selon le contexte
        };

        // Appelle les méthodes internes pour simuler l'événement
        interactable.OnSelectExiting(args);
        interactor.OnSelectExiting(args);

        interactable.OnSelectExited(args);
        interactor.OnSelectExited(args);
    }
}
