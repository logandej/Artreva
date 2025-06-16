using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DissolveReplaceMaterial : MonoBehaviour
{
    public Material dissolveMaterialBase;
    public float duration = 1f;
    public bool autoStart = true;

    private Renderer rend;
    private Material originalMaterial;
    private Material dissolveInstance;
    private float progress = 0f;
    private bool running = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (dissolveMaterialBase == null || rend == null)
        {
            Debug.LogWarning("Missing components.");
            enabled = false;
            return;
        }

        originalMaterial = rend.sharedMaterial;

        if (autoStart)
            StartDissolve();
    }

    public void StartDissolve()
    {
        // Instance du shader de dissolve
        dissolveInstance = new Material(dissolveMaterialBase);
        rend.material = dissolveInstance;

        progress = 0f;
        running = true;
    }

    void Update()
    {
        if (!running || dissolveInstance == null) return;

        progress += Time.deltaTime / duration;
        float t = Mathf.Clamp01(progress);

        dissolveInstance.SetFloat("_FillAmount", t);

        if (t >= 1f)
        {
            // On remet le matériau d'origine
            rend.material = originalMaterial;
            Destroy(dissolveInstance);
            running = false;
        }
    }

    private void OnEnable()
    {
        StartDissolve();
    }
}