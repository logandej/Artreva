using System.Collections;
using TMPro;
using UnityEngine;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float typingSpeed = 0.1f;

    public void SetText(string text)
    {
        //this.text.text = text;
        StartCoroutine(TypeText(text));
    }


    private IEnumerator TypeText(string text)
    {
        this.text.text = "";
        foreach (char c in text)
        {
            this.text.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
