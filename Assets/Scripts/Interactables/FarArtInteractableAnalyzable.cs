using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FarArtInteractableAnalyzable : FarArtInteractable
{
    private bool isAnalyzing = false;
    private bool isAnalyzed = false;
    [SerializeField] private float rotationSpeed = 10f; // degrés/seconde

    [SerializeField] private bool canDeanalyze = true;
    [SerializeField] private float deanalyzeDelay = 3f;

    private Transform handAnalyzing;

    [SerializeField] ArtAnalyzer analyzer;

    public UnityEvent eventAnalyzed = new();

    private void Start()
    {
        GetComponentInChildren<CurvedSpawner>();
        eventAnalyzed.AddListener(GetComponentInChildren<CurvedSpawner>().LaunchAnimations);
    }
    public void Analyze(Transform hand)
    {
        if (isActive && !isAnalyzed && !isAnalyzing)
        {
            handAnalyzing = hand;
            isAnalyzing = true;
            StartAnalyzing();
        }
    }

    private void StartAnalyzing()
    {
        //ObjectHelper.ChangeColor(this.gameObject, Color.cyan);
        analyzer.eventDone.AddListener(StopAnalyzing);
        analyzer.GenerateMarkers();

    }

    private void StopAnalyzing()
    {
        isAnalyzing = false;
        isAnalyzed = true;
        handAnalyzing = null;
        //ObjectHelper.ChangeColor(this.gameObject, Color.green);
        if (canDeanalyze)
        {
            Invoke(nameof(Deanalyze),deanalyzeDelay);
        }
        analyzer.eventDone.RemoveListener(StopAnalyzing);
        eventAnalyzed?.Invoke();


    }

    private void Deanalyze()
    {
        if(isAnalyzed)
        {
            isAnalyzed = false;
            DeactivateNow();
        }
    }


    public override void OnHoverExit()
    {
        // IF the player is analyzing an art, then he can't deactive it until he finishes the analyze.
        if(!isAnalyzing) { base.OnHoverExit(); }
    }

    protected override void ActivateNow()
    {
        base.ActivateNow();
        slider.gameObject.SetActive(true);
        HandActionManager.Instance.LockAnalyzable(this);
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
            RotateWithHand(ObjectHelper.GetLocalRotationAbsoluteDifferenceFromTransform(handAnalyzing).z);
        }
    }

    public void RotateWithHand(float ZhandRotate)
    {
        transform.Rotate(CalculRotationValue(ZhandRotate) * rotationSpeed * Time.deltaTime * Vector3.up);
        //analyzer.AnalyzeRotation(ZhandRotate);
        analyzer.AnalyzeFromProximity();
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
