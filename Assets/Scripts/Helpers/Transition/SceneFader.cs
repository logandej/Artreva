using System.Collections;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    // Référence au Material (peut être assignée dans l’inspecteur)
    public GameObject sphereFader;


    void Start()
    {
        
        StartCoroutine(Sequence());
    }

    // Exemple de méthode pour changer la couleur via script
    public void LoadFade(Color color)
    {
        TransitionManager.ChangeBaseColor(sphereFader, color, 3f);
    }

    public void UnloadFade()
    {
        TransitionManager.ChangeBaseColor(sphereFader, new Color(0, 0, 0, 0), 3);
    }

    private IEnumerator Sequence()
    {
        for (int i = 0; i < 10; i++)
        {
            LoadFade(new Color(1, 1, 1, 1));
            yield return new WaitForSeconds(5);
            UnloadFade();
            yield return new WaitForSeconds(5);
            LoadFade(new Color(0, 0, 0, 1));
            yield return new WaitForSeconds(5);
            UnloadFade();
            yield return new WaitForSeconds(5);
        }

    }
}
