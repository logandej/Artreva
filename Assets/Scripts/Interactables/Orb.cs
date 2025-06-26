using UnityEngine;

public class Orb: MonoBehaviour
{
    [SerializeField] bool isPArtOfEnigma = true;

    private Vector3 initialPosition;
    private static float maxFar = 1f;
    private bool isReturningToInitialPosition = false;
    private float initialSize;
    private void Awake()
    {
        
        initialPosition = transform.position;
        initialSize = transform.localScale.x;
        if (isPArtOfEnigma)
        {
            transform.localScale = Vector3.zero;
            Hide();
        }
    }
    private void Update()
    {
        if(ObjectHelper.HasMovedTooFar(transform, initialPosition, maxFar) && !isReturningToInitialPosition)
        {
            isReturningToInitialPosition=true;
            TransitionManager.ChangePosition(gameObject, initialPosition, 1f, Vector3.up, .2f);
            Invoke(nameof(DoneTransition),1f);
        }
    }

    public void StartOrbForEnigma()
    {
        gameObject.SetActive(true);
        print("APPAR");
        TransitionManager.ChangeSize(gameObject, Vector3.one * initialSize, 1f);
    }

    public void StopEnigma()
    {
        TransitionManager.ChangeSize(gameObject, Vector3.zero, 1f);
        Destroy(gameObject, 1f);
        Invoke(nameof(Hide), 1f);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void DoneTransition()
    {
        isReturningToInitialPosition=false;
    }
}
