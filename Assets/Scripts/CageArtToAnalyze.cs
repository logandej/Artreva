using UnityEngine;

public class CageArtToAnalyze : MonoBehaviour
{
    FarArtInteractableAnalyzable artAnalyzable;
    DissolveReplaceMaterial dissolve;
    [SerializeField] Transform goTo;
    [SerializeField] float transitionDuration = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dissolve = GetComponent<DissolveReplaceMaterial>();
        artAnalyzable = GetComponent<FarArtInteractableAnalyzable>();
        artAnalyzable.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        DissolveBy();
    }

    public void OnAction()
    {
        TransitionManager.ChangePosition(dissolve.gameObject, goTo.position, transitionDuration);
        Invoke(nameof(EnableArt), transitionDuration);
    }

    void EnableArt()
    {
        artAnalyzable.enabled = true;
    }

    void DissolveBy()
    {
        dissolve.Progress = 1 - artAnalyzable.ActivePercent;
    }
}
