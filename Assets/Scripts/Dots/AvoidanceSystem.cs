using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct AvoidanceSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AvoidanceRadius>();
    }

    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;

        var query = SystemAPI.QueryBuilder().WithAll<LocalTransform, AvoidanceRadius>().Build();

        var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);
        var transforms = query.ToComponentDataArray<LocalTransform>(Unity.Collections.Allocator.Temp);
        var radii = query.ToComponentDataArray<AvoidanceRadius>(Unity.Collections.Allocator.Temp);

        for (int i = 0; i < entities.Length; i++)
        {
            float3 posA = transforms[i].Position;
            float radiusA = radii[i].Value;
            float3 avoid = float3.zero;

            for (int j = 0; j < entities.Length; j++)
            {
                if (i == j) continue;

                float3 posB = transforms[j].Position;
                float radiusB = radii[j].Value;

                float3 dir = posA - posB;
                float dist = math.length(dir);
                float minDist = radiusA + radiusB;

                if (dist < minDist && dist > 0.01f)
                    avoid += math.normalize(dir) * (minDist - dist);
            }

            if (!avoid.Equals(float3.zero))
            {
                var transformRW = SystemAPI.GetComponentRW<LocalTransform>(entities[i]);
                transformRW.ValueRW.Position += avoid * dt;
            }
        }

        entities.Dispose();
        transforms.Dispose();
        radii.Dispose();
    }
}