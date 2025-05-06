using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class GizmoSphereDrawer : MonoBehaviour
{
    [System.Serializable]
    public class GizmoSphere
    {
        public string name;
        public Color color = Color.white;
        public float radius = 1f;
        public Vector3 offset = Vector3.zero;
    }

    public List<GizmoSphere> spheres = new List<GizmoSphere>();

    private void OnDrawGizmos()
    {
        foreach (var sphere in spheres)
        {
            if (sphere == null) continue;

            Gizmos.color = sphere.color;
            Gizmos.DrawWireSphere(transform.position + sphere.offset, sphere.radius);

#if UNITY_EDITOR
            UnityEditor.Handles.color = sphere.color;
            UnityEditor.Handles.Label(transform.position + sphere.offset, sphere.name);
#endif
        }
    }
}
