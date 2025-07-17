using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandFeedBacks : SelectFeedbacks
{
    [Header("Hands")]
    public UnityEvent specialHoverRight;
    public UnityEvent specialHoverLeft;
    private enum Hand
    {
        None,
        Right,
        Left,
    }

    //It's the Near Far Interactor
    public override void OnHoverEnter(HoverEnterEventArgs args)
    {
        switch (IsHand(args.interactorObject.transform.gameObject))
        {
            case Hand.Left:
                specialHoverLeft?.Invoke();
                base.OnHoverEnter(args);
                break;
            case Hand.Right:
                specialHoverRight?.Invoke();
                base.OnHoverEnter(args);
                break;
            default: break;

        }
    }

    public override void OnHoverExit(HoverExitEventArgs args)
    {
        if (IsHand(args.interactorObject.transform.gameObject) != Hand.None)
        {
            base.OnHoverExit(args);
        }
    }

    public override void OnSelectEnter(SelectEnterEventArgs args)
    {
        if (IsHand(args.interactorObject.transform.gameObject) != Hand.None)
        {
            base.OnSelectEnter(args);
        }
    }

    public override void OnSelectExit(SelectExitEventArgs args)
    {
        if (IsHand(args.interactorObject.transform.gameObject) != Hand.None)
        {
            base.OnSelectExit(args);
        }
    }

    private Hand IsHand(GameObject obj)
    {
        if (obj.name.Contains("LeftHand")) { return Hand.Left; }
        if(obj.name.Contains("RightHand")) { return Hand.Right; }
        return Hand.None;
    }

}
