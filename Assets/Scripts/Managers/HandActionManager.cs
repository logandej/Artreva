using System.Collections.Generic;
using UnityEngine;

public class HandActionManager : MonoBehaviour
{
    [SerializeField] private Transform leftHandAnchor;
    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] LineRenderer leftRayRenderer;
    [SerializeField] LineRenderer rightRayRenderer;
    [SerializeField] float rayLength = 10f;

    private readonly HashSet<FarArtInteractable> previousHits = new();
    private readonly HashSet<FarArtInteractable> currentHits = new();

    private bool leftRayActive = false;
    private bool rightRayActive = false;

    void Update()
    {
        currentHits.Clear();

        if(leftRayActive) RaycastHand(leftHandAnchor, leftRayRenderer);
        else leftRayRenderer.enabled = false;
        if(rightRayActive) RaycastHand(rightHandAnchor, rightRayRenderer);
        else rightRayRenderer.enabled = false;

        // Active ceux actuellement touchés
        foreach (var obj in currentHits)
            obj.Active();

        // Désactive ceux qui ne sont plus touchés
        foreach (var obj in previousHits)
        {
            if (!currentHits.Contains(obj))
                obj.Deactive();
        }

        // Préparer pour la frame suivante
        previousHits.Clear();
        foreach (var obj in currentHits)
            previousHits.Add(obj);
    }

    private void RaycastHand(Transform hand, LineRenderer rayRenderer)
    {
        Ray ray = new(hand.position, hand.forward);
        Vector3 start = ray.origin;
        Vector3 end = start + ray.direction * rayLength;

        rayRenderer.enabled = true;

        if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        {
            end = hit.point;

            if (hit.collider.TryGetComponent(out FarArtInteractable fa))
                currentHits.Add(fa);
        }

        rayRenderer.SetPosition(0, start);
        rayRenderer.SetPosition(1, end);
    }

    public void ActiveRayLeftHand() => leftRayActive = true;
    public void StopRayLeftHand() => leftRayActive = false;
    public void ActiveRayRightHand() => rightRayActive = true;
    public void StopRayRightHand() => rightRayActive = false;
}
