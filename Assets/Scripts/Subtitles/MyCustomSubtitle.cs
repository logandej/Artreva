using System.Collections;
using TMPro;
using UnityEngine;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] string speakerName;
    [SerializeField] Transform speakerTransform;
    [SerializeField] Canvas canvas;
    [Header("Settings")]
    [SerializeField] TMP_Text speakerNameText;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] float typingSpeed = 0.1f;
    private bool isAboveSpeaker=true;

    private void Start()
    {
        HideSubtitle();
        GoToSpeakerTransform();
    }

    public void SetText(string text)
    {
      
        speakerNameText.text = speakerName;
        canvas.gameObject.SetActive(true);
        StartCoroutine(TypeText(text));
    }

    public void HideSubtitle()
    {
        canvas.gameObject.SetActive(false);
    }


    private IEnumerator TypeText(string text)
    {
        this.subtitleText.text = "";
        foreach (char c in text)
        {
            this.subtitleText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void Update()
    {
        if(ObjectHelper.IsInView(Camera.main,speakerTransform))
        {
            GoToSpeakerTransform();
        }
        else{
            GoToCameraTransform();
        }
    }

    public void GoToSpeakerTransform()
    {
        if (!isAboveSpeaker)
        {
            transform.SetParent(null);
            TransitionManager.ChangeLocalPosition(this.gameObject, speakerTransform.position + Vector3.up, 0.5f);
            TransitionManager.ChangeSize(this.gameObject, Vector3.one, 0.5f);
            isAboveSpeaker = true;
            
        }
    }

    public void GoToCameraTransform()
    {
        if (isAboveSpeaker)
        {
            transform.SetParent(GameManager.Instance.cameraSubtitleTransform);
            TransitionManager.ChangeLocalPosition(this.gameObject, Vector3.zero, 0.5f);
            TransitionManager.ChangeSize(this.gameObject, Vector3.one/4, 0.5f);
            isAboveSpeaker = false;
        }
    }
}
