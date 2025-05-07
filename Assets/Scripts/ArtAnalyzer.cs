using System.Collections.Generic;
using UnityEngine;

public class ArtAnalyzer : MonoBehaviour
{
    [Header("Analyse Settings")]
    [SerializeField] private int angleCount = 8;
    [SerializeField] private float radius = 2f;
    [SerializeField] private float angleTolerance = 10f;
    [SerializeField] private GameObject markerPrefab; // Le prefab à instancier

    private List<GameObject> angleMarkers = new();
    private List<bool> observedAngles = new();
    private List<float> angleValues = new();

    void Start()
    {
        GenerateMarkers();
    }

    public void AnalyzeRotation(float currentY)
    {
        for (int i = 0; i < angleCount; i++)
        {
            if (observedAngles[i]) continue;

            float targetAngle = angleValues[i];
            float diff = Mathf.Abs(Mathf.DeltaAngle(currentY, targetAngle));

            if (diff <= angleTolerance)
            {
                observedAngles[i] = true;
                ObjectHelper.ChangeColor(angleMarkers[i], Color.green);
                Debug.Log($"Angle {targetAngle}° observé !");
            }
        }
    }

    private void GenerateMarkers()
    {
        float step = 360f / angleCount;

        for (int i = 0; i < angleCount; i++)
        {
            float angle = i * step;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            Vector3 pos = transform.position + dir * radius;

            GameObject marker = Instantiate(markerPrefab, pos, Quaternion.identity, transform);
            marker.name = "AngleMarker_" + angle;
            angleMarkers.Add(marker);
            observedAngles.Add(false);
            angleValues.Add(angle);

            ObjectHelper.ChangeColor(marker, Color.red);
        }
    }
}