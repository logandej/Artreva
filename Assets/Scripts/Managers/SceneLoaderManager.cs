using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance { get; private set; }
    public float LoadingProgress { get; private set; } = 0f;
    private AsyncOperation loadingOperation;
    private bool isLoading = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    /// <summary>
    /// Lance le chargement asynchrone d'une scène.
    /// </summary>
    /// <param name="sceneName">Nom de la scène à charger</param>
    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isLoading = true;
        SceneFader.Instance.LoadWhiteFade();
        yield return new WaitForSeconds(SceneFader.Instance.FadeTime);
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        // Attente du chargement
        while (!loadingOperation.isDone)
        {
            // Valeur plafonnée à 0.9 tant que allowSceneActivation = false
            LoadingProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            // Déclenchement de la scène quand elle est complètement chargée
            if (loadingOperation.progress >= 0.9f)
            {
                // Petite attente optionnelle (fade, etc.)
                yield return new WaitForSeconds(1f);
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        LoadingProgress = 1f;
        SceneFader.Instance.UnloadFade();
        isLoading = false;
    }

    /// <summary>
    /// Retourne si une scène est en cours de chargement.
    /// </summary>
    public bool IsLoading()
    {
        return isLoading;
    }
}