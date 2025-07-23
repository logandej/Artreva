using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Localization.Components;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] string speakerName;
    [SerializeField] Sprite speakerSprite;
    [SerializeField] Image speakerImage;
    [SerializeField] Transform speakerTransform;
    [SerializeField] Canvas canvas;
    [Header("Settings")]
    [SerializeField] TMP_Text speakerNameText;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] float typingSpeed = 0.1f;
    private bool isAboveSpeaker=false;

    public static UnityEvent eventLock = new();
    public static UnityEvent eventUnlock = new();

    private bool changeWithView = true;
    public static float subtitle_size = .2f;

    private void Start()
    {
        eventLock.AddListener(Lock);
        eventUnlock.AddListener(Unlock);
        HideSubtitle();
        GoToSpeakerTransform();
    }

    public void SetText(string key)
    {
        speakerNameText.text = speakerName;
        speakerImage.sprite = speakerSprite;
        canvas.gameObject.SetActive(true);

        var lse = GetComponent<LocalizeStringEvent>();
        lse.StringReference.TableEntryReference = key;
        lse.RefreshString(); // Ceci déclenche la mise à jour du TMP Text

    }

    public void SetTextWithDuration(string key)
    {
        SetText(key);
        Invoke("HideSubtitle",5);

    }

    public void HideSubtitle()
    {
        canvas.gameObject.SetActive(false);
    }


    private void Update()
    {
        if(ObjectHelper.IsInView(Camera.main,speakerTransform) && changeWithView)
        {
            GoToSpeakerTransform();
        }
        else{
            GoToCameraTransform();
        }

        SetSizeDependingCameraDistance();

    }

    private void SetSizeDependingCameraDistance()
    {
        float distance = (this.transform.position - Camera.main.transform.position).magnitude;
        transform.localScale = distance * subtitle_size * Vector3.one;
    }

    public void Lock()
    {
        changeWithView = false;
        GoToSpeakerTransform();
    }
    public void Unlock()
    {
        changeWithView = true;
    }

    public void GoToSpeakerTransform()
    {
        if (!isAboveSpeaker)
        {
            if (speakerTransform == null) GoToCameraTransform();
            transform.SetParent(speakerTransform);
            TransitionManager.ChangeLocalPosition(this.gameObject, Vector3.zero, 0.5f);
            //TransitionManager.ChangeSize(this.gameObject, Vector3.one/2, 0.5f);
            isAboveSpeaker = true;
            
        }
    }

    public void GoToCameraTransform()
    {
        if (isAboveSpeaker)
        {
            transform.SetParent(GameManager.Instance.CameraSubtitleTransform);
            TransitionManager.ChangeLocalPosition(this.gameObject, Vector3.zero, 0.5f);
            //TransitionManager.ChangeSize(this.gameObject, Vector3.one/4, 0.5f);
            isAboveSpeaker = false;
        }
    }
}
