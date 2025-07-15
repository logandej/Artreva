using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public interface ISelectable : IHoverable
{

    void OnSelectEnter(SelectEnterEventArgs args);

    void OnSelectExit(SelectExitEventArgs args);
}
