using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class FarArtInteractableAnalyzable : FarArtInteractable
{
    private bool isAnalyzing = false;
    private bool isAnalyzed = false;
    private float rotationAmount = 0f;
    [SerializeField] private float rotationSpeed = 10f; // degrés/seconde

    [SerializeField] private bool canDeanalyze = true;
    [SerializeField] private float deanalyzeDelay = 3f;

    private Transform handAnalyzing;

    [SerializeField] ArtAnalyzer analyzer;
    public void Analyze(Transform hand)
    {
        if (isActive && !isAnalyzed)
        {
            handAnalyzing = hand;
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
        isAnalyzed = true;
        handAnalyzing = null;
        ObjectHelper.ChangeColor(this.gameObject, Color.green);
        if (canDeanalyze)
        {
            Invoke(nameof(Deanalyze),deanalyzeDelay);
        }

    }

    private void Deanalyze()
    {
        if(isAnalyzed)
        {
            isAnalyzed = false;
            DeactivateNow();
            rotationAmount = 0f;
        }
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

        Debug.Log("1 : "+CalculRotationValue(1));
        Debug.Log("179 : "+CalculRotationValue(179));
        Debug.Log("181 : "+CalculRotationValue(181));
        Debug.Log("270 : "+CalculRotationValue(270));
        Debug.Log("359 : "+CalculRotationValue(359));

        if (isAnalyzing)
        {
            float deltaRotation = rotationSpeed * Time.deltaTime;
            //transform.Rotate(Vector3.up, deltaRotation);
            //rotationAmount += deltaRotation;
            RotateWithHand(ObjectHelper.GetLocalRotationAbsoluteDifferenceFromTransform(handAnalyzing).z);

            if (rotationAmount >= 360f)
            {
                StopAnalyzing();
            }
        }
    }

    public void RotateWithHand(float ZhandRotate)
    {
        transform.Rotate(CalculRotationValue(ZhandRotate) * rotationSpeed * Time.deltaTime * Vector3.up);
        analyzer.AnalyzeRotation(ZhandRotate);
    }

    private float CalculRotationValue(float ZhandRotation)
    {
        if(ZhandRotation <= 180)
        {
            return ZhandRotation / 180;
        }
        else
        {
            return (360 - ZhandRotation) / 180;
        }
    }
}
