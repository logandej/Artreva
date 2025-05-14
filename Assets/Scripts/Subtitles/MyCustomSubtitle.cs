using System.Collections;
using TMPro;
using UnityEngine;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] string speakerName;
    [SerializeField] Transform speakerTransform;
    [Header("Settings")]
    [SerializeField] TMP_Text speakerNameText;
    [SerializeField] TMP_Text subtitleText;
    [SerializeField] float typingSpeed = 0.1f;
    private bool isAboveSpeaker=true;

    private void Start()
    {
        GoToSpeakerTransform();
    }

    public void SetText(string text)
    {
        speakerNameText.text = speakerName;
        //this.text.text = text;
        StartCoroutine(TypeText(text));
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
            TransitionManager.ChangePosition(this.gameObject, speakerTransform.position + Vector3.up, 0.5f);
            TransitionManager.ChangeSize(this.gameObject, 1, 0.5f);
            transform.SetParent(null);
            isAboveSpeaker = true;
            
        }
    }

    public void GoToCameraTransform()
    {
        if (isAboveSpeaker)
        {
            TransitionManager.ChangePosition(this.gameObject, GameManager.Instance.cameraSubtitleTransform.position, 0.5f);
            TransitionManager.ChangeSize(this.gameObject, .3f, 0.5f);
            transform.SetParent(GameManager.Instance.cameraSubtitleTransform);
            isAboveSpeaker = false;
        }
    }
}
