using TMPro;
using UnityEngine;

public class MyCustomSubtitle : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
