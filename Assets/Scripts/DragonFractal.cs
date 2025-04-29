using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragonFractal : MonoBehaviour
{
    [Header("Fractal Settings")]
    public int maxIterations = 12;
    public float stepSize = 1f;
    public float iterationDelay = 0.5f;
    public float segmentDrawDelay = 0.02f;

    [Header("Line Appearance")]
    public float lineWidth = 0.05f;
    public Color startColor = Color.yellow;
    public Color endColor = Color.red;

    private List<Vector3> points = new List<Vector3>();
    private LineRenderer lineRenderer;

    void Start()
    {
        Vector3 start = transform.position;
        points.Add(start);
        points.Add(start + transform.right * stepSize);

        SetupLine();
        StartCoroutine(BuildFractal());
    }

    void SetupLine()
    {
        GameObject lineObj = new GameObject("DragonFractal");
        lineObj.transform.parent = this.transform;

        lineRenderer = lineObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
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

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }

    IEnumerator BuildFractal()
    {
        for (int i = 0; i < maxIterations; i++)
        {
            List<Vector3> newPoints = GetDragonStepPoints();

            foreach (Vector3 newPoint in newPoints)
            {
                points.Add(newPoint);
                lineRenderer.positionCount = points.Count;
                lineRenderer.SetPositions(points.ToArray());
                yield return new WaitForSecondsRealtime(iterationDelay);
            }

            //yield return new WaitForSeconds(iterationDelay);
        }
        yield return null;
    }

    List<Vector3> GetDragonStepPoints()
    {
        int count = points.Count;
        Vector3 pivot = points[count - 1];
        List<Vector3> newPoints = new List<Vector3>();

        for (int i = count - 2; i >= 0; i--)
        {
            Vector3 dir = points[i] - points[i + 1];
            Vector3 rotated = Quaternion.Euler(0, 0, 90) * dir;
            Vector3 newPoint = pivot + rotated;
            newPoints.Add(newPoint);
            pivot = newPoint;
        }

        return newPoints;


    }

}
