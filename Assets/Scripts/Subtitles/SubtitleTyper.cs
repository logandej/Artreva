using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleTyper : MonoBehaviour
{
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private float typingSpeed = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TypeText(string text)
    {
        subtitleText.text = "";
        foreach (char c in text)
        {
            subtitleText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

}
