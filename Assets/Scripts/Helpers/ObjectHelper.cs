using Pico.Platform.Models;
using System;
using UnityEngine;

public class ObjectHelper : MonoBehaviour
{
    public static void ChangeColor(GameObject obj,Color color)
    {
        if (obj.TryGetComponent(out MeshRenderer renderer) && renderer.material != null)
        {
            renderer.material.color = color;
        }
        else
        {
            Debug.LogWarning("Couldn't change color on" + obj.name);
        }
    }

    public static void ChangeColorLerp(GameObject obj, Color startColor, Color endColor, float t)
    {
        if (!obj.TryGetComponent<MeshRenderer>(out var renderer)) return;
        if (renderer.material == null) return;

        Color lerpedColor = Color.Lerp(startColor, endColor, Mathf.Clamp01(t));
        renderer.material.color = lerpedColor;
    }

    public static void ChangeLocalScale(GameObject obj, Vector3 scale)
    {
        obj.transform.localScale = scale;
    }

    public static Vector3 GetRotationFromTransform(Transform t)
    {
        return t.rotation.eulerAngles;
    }

    public static Vector3 GetLocalRotationFromTransform(Transform t)
    {
        return t.localRotation.eulerAngles;
    }

    public static Vector3 GetLocalRotationAbsoluteDifferenceFromTransform(Transform t)
    {
        var x = MathF.Abs(360 - t.localRotation.eulerAngles.x);
        var y = MathF.Abs(360 - t.localRotation.eulerAngles.y);
        var z = MathF.Abs(360 - t.localRotation.eulerAngles.z);

        return new Vector3(x, y, z);
    }

    public static bool IsInView(Camera cam, Transform target)
    {
        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);

        bool inFront = viewportPos.z > 0;
        bool inViewport = viewportPos.x >= 0 && viewportPos.x <= 1 &&
                          viewportPos.y >= 0 && viewportPos.y <= 1;

        return inFront && inViewport;
    }


    /// <summary>
    /// Retourne vrai si l’objet a trop bougé depuis sa position de départ
    /// </summary>
    public static bool HasMovedTooFar(Transform target, Vector3 initialPosition, float maxAllowedDistance)
    {
        float distance = Vector3.Distance(target.position, initialPosition);
        return distance > maxAllowedDistance;
    }
}
