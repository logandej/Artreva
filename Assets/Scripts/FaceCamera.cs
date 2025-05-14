using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        // Oriente l'UI vers la caméra en inversant les axes si nécessaire
        transform.forward = Camera.main.transform.forward;
    }
}
