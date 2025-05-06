using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class GizmoCircleDrawer : MonoBehaviour
{
    [System.Serializable]
    public class GizmoCircle
    {
        public string name;
        public Color color = Color.white;
        public float radius = 1f;
    }

    public List<GizmoCircle> circles = new List<GizmoCircle>();

    private void OnDrawGizmos()
    {
        foreach (var circle in circles)
        {
            if (circle == null) continue;

            Gizmos.color = circle.color;
            DrawCircle(transform.position, circle.radius);
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments = 64)
    {
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0, Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float rad = Mathf.Deg2Rad * angleStep * i;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * radius;
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}