using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public XROrigin Player;
    public Transform cameraSubtitleTransform;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }
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
