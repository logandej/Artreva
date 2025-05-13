using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ArtAnalyzer : MonoBehaviour
{
    [Header("Analyse Settings")]
    [SerializeField] private int angleCount = 8;
    [SerializeField] private float radius = 2f;
    [SerializeField] private GameObject markerPrefab; // Le prefab à instancier

    private List<GameObject> angleMarkers = new();
    private List<bool> observedAngles = new();

    public UnityEvent eventDone = new();



    public void AnalyzeFromProximity()
    {
        float closestDist = float.MaxValue;
        int closestIndex = -1;
        var playerTransform = GameManager.Instance.Player.transform;


        for (int i = 0; i < angleMarkers.Count; i++)
        {
            //if (observedAngles[i]) continue;

            float dist = Vector3.Distance(playerTransform.position, angleMarkers[i].transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex >= 0)
        {
            if (!observedAngles[closestIndex]) // Check if already observed
            {
                observedAngles[closestIndex] = true;
                ObjectHelper.ChangeColor(angleMarkers[closestIndex], Color.green);
                Debug.Log($"Marker #{closestIndex} observé par proximité !");
                CheckAllAnalyzed();
            }
        }
    }

    private void CheckAllAnalyzed()
    {
        if (observedAngles.All(c => c == true)) // If all booleans of the list are equals to 'true' could write (c=>c) only but it's for readibility
        {
            eventDone?.Invoke();
            Invoke(nameof(DestroyMarkers),3);
        }
    }


    public void GenerateMarkers()
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

            ObjectHelper.ChangeColor(marker, Color.red);
        }
    }

    private void DestroyMarkers()
    {
        angleMarkers.ForEach(marker => Destroy(marker));
        angleMarkers.Clear();
        observedAngles.Clear();
    }
}