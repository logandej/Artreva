using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorStep : MonoBehaviour
{
    public static IndicatorStep Instance { get; private set; }
    //3D Canvas
    [SerializeField] Canvas canvas;
    [SerializeField] TMP_Text text;
    [SerializeField] Image image;
    [SerializeField] float durationTransition = 1;
    
    public void SetText(string keyText)
    {
        UIManager.SetTextByKey(text, keyText);
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void GoTo(Transform t)
    {
        TransitionManager.ChangePosition(this.gameObject, t.position, durationTransition);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
