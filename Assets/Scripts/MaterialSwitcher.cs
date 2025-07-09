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

    [SerializeField] bool OnStart = false;

    private void Start()
    {
        ChangeColors(false);
        if(OnStart)
            ChangeToEnd();
    }

    public void ChangeToStart()
    {
        ChangeColors(false,timeToChange);
    }

    public void ChangeToEnd()
    {
        ChangeColors(true,timeToChange);
    }

    public void ChangeColors(bool ToEnd, float duration = 0)
    {
        TransitionManager.InterpolateFloat(0, 1, duration, t =>
        {
            foreach (var m in materialNameColorList)
            {
                Color interpolatedColor = Color.Lerp(ToEnd ? m.colorStart : m.colorEnd, ToEnd ? m.colorEnd : m.colorStart, t);
                material.SetColor(m.name, interpolatedColor);
            }
            print("ishdfgi"+t);
        });
    }
}
