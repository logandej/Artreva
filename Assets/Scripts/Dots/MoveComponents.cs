using Unity.Entities;

public struct MoveSpeed : IComponentData
{
    public float Value;
}

public struct AvoidanceRadius : IComponentData
{
    public float Value;
}