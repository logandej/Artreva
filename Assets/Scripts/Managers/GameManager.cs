using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public XROrigin Player;
    public Transform cameraSubtitleTransform;
    public SensePackMR sensePackMR;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }

    public void LoadScene(string sceneName)
    {
        SceneLoaderManager.Instance.LoadScene(sceneName);
    }

    public void SelectFrench()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
    }

    public void SelectEnglish()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
    }

}
