using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    [System.Serializable]
    public class MaterialNameColor
    {
        public string name;
        public Color colorStart;
        public Color colorEnd;
    }

    public List<MaterialNameColor> materialNameColorList;

    public Material material;
    public float timeToChange = 5;

    [SerializeField] bool OnStartChangeToEnd = false;
    [SerializeField] bool OnStartChangeToStart = false;

    private void Start()
    {
        ChangeColors(false);
        if(OnStartChangeToEnd)
            ChangeToEnd();
        if(OnStartChangeToStart)
            ChangeToStart();
    }

    public void ChangeToStart()
    {
        ChangeColors(false,timeToChange);
    }

    public void ChangeToStart(float duration)
    {
        ChangeColors(false, duration);
    }

    public void ChangeToEnd()
    {
        ChangeColors(true,timeToChange);
    }

    public void ChangeToEnd(float duration)
    {
        ChangeColors(true, duration);
    }

    private Coroutine currentColorTransition;


    public void ChangeColors(bool ToEnd, float duration = 0)
    {
        if (currentColorTransition != null)
        {
            StopCoroutine(currentColorTransition);
            currentColorTransition = null;
        }

        currentColorTransition = StartCoroutine(InterpolateColors(ToEnd, duration));
    }

    private IEnumerator InterpolateColors(bool toEnd, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            foreach (var m in materialNameColorList)
            {
                Color interpolatedColor = Color.Lerp(
                    toEnd ? m.colorStart : m.colorEnd,
                    toEnd ? m.colorEnd : m.colorStart,
                    t
                );
                material.SetColor(m.name, interpolatedColor);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final value is applied
        foreach (var m in materialNameColorList)
        {
            material.SetColor(m.name, toEnd ? m.colorEnd : m.colorStart);
        }

        currentColorTransition = null;
    }
}
