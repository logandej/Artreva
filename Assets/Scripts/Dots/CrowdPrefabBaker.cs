using Unity.Entities;
using UnityEngine;

public struct CrowdPrefabReference : IComponentData
{
    public Entity Prefab;
}

public class CrowdPrefabBaker : Baker<CrowdPrefabAuthoring>
{
    public override void Bake(CrowdPrefabAuthoring authoring)
    {
        //On déclare que ce prefab doit être converti en tant que prefab (pas comme objet de scène)
        if (authoring.prefab == null)
        {
            Debug.LogError("Le prefab n'est pas assigné");
            return;
        }

        var prefabEntity = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic | TransformUsageFlags.WorldSpace | TransformUsageFlags.NonUniformScale);

        AddComponent(GetEntity(TransformUsageFlags.None), new CrowdPrefabReference
        {
            Prefab = prefabEntity
        });

        Debug.Log("CrowdPrefabBaker exécuté et prefab Entity assigné");
    }
}