using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DragonFractal : MonoBehaviour
{
    [Header("Fractal Settings")]
    public int maxIterations = 12;
    public float stepSize = 1f;

    [Header("Timing (set 0 for instant build)")]
    public float totalBuildTime = 0.5f;

    [Header("Line Appearance")]
    public float lineWidth = 0.05f;
    public Color startColor = Color.yellow;
    public Color endColor = Color.red;

    private List<Vector3> allPoints = new List<Vector3>(); // World-space points
    private List<Vector3> points = new List<Vector3>();     // Points being drawn progressively
    private LineRenderer lineRenderer;

    private bool building = false;
    private float buildStartTime;
    private int totalSegments;
    private int currentSegmentIndex = 0;

    public void Draw()
    {
        SetupLine();

        allPoints = GenerateFractalPoints();
        totalSegments = allPoints.Count - 2;

        if (totalBuildTime <= 0f)
        {
            points = new List<Vector3>(allPoints);
            UpdateLine();
        }
        else
        {
            building = true;
            buildStartTime = Time.realtimeSinceStartup;
            points.Clear();
            points.Add(allPoints[0]);
            points.Add(allPoints[1]);
            currentSegmentIndex = 2;
        }
    }

    void Start()
    {
        if (Application.isPlaying)
        {
            Draw();
        }
    }

    void Update()
    {
        if (!building)
            return;

        float elapsed = Time.realtimeSinceStartup - buildStartTime;
        float progress = Mathf.Clamp01(elapsed / totalBuildTime);
        int targetSegmentCount = Mathf.RoundToInt(progress * totalSegments);

        while (currentSegmentIndex < targetSegmentCount)
        {
            points.Add(allPoints[currentSegmentIndex]);
            currentSegmentIndex++;
        }

        UpdateLine();

        if (currentSegmentIndex >= totalSegments)
        {
            building = false;
        }
    }

    void SetupLine()
    {
        if (lineRenderer == null)
        {
            GameObject lineObj = new GameObject("DragonFractal_Line");
            lineObj.transform.parent = this.transform;
            lineRenderer = lineObj.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        lineRenderer.widthMultiplier = lineWidth;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(startColor, 0.0f),
                new GradientColorKey(endColor, 1.0f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f)
            }
        );
        lineRenderer.colorGradient = gradient;
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    private List<Vector3> GenerateFractalPoints()
    {
        List<Vector3> localPoints = new List<Vector3>();
        localPoints.Add(Vector3.zero);
        localPoints.Add(Vector3.right * stepSize);

        for (int i = 0; i < maxIterations; i++)
        {
            List<Vector3> newPoints = new List<Vector3>();
            int count = localPoints.Count;
            Vector3 pivot = localPoints[count - 1];

            for (int j = count - 2; j >= 0; j--)
            {
                Vector3 dir = localPoints[j] - localPoints[j + 1];
                Vector3 rotated = Quaternion.Euler(0, 0, 90) * dir;
                Vector3 newPoint = pivot + rotated;
                newPoints.Add(newPoint);
                pivot = newPoint;
            }

            localPoints.AddRange(newPoints);
        }

        // Convert to world space
        List<Vector3> worldPoints = new List<Vector3>();
        foreach (var p in localPoints)
            worldPoints.Add(transform.TransformPoint(p));

        return worldPoints;
    }

    void OnDrawGizmos()
    {
        List<Vector3> worldPoints = GenerateFractalPoints();

        Gizmos.color = Color.white;
        for (int i = 0; i < worldPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(worldPoints[i], worldPoints[i + 1]);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(worldPoints[0], 0.1f);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(worldPoints[worldPoints.Count - 1], 0.1f);
    }
}
