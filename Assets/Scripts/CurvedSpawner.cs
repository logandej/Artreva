using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToAnimate;
    private List<Vector3> objectsStartSize = new();
    [SerializeField] private float radius = 2f;
    [SerializeField] private float transitionDuration = 1.5f;
    [SerializeField] private float curveStrength = 1f;

    private int finishedCount = 0;
    private void Start()
    {
        // Cacher tous les objets au départ
        foreach (var obj in objectsToAnimate)
        {
            objectsStartSize.Add(obj.transform.localScale);
            obj.SetActive(false);
            TransitionManager.ChangeSize(obj, Vector3.zero, 0);
        }
    }


    public void LaunchAnimations()
    {
        finishedCount = 0; // reset
        int index = 0;
        foreach (var obj in objectsToAnimate)
        {
            StartCoroutine(AnimateObject(obj,index));
            index++;
        }
    }

    private IEnumerator AnimateObject(GameObject obj, int index)
    {
        obj.SetActive(true);

        // Point de départ de l'objet
        Vector3 startPos = obj.transform.position;
        float radius = this.radius;   // rayon du cercle

        // Calcul de l’angle pour cet objet
        float stepAngle = 180f / (objectsToAnimate.Count + 1);
        float angle = stepAngle * (index + 1);
        float rad = angle * Mathf.Deg2Rad;

        // Position cible sur un arc vertical (XY)
        Vector3 targetPos = new Vector3(
            startPos.x + radius * Mathf.Cos(rad), // décalage latéral
            startPos.y + radius * Mathf.Sin(rad), // monte en hauteur
            startPos.z                             // pas de profondeur
        );

        // Animation
        TransitionManager.ChangePosition(obj, targetPos, transitionDuration, Vector3.up, curveStrength);
        TransitionManager.ChangeSize(obj, objectsStartSize[objectsToAnimate.IndexOf(obj)], transitionDuration);

        yield return new WaitForSeconds(transitionDuration);
        yield return new WaitForSeconds(5f);

        finishedCount++;
        if (finishedCount == objectsToAnimate.Count)
        {
            HideObjects();
        }
    }

    private void HideObjects()
    {
        foreach (var obj in objectsToAnimate)
        {
           
            TransitionManager.ChangeSize(obj, Vector3.zero, transitionDuration);
            TransitionManager.ChangeLocalPosition(obj, Vector3.zero, transitionDuration);
        }
    }
}