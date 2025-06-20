using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] string speakerName;
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

    private string lastKey = "";

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
        canvas.gameObject.SetActive(true);

        //if (key.Equals("") || key.Equals(" "))
        //{
        //    key = IncrementKeySuffix(lastKey);
        //    print("OLALALLALALAL");
        //}
        //print("key  "+ key);
        subtitleText.text = GameManager.Instance.PrintLocalizedString(key);
        lastKey = key;
    }

    public void SetTextWithDuration(string key)
    {
        SetText(key);
        Invoke("HideSubtitle",2);

    }

    private string IncrementKeySuffix(string key)
    {
        int underscoreIndex = key.LastIndexOf('_');
        if (underscoreIndex == -1 || underscoreIndex == key.Length - 1)
            return key;

        string prefix = key.Substring(0, underscoreIndex + 1); // garde "sub.koonsa.4_"
        string numberStr = key.Substring(underscoreIndex + 1); // garde "03"

        if (!int.TryParse(numberStr, out int number))
            return key;

        number++; // incrémente

        // Si l'incrément donne un nombre à 2 chiffres ou plus plus de padding
        string result = (number >= 10) ? number.ToString() : number.ToString("D" + numberStr.Length);

        return prefix + result;
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
        if(ObjectHelper.IsInView(Camera.main,speakerTransform) && changeWithView)
        {
            GoToSpeakerTransform();
        }
        else{
            GoToCameraTransform();
        }
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
            TransitionManager.ChangeSize(this.gameObject, Vector3.one/2, 0.5f);
            isAboveSpeaker = true;
            
        }
    }

    public void GoToCameraTransform()
    {
        if (isAboveSpeaker)
        {
            transform.SetParent(GameManager.Instance.CameraSubtitleTransform);
            TransitionManager.ChangeLocalPosition(this.gameObject, Vector3.zero, 0.5f);
            TransitionManager.ChangeSize(this.gameObject, Vector3.one/4, 0.5f);
            isAboveSpeaker = false;
        }
    }
}
