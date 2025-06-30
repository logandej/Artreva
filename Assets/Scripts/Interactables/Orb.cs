using UnityEngine;

public class Orb: MonoBehaviour
{
    [SerializeField] bool isPartOfEnigma = true;

    private Vector3 initialPosition;
    private static float maxFar = 1f;
    private bool isReturningToInitialPosition = false;
    private float initialSize;

    private Transform originalParent;
    private void Awake()
    {
        
        initialPosition = transform.position;
        initialSize = transform.localScale.x;
        if (isPartOfEnigma)
        {
            transform.localScale = Vector3.zero;
            Hide();
        }

        originalParent = transform.parent;
    }

    private void Update()
    {
        if(ObjectHelper.HasMovedTooFar(transform, initialPosition, maxFar) && !isReturningToInitialPosition && isPartOfEnigma)
        {
            isReturningToInitialPosition=true;
            TransitionManager.ChangeLocalPosition(gameObject, initialPosition, 1f, Vector3.up, .2f);
            Invoke(nameof(DoneTransition),1f);
        }

        //if (!isPartOfEnigma && transform.parent != originalParent)
        //{
        //    transform.SetParent(originalParent);
        //}
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
