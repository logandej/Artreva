using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Canvas canvas;
    public Image image;

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
        image.gameObject.SetActive(false);
    }

    public void ShowInfo(Sprite sprite)
    {
        image.sprite = sprite;
        image.color = new Color(1, 1, 1, 0);
        image.gameObject.SetActive(true);

        TransitionManager.InterpolateFloat(0f, 1f, 0.5f, alpha =>
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        });

        Invoke(nameof(HideInfo), 5f);
    }


    private void HideInfo()
    {
        TransitionManager.InterpolateFloat(1f, 0f, 0.5f, alpha =>
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        });

        StartCoroutine(DisableAfter(0.5f));
    }

    private IEnumerator DisableAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        image.gameObject.SetActive(false);
    }

}
