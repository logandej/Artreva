using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    void Update()
    {
        // Oriente l'UI vers la cam�ra en inversant les axes si n�cessaire
        transform.forward = Camera.main.transform.forward;
    }
}
