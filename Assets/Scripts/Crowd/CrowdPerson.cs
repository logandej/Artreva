using UnityEngine;

public class CrowdPerson : MonoBehaviour
{
    public float idleDurationMin = 3f, idleDurationMax = 8f;
    public float moveDurationMin = 2f, moveDurationMax = 4f;
    public float moveSpeed = 1f;

    private enum State { Idle, Moving }
    private State currentState;
    private float stateTimer;
    private Vector3 moveTarget, moveStart;
    private float moveProgress;
    private Bounds moveZone;

    public void InitZone(Bounds zone)
    {
        moveZone = zone;
        EnterIdle();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                if ((stateTimer -= Time.deltaTime) <= 0f) EnterMove();
                break;
            case State.Moving:
                moveProgress += Time.deltaTime / stateTimer;
                transform.position = Vector3.Lerp(moveStart, moveTarget, Mathf.SmoothStep(0f, 1f, moveProgress));
                if (moveProgress >= 1f) EnterIdle();
                break;
        }
    }

    void EnterIdle()
    {
        currentState = State.Idle;
        stateTimer = Random.Range(idleDurationMin, idleDurationMax);
    }

    void EnterMove()
    {
        currentState = State.Moving;
        stateTimer = Random.Range(moveDurationMin, moveDurationMax);
        moveStart = transform.position;
        moveTarget = new Vector3(
            Random.Range(moveZone.min.x, moveZone.max.x),
            moveZone.min.y,
            Random.Range(moveZone.min.z, moveZone.max.z)
        );
        moveProgress = 0f;
        Vector3 dir = moveTarget - moveStart; dir.y = 0f;
        if (dir.sqrMagnitude > 0.01f) transform.rotation = Quaternion.LookRotation(dir);
    }
}
