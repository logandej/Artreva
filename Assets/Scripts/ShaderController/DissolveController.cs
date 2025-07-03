using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveReplaceMaterial : MonoBehaviour
{
    public Material dissolveMaterialBase;
    public float duration = 1f;
    public bool solveOnEnable = true;

    private Renderer rend;
    private Material originalMaterial;
    private Material dissolveInstance;
    private Coroutine dissolveRoutine;

    public float Progress
    {
        get => dissolveInstance != null ? dissolveInstance.GetFloat("_FillAmount") : 0f;
        set
        {
            if (dissolveInstance != null)
            {
                dissolveInstance.SetFloat("_FillAmount", value);
                rend.material = dissolveInstance;
            }
        }
    }

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        if (dissolveMaterialBase == null || rend == null)
        {
            Debug.LogWarning("Missing components.");
            enabled = false;
            return;
        }

        originalMaterial = rend.sharedMaterial;
        dissolveInstance = new Material(dissolveMaterialBase);
    }

    private void OnEnable()
    {
        if (solveOnEnable)
            Solve(duration); // durée par défaut
    }

    public void Dissolve(float duration)
    {
        if (dissolveRoutine != null) StopCoroutine(dissolveRoutine);
        dissolveRoutine = StartCoroutine(DissolveRoutine(1f, 0f, duration));
    }

    public void Solve(float duration)
    {
        if (dissolveRoutine != null) StopCoroutine(dissolveRoutine);
        dissolveRoutine = StartCoroutine(DissolveRoutine(0f, 1f, duration));
    }

    private IEnumerator DissolveRoutine(float startValue, float endValue, float duration)
    {
        rend.material = dissolveInstance;

        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            Progress = Mathf.Lerp(startValue, endValue, t);
            timer += Time.deltaTime;
            yield return null;
        }

        Progress = endValue;

        if (endValue >= 1f)
        {
            // Rétablir le matériau d'origine après un solve complet
            rend.material = originalMaterial;
        }

        dissolveRoutine = null;
    }

}