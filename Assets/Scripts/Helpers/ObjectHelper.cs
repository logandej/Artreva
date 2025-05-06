using Pico.Platform.Models;
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
}
