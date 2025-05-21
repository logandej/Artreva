using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    public int count = 100;

    void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        var allEntities = entityManager.GetAllEntities(Unity.Collections.Allocator.Temp);
        Debug.Log("Nombre total d'entités dans la scène : " + allEntities.Length);

        bool found = false;
        foreach (var e in allEntities)
        {
            if (entityManager.HasComponent<CrowdPrefabReference>(e))
            {
                Debug.Log("Entité trouvée avec CrowdPrefabReference");
                var prefabRef = entityManager.GetComponentData<CrowdPrefabReference>(e).Prefab;

                for (int i = 0; i < count; i++)
                {
                    var entity = entityManager.Instantiate(prefabRef);
                    var pos = new float3(UnityEngine.Random.Range(-10f, 10f), 0, UnityEngine.Random.Range(-10f, 10f));
                    entityManager.SetComponentData(entity, LocalTransform.FromPosition(pos));
                }

                found = true;
                break;
            }
        }

        if (!found)
        {
            Debug.LogError("Aucun singleton avec CrowdPrefabReference trouvé.");
        }

        allEntities.Dispose();
    }
}