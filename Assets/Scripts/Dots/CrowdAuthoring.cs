using Unity.Entities;
using UnityEngine;

public class CrowdAuthoring : MonoBehaviour
{
    public float moveSpeed = 1.5f;
    public float avoidanceRadius = 1.5f;
}

public class CrowdAuthoringBaker : Baker<CrowdAuthoring>
{
    public override void Bake(CrowdAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new MoveSpeed { Value = authoring.moveSpeed });
        AddComponent(entity, new AvoidanceRadius { Value = authoring.avoidanceRadius });
    }
}