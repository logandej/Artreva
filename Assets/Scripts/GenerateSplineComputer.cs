using UnityEngine;
using Dreamteck.Splines;
using UnityEngine.Rendering;

public class GenerateSplineComputer : MonoBehaviour
{
    [Header("Main Parameters")]
    public Transform handTransform;
    public float maxDistance = 10f;
    public float aimantationRadius = 0.6f;
    public LayerMask interactableLayer;
    [SerializeField] GameObject rayObjectVisual;

    private SplineComputer spline;
    private FarArtInteractable currentTarget;

    public FarArtInteractable Target => currentTarget;
    public bool ActiveSpline { get; set; } = true;

    void Start()
    {
        spline = GetComponent<SplineComputer>();
    }

    void Update()
    {
        spline.enabled = ActiveSpline;
        rayObjectVisual.SetActive(ActiveSpline);

        if (!ActiveSpline)
        {
            return;
        }



        Vector3 origin = handTransform.position;
        Vector3 direction = handTransform.forward;

        bool hasTarget = FindTarget(origin, direction, out currentTarget, out Vector3 targetPos);


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

    /// <summary>
    /// Détecte le FarArtInteractable le plus proche du rayon
    /// </summary>
    private bool FindTarget(Vector3 origin, Vector3 direction, out FarArtInteractable target, out Vector3 targetCenter)
    {
        target = null;
        targetCenter = origin + direction * maxDistance;

        RaycastHit[] hits = Physics.SphereCastAll(origin, aimantationRadius, direction, maxDistance, interactableLayer);
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
                targetCenter = hit.transform.position; // 👈 SNAP sur le centre de l’objet
            }
        }

        return target != null;
    }

    // Debug visuel de la zone d'aimantation
    void OnDrawGizmosSelected()
    {
        if (handTransform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(handTransform.position + handTransform.forward * (maxDistance / 2f), aimantationRadius);
        }
    }


}
