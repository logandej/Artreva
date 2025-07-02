using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToAnimate;
    private List<Vector3> objectsStartSize = new();
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private int maxPoints = 3;
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private float curveStrength = 1f;

    private void Start()
    {
        // Cacher tous les objets au départ
        foreach (var obj in objectsToAnimate)
        {
            objectsStartSize.Add(obj.transform.localScale);
            obj.SetActive(false);
            TransitionManager.ChangeSize(obj, Vector3.zero, 0);
        }

        LaunchAnimations();
    }

    public void LaunchAnimations()
    {
        foreach (var obj in objectsToAnimate)
        {
            StartCoroutine(AnimateObject(obj));
        }
    }

    private IEnumerator AnimateObject(GameObject obj)
    {
        obj.SetActive(true);

        Vector3 startPos = obj.transform.position;
        List<Vector3> points = new List<Vector3>();

        // Générer un premier point dans un rayon max
        Vector3 firstPoint = startPos + Random.insideUnitSphere * maxDistance/2;
        firstPoint.y = Mathf.Abs(firstPoint.y); // rester sur le même plan si besoin
        points.Add(firstPoint);

        float distance = Vector3.Distance(startPos, firstPoint);

        // Ajouter d'autres points si le premier est proche
        if (distance < maxDistance / 2f)
        {
            int extraPoints = Random.Range(1, maxPoints); // 1 ou 2
            for (int i = 0; i < extraPoints; i++)
            {
                Vector3 next = points[points.Count - 1] + Random.insideUnitSphere * maxDistance / 2f;
                next.y = Mathf.Abs(next.y);
                points.Add(next);
            }
        }

        Vector3 currentPos = startPos;
        foreach (var point in points)
        {
            Vector3 direction = (point - currentPos).normalized;
            direction = new Vector3(direction.x, -direction.y, -direction.z);
            TransitionManager.ChangePosition(obj, point, transitionDuration, direction, curveStrength);
            TransitionManager.ChangeSize(obj, objectsStartSize[objectsToAnimate.IndexOf(obj)], transitionDuration);
            yield return new WaitForSeconds(transitionDuration);
            currentPos = point;
        }

        yield return new WaitForSeconds(5);
        HideObjects();
    }

    private void HideObjects()
    {
        foreach (var obj in objectsToAnimate)
        {
           
            TransitionManager.ChangeSize(obj, Vector3.zero, transitionDuration);
        }
    }
}