using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FarArtInteractableAnalyzable : FarArtInteractable
{
    private bool isAnalyzing = false;
    private bool isAnalyzed = false;
    private float rotationAmount = 0f;
    [SerializeField] private float rotationSpeed = 90f; // degrés/seconde
    
    public void Analyze()
    {
        if (isActive && !isAnalyzed)
        {
            isAnalyzing = true;
            StartAnalyzing();
        }
    }

    private void StartAnalyzing()
    {
        ObjectHelper.ChangeColor(this.gameObject, Color.cyan);
    }

    private void StopAnalyzing()
    {
        isAnalyzing = false;
        ObjectHelper.ChangeColor(this.gameObject, Color.green);

    }


    public override void Deactive()
    {
        // IF the player is analyzing an art, then he can't deactive it until he finishes the analyze.
        if(!isAnalyzing) { base.Deactive(); }
    }

    public override void DeactivateNow()
    {
        if(!isAnalyzing) { base.DeactivateNow(); }
    }

    protected override void Update()
    {
        base.Update();

        if (isAnalyzing)
        {
            float deltaRotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, deltaRotation);
            rotationAmount += deltaRotation;

            if (rotationAmount >= 360f)
            {
                StopAnalyzing();
            }
        }
    }
}
