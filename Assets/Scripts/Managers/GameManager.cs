using System.Collections;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public XROrigin Player {get; private set;}
    public Transform CameraSubtitleTransform { get; private set; }
    public SensePackMR SensePackMR { get; private set; }

    public LightSwitcher LightSwitcher { get; private set; }

    private StringTable _cachedTable;
    private StringTable _cachedTable2;

    public GameStates GameStatus;
    public enum GameStates
    {
        Warning = 0,
        Menu = 1,
        MiraScene = 2,
        FestivalScene = 3,
        Adjuvant = 4,
        KoonsaScene = 5,
        LastTalk = 6,
        Ending= 7,
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else { Destroy(this.gameObject); }
    }




    private void Start()
    {
        StartCoroutine(StartTable());
    }

    IEnumerator StartTable()
    {
        var tableOp = LocalizationSettings.StringDatabase.GetTableAsync("Subtitles");
        yield return tableOp;
        _cachedTable = tableOp.Result;

        var tableOp2 = LocalizationSettings.StringDatabase.GetTableAsync("OtherTexts");
        yield return tableOp2;
        _cachedTable2 = tableOp2.Result;

    }

    public string PrintLocalizedString(string key)
    {
        if (_cachedTable == null)
        {
            return "Localization table not yet loaded.";
        }

        var entry = _cachedTable.GetEntry(key);
        if (entry == null)
            return $"Key not found: {key}";

        return entry.LocalizedValue;
    }

    public string PrintLocalizedString2(string key)
    {
        if (_cachedTable2 == null)
        {
            return "Localization table not yet loaded.";
        }

        var entry = _cachedTable2.GetEntry(key);
        if (entry == null)
            return $"Key not found: {key}";

        return entry.LocalizedValue;
    }

    public void LoadScene(string sceneName)
    {
        SceneLoaderManager.Instance.LoadScene(sceneName);
    }

    public void SelectFrench()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        //StartCoroutine(StartTable());
    }

    public void SelectEnglish()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        //StartCoroutine(StartTable()); // recharge bien la table pour la nouvelle langue

    }



    public void SetGameStatus(int status)
    {
        print("SET GAME STATUS" + (GameStates)status);
        //Instance très important vu que je ne prends pas la valeur du préfab mais bien de l'instance unique dans le jeu.
        Instance.GameStatus = (GameStates)status;
    }

    public void ResetGame()
    {
        Instance.GameStatus = GameStates.Warning;
        //TODO
    }

    public void RegisterSceneReferences(SceneInfo sceneInfo)
    {
        Player = sceneInfo.Player;
        CameraSubtitleTransform = sceneInfo.cameraSubtitleTransform;
        SensePackMR = sceneInfo.sensePackMR;
        LightSwitcher = sceneInfo.lightSwitcher;
    }

    public void SwitchToDay()
    {
        Instance.LightSwitcher.SwitchToDay();
    }

    public void SwitchToNight()
    {
        Instance.LightSwitcher.SwitchToNight();
    }

    public void PauseGame()
    {
        ScenarioManager.Instance.PauseTimeline();
    }
     public void ResumeGame()
    {
        ScenarioManager.Instance.ResumeTimeline();
    }

}
