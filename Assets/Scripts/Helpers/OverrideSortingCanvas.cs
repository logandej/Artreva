using UnityEngine;

public class OverrideSortingCanvas : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1000;
        canvas.sortingLayerName = "UI"; // ou autre nom de layer
    }


}
