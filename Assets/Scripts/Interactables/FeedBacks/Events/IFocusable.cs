using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public interface IFocusable : IHoverable
{
    void OnFocusEnter(FocusEnterEventArgs args);
    void OnFocusExit(FocusExitEventArgs args);
}
