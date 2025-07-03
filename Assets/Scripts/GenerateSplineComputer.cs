using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.Rendering;

public class GenerateSplineComputer : MonoBehaviour
{
    [Header("Main Parameters")]
    public Transform handTransform;
    public float MaxDistance { get; set; } = 10f;
    public float aimantationRadius = 0.6f;
    public LayerMask interactableLayer;
    [SerializeField] ParticleController rayObjectVisual;

    private SplineComputer spline;
    private FarArtInteractable currentTarget;

    public FarArtInteractable Target => currentTarget;
    private bool isForcingTarget = false;
    public bool ActiveSpline { get; set; } = true;

    void Start()
    {
        spline = GetComponent<SplineComputer>();
    }

    void Update()
    {


        if (!handTransform.gameObject.activeSelf)
        {
            ActiveSpline = false;
        }

        spline.enabled = ActiveSpline;
        rayObjectVisual.gameObject.SetActive(ActiveSpline);

        if (!ActiveSpline)
        {
            return;
        }



        Vector3 origin = handTransform.position;
        Vector3 direction = handTransform.forward;

        bool hasTarget = true;
        Vector3 targetPos;

        if (!isForcingTarget)
        {
            hasTarget = FindTarget(origin, direction, out currentTarget, out targetPos);
        }
        else
        {
           targetPos = GetTargetPos(currentTarget.transform);
        }


        //SetDistanceParticule;
        float targetDistance = hasTarget ? 1f : 0.05f;

        if (Mathf.Abs(targetDistance - lastTargetDistance) > transitionThreshold)
        {
            ChangeRayParticleDistance(targetDistance);
            lastTargetDistance = targetDistance;
        }



        SplinePoint[] points = new SplinePoint[2];

        // POINT MAIN
        points[0] = new SplinePoint();
        points[0].position = origin;

        // POINT CIBLE
        points[1] = new SplinePoint();
        points[1].position = targetPos;

        if (hasTarget)
        {

            // Tangente de la main : toujours dans le sens du hand.forward
            points[0].tangent2 = origin + direction * 0.5f;

            // Tangente de la cible : vers la main
            Vector3 towardOrigin = (origin - targetPos).normalized;
            points[1].tangent = targetPos + towardOrigin * 0.5f;
        }
        else
        {

            // Droite tendue : tangentes nulles
            points[0].tangent2 = origin;
            points[1].tangent = targetPos;

         
        }

        spline.SetPoints(points);
    }

    private float lastTargetDistance = -1f; // Valeur de référence pour éviter les appels inutiles
    private float transitionThreshold = 0.01f;
    private void ChangeRayParticleDistance(float distance)
    {
        
        float startClip = (float)rayObjectVisual.clipTo;

        TransitionManager.InterpolateFloat(startClip, distance, .5f, t =>
        {
            rayObjectVisual.clipTo = t;
        });
    }

    /// <summary>
    /// Détecte le FarArtInteractable le plus proche du rayon
    /// </summary>
    private bool FindTarget(Vector3 origin, Vector3 direction, out FarArtInteractable target, out Vector3 targetCenter)
    {
        target = null;
        targetCenter = origin + direction * MaxDistance;

        RaycastHit[] hits = Physics.SphereCastAll(origin, aimantationRadius, direction, MaxDistance, interactableLayer);
        float closestDistance = float.MaxValue;

        foreach (RaycastHit hit in hits)
        {
            float distance = hit.distance;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                if(hit.transform.TryGetComponent(out FarArtInteractable farArtInteractable))
                {
                    target = farArtInteractable;
                }
                //targetCenter = hit.transform.position; // SNAP sur le centre de l’objet
                targetCenter = GetTargetPos(hit.transform);
            }
        }

        return target != null;
    }

    private Vector3 GetTargetPos(Transform transform)
    {
        Renderer renderer = transform.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
           return renderer.bounds.center;
        }
        else
        {
           return transform.position; // fallback
        }
    }

    public void ForceTarget(FarArtInteractable target)
    {
        isForcingTarget = true;
        currentTarget = target;
    }

    public void StopForcingTarget()
    {
        currentTarget = null;
        isForcingTarget = false;

    }

    // Debug visuel de la zone d'aimantation
    void OnDrawGizmosSelected()
    {
        if (handTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(handTransform.position + handTransform.forward * (MaxDistance / 2f), aimantationRadius);
        }
    }


}
