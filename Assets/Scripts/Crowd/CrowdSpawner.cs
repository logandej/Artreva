using System.Collections.Generic;
using UnityEngine;

public class CrowdSpawner : MonoBehaviour
{
    [Header("Mesh & Material")]
    public List<Mesh> meshes;
    public Material crowdMaterial;

    [Header("Crowd Settings")]
    public Vector3 baseScale = Vector3.one;
    public Vector3 baseRotation = Vector3.zero;
    public float YRotation = 0;
    public bool RandomYRotation = true;
    public bool YRandom = false;
    public float YOffset = 1;
    public int count = 500;

    [Header("Spawn Zones")]
    public List<BoxCollider> spawnZones;

    Dictionary<Mesh, List<Matrix4x4>> meshMatrixMap = new();
    Dictionary<Mesh, List<float>> meshRandomSeedMap = new();
    Dictionary<Mesh, List<int>> meshColorIndexMap = new();

    private MaterialPropertyBlock props;

    [Header("Activation")]
    public bool enableCrowd = true;

    void Start()
    {
        props = new MaterialPropertyBlock();

        for (int i = 0; i < count; i++)
        {
            BoxCollider zone = spawnZones[Random.Range(0, spawnZones.Count)];

            Vector3 localPos = new Vector3(
                Random.Range(-zone.size.x / 2f, zone.size.x / 2f),
                YRandom ? Random.Range(-zone.size.y / 2f, zone.size.y / 2f) : YOffset,
                Random.Range(-zone.size.z / 2f, zone.size.z / 2f)
            );

            Vector3 worldPos = zone.transform.TransformPoint(localPos);
            // Orientation combinée : baseRotation + rotation aléatoire Y
            Quaternion baseRot = Quaternion.Euler(baseRotation);
            Quaternion randomRotY = Quaternion.Euler(0f, RandomYRotation ? Random.Range(0f, 360f) : YRotation, 0f);
            Quaternion finalRot = randomRotY * baseRot;

            // Échelle combinée
            //float scaleFactor = Random.Range(0.9f, 1.1f);
            //Vector3 finalScale = baseScale * scaleFactor;

            Mesh chosenMesh = meshes[Random.Range(0, meshes.Count)];
            if (!meshMatrixMap.ContainsKey(chosenMesh))
            {
                meshMatrixMap[chosenMesh] = new List<Matrix4x4>();
                meshRandomSeedMap[chosenMesh] = new List<float>();
                meshColorIndexMap[chosenMesh] = new List<int>();
            }
            meshMatrixMap[chosenMesh].Add(Matrix4x4.TRS(worldPos, finalRot, baseScale));
            meshRandomSeedMap[chosenMesh].Add(Random.Range(0f, 100f));
            meshColorIndexMap[chosenMesh].Add(Random.Range(0, 5));
        }
    }

    void Update()
    {
        if (!enableCrowd) return; // On saute tout si désactivé

        int batchSize = 1023;

        foreach (var mesh in meshes)
        {
            if (!meshMatrixMap.ContainsKey(mesh)) continue;

            List<Matrix4x4> matrices = meshMatrixMap[mesh];
            List<float> seeds = meshRandomSeedMap[mesh];
            List<int> indices = meshColorIndexMap[mesh];

            for (int i = 0; i < matrices.Count; i += 1023)
            {
                int count = Mathf.Min(1023, matrices.Count - i);
                Matrix4x4[] matrixArray = new Matrix4x4[count];
                matrices.CopyTo(i, matrixArray, 0, count);

                props.Clear();
                props.SetFloatArray("_RandomSeed", seeds.GetRange(i, count));
                props.SetFloatArray("_ColorIndex", indices.GetRange(i, count).ConvertAll(c => (float)c));

                Graphics.DrawMeshInstanced(mesh, 0, crowdMaterial, matrixArray, count, props);
            }
        }
    }

    private void OnEnable()
    {
        enableCrowd = true;
    }

    private void OnDisable()
    {
        enableCrowd = false;
    }
}

