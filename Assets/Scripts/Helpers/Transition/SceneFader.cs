using System.Collections;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    // Référence au Material (peut être assignée dans l’inspecteur)
    public GameObject sphereFader;

    public float FadeTime { get; private set; } = 3f;

    public static SceneFader Instance;
    private void Awake()
    {
        if(Instance== null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        UnloadFade();
    }


    // Exemple de méthode pour changer la couleur via script

    public void LoadWhiteFade(float alpha = 1)
    {
        LoadFade(new Color(1, 1, 1, alpha));
    }
    public void LoadBlackFade(float alpha = 1)
    {
        LoadFade(new Color(0, 0, 0, alpha));
    }
    private void LoadFade(Color color)
    {
        TransitionManager.ChangeBaseColor(sphereFader, color, FadeTime);
        Invoke(nameof(ChangementsWhileFadeFinished),FadeTime);
    }

    public void UnloadFadeIn(float time)
    {
        Invoke(nameof(UnloadFade),time);
    }

    public void UnloadFade()
    {
        TransitionManager.ChangeBaseColor(sphereFader, new Color(0, 0, 0, 0), FadeTime);
    }

    private void ChangementsWhileFadeFinished()
    {
        GameManager.Instance.SensePackMR?.ActiveMR(false);
    }

}
