using System.Collections.Generic;
using UnityEngine;

public class HandActionManager : MonoBehaviour
{

    [Header("Hand Anchors")]
    [SerializeField] private Transform leftHandAnchor;
    [SerializeField] private Transform rightHandAnchor;

    [Header("Hand Palms")]
    [SerializeField] private Transform leftHandPalm;
    [SerializeField] private Transform rightHandPalm;

    [Header("Rays")]
    [SerializeField] GenerateSplineComputer leftRayRenderer;
    [SerializeField] GenerateSplineComputer rightRayRenderer;
    [SerializeField] float rayLength = 10f;

    private readonly HashSet<FarArtInteractable> previousHits = new();
    private readonly HashSet<FarArtInteractable> currentHits = new();

    private bool leftRayActive = false;
    private bool rightRayActive = false;

    private FarArtInteractable leftCurrentTarget;
    private FarArtInteractable rightCurrentTarget;

    void Update()
    {
        currentHits.Clear();

        if(leftRayActive) RaycastHand(leftHandPalm, leftRayRenderer);
        else { leftRayRenderer.ActiveSpline = false; }

        if(rightRayActive) RaycastHand(rightHandPalm, rightRayRenderer);
        else {  rightRayRenderer.ActiveSpline = false; }

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

    private void RaycastHand(Transform hand, GenerateSplineComputer rayRenderer)
    {
        //Ray ray = new(hand.position, hand.forward);
        //Vector3 start = ray.origin;
        //Vector3 end = start + ray.direction * rayLength;

        //rayRenderer.enabled = true;

        if (!rayRenderer.ActiveSpline)
        {
            rayRenderer.ActiveSpline = true;
        }

        FarArtInteractable hitTarget = null;

        //if (Physics.Raycast(ray, out RaycastHit hit, rayLength))
        //{
        //    end = hit.point;

        //    if (hit.collider.TryGetComponent(out FarArtInteractable fa))
        //    {
        //        currentHits.Add(fa);
        //        hitTarget = fa;
        //    }

        //}
        var target = rayRenderer.Target;
        if (rayRenderer.Target!=null)
        {
            currentHits.Add(target);
            hitTarget = target;
        }

        // Stock la cible en fonction de la main
        if (hand == leftHandPalm)
            leftCurrentTarget = hitTarget;
        else if (hand == rightHandPalm)
            rightCurrentTarget = hitTarget;
    }

    public void AnalyzeLeftTarget()
    {
        if (leftCurrentTarget is FarArtInteractableAnalyzable analyzable)
            analyzable.Analyze(leftHandAnchor);
    }

    public void AnalyzeRightTarget()
    {
        if (rightCurrentTarget is FarArtInteractableAnalyzable analyzable)
            analyzable.Analyze(rightHandAnchor);
    }

    public void ActiveRayLeftHand() => leftRayActive = true;
    public void StopRayLeftHand() => leftRayActive = false;
    public void ActiveRayRightHand() => rightRayActive = true;
    public void StopRayRightHand() => rightRayActive = false;
}
