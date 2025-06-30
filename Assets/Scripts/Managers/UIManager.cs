using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;


    [Header("Info")]
    public Canvas canvasInfo;
    public Image infoImage;
    public Image bgInfoImage;
    public TMP_Text infoText;

    [Header("Settings")]
    public Canvas canvasSettings;

    public int TimeShowInfo { get; set; } = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

        }
        else {  Destroy(this.gameObject);}
    }

    private void Start()
    {
        infoImage.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
        canvasSettings.gameObject.SetActive(false);
    }

    public void SetText(string key)
    {
        var lse = infoText.GetComponent<LocalizeStringEvent>();
        lse.StringReference.TableEntryReference = key;
        lse.RefreshString(); // Ceci déclenche la mise à jour du TMP Text
    }

    public void ShowInfo(Sprite sprite)
    {
        infoImage.sprite = sprite;
        infoImage.color = new Color(1, 1, 1, 0);
        infoImage.gameObject.SetActive(true);
        infoText.gameObject.SetActive(true);

        TransitionManager.InterpolateFloat(0f, 1f, 0.5f, alpha =>
        {
            Color c = infoImage.color;
            c.a = alpha;
            infoImage.color = c;
            bgInfoImage.color = c;
            infoText.color = c;
        });

        Invoke(nameof(HideInfo), 5f);
    }


    private void HideInfo()
    {
        TransitionManager.InterpolateFloat(1f, 0f, 0.5f, alpha =>
        {
            Color c = infoImage.color;
            c.a = alpha;
            infoImage.color = c;
            bgInfoImage.color = c;
            infoText.color = c;
        });

        StartCoroutine(DisableAfter(0.5f));
    }

    private IEnumerator DisableAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        infoImage.gameObject.SetActive(false);
        infoText.gameObject.SetActive(false);
    }

    private bool showWhileWaitingForPlayer = false;

    public void ShowSettings()
    {
        canvasInfo.gameObject.SetActive(false);
        canvasSettings.gameObject.SetActive(true);

        showWhileWaitingForPlayer = ScenarioManager.Instance.WaitingForPlayer;

        if (!showWhileWaitingForPlayer)
            GameManager.Instance.PauseGame();
        

    }

    public void HideSettings()
    {
        canvasInfo.gameObject.SetActive(true);
        canvasSettings.gameObject.SetActive(false);

        if(!showWhileWaitingForPlayer)
            GameManager.Instance.ResumeGame();

    }

}
