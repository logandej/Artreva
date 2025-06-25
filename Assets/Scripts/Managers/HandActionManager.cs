using System.Collections.Generic;
using UnityEngine;

public class HandActionManager : MonoBehaviour
{
    public static HandActionManager Instance;

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

    private bool isAnalysingRight = false;
    private bool isAnalysingLeft = false;

    private FarArtInteractable leftCurrentTarget;
    private FarArtInteractable rightCurrentTarget;

    public bool EnableHandRays { get; set; } = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject);}
    }

    private void Start()
    {
        rightRayRenderer.MaxDistance = rayLength;
        leftRayRenderer.MaxDistance = rayLength;
    }

    void Update()
    {
        currentHits.Clear();

        if (!EnableHandRays)
        {
            rightRayRenderer.ActiveSpline = false;
            leftRayRenderer.ActiveSpline = false;
            return; 
        }

        if (!isAnalysingRight)
        {
            if (rightRayActive) RaycastHand(rightHandPalm, rightRayRenderer);
            else rightRayRenderer.ActiveSpline = false;
        }

        if (!isAnalysingLeft)
        {
            if (leftRayActive) RaycastHand(leftHandPalm, leftRayRenderer);
            else leftRayRenderer.ActiveSpline = false;
        }



        // Active ceux actuellement touchés
        foreach (var obj in currentHits)
            obj.OnHoverEnter();

        // Désactive ceux qui ne sont plus touchés
        foreach (var obj in previousHits)
        {
            if (!currentHits.Contains(obj))
                obj.OnHoverExit();
        }

        // Préparer pour la frame suivante
        previousHits.Clear();
        foreach (var obj in currentHits)
            previousHits.Add(obj);
    }

    private void RaycastHand(Transform hand, GenerateSplineComputer rayRenderer)
    {

        if (!rayRenderer.ActiveSpline)
        {
            rayRenderer.ActiveSpline = true;
        }

        FarArtInteractable hitTarget = null;

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

    public void LockAnalyzable(FarArtInteractableAnalyzable farArtInteractableAnalyzable)
    {
        if(leftCurrentTarget == farArtInteractableAnalyzable)
        {
            isAnalysingLeft = true;
            leftRayRenderer.ForceTarget(farArtInteractableAnalyzable);
            leftRayRenderer.ActiveSpline = true;
            VRDebug.Instance.Log("left target");


        }
        else if(rightCurrentTarget == farArtInteractableAnalyzable)
        {
            isAnalysingRight = true;
            rightRayRenderer.ForceTarget(farArtInteractableAnalyzable);
            rightRayRenderer.ActiveSpline = true;
            VRDebug.Instance.Log("right target");

        }
        else
        {
            //Debug.Log("Not an analyzable");
            VRDebug.Instance.Log("not an analyzable");
        }
    }

    public void AnalyzeLeftTarget()
    {
        if (leftCurrentTarget is FarArtInteractableAnalyzable analyzable)
        {
            analyzable.Analyze(leftHandAnchor);
         

            analyzable.eventAnalyzed.AddListener(() => {
                isAnalysingLeft = false;
                analyzable.eventAnalyzed.RemoveAllListeners(); 
                leftRayRenderer.StopForcingTarget();
            });
        }
    }

    public void AnalyzeRightTarget()
    {
        if (rightCurrentTarget is FarArtInteractableAnalyzable analyzable)
        {
            analyzable.Analyze(rightHandAnchor);
           
            analyzable.eventAnalyzed.AddListener(() => {
                isAnalysingRight = false;
                analyzable.eventAnalyzed.RemoveAllListeners();
                rightRayRenderer.StopForcingTarget();

            });
        }
    }

    public void ActiveRayLeftHand() => leftRayActive = true;
    public void StopRayLeftHand() => leftRayActive = false;
    public void ActiveRayRightHand() => rightRayActive = true;
    public void StopRayRightHand() => rightRayActive = false;
}
