using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public interface IHoverable
{
    void OnHoverEnter(HoverEnterEventArgs args);
    void OnHoverExit(HoverExitEventArgs args);
   
}
