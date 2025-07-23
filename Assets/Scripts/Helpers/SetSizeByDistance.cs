using UnityEngine;

public class SetSizeByDistance : MonoBehaviour
{
    [SerializeField] float sizeScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetSizeDependingCameraDistance();
    }

    private void SetSizeDependingCameraDistance()
    {
        float distance = (this.transform.position - Camera.main.transform.position).magnitude;
        transform.localScale = distance * sizeScale * Vector3.one;
    }
}
