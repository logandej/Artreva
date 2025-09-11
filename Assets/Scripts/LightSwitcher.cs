using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Rendering;

public class LightSwitcher : MonoBehaviour
{
    public Light sunLight;
    public Color dayColor, nightColor;
    public float dayIntensity, nightIntensity;
    public float dayAmbiantIntensity, nightAmbiantIntensity;
    //public Color dayAmbient, nightAmbient;

    public Color sunSkyboxColor, nightSkyboxColor;

    public void SwitchToNight()
    {

     
        ChangeLightNight();
       
        //RenderSettings.ambientLight = nightAmbient;

        TransitionManager.InterpolateFloat(0, 1, 5, t =>
        {
            Color interpolatedColor = Color.Lerp(sunSkyboxColor, nightSkyboxColor, t);
            Color lightLerpColor = Color.Lerp(dayColor, nightColor, t);
            sunLight.color = lightLerpColor;
            RenderSettings.skybox.SetColor("_Tint", interpolatedColor); // "_Tint" dépend du shader utilisé
        });
    }

    public void SwitchToDay()
    {

        ChangeLightDay();

        //RenderSettings.ambientLight = nightAmbient;

        TransitionManager.InterpolateFloat(0, 1, 5, t =>
        {
            Color interpolatedColor = Color.Lerp(nightSkyboxColor, sunSkyboxColor, t);
            Color lightLerpColor = Color.Lerp(nightColor, dayColor, t);
            sunLight.color = lightLerpColor;
            RenderSettings.skybox.SetColor("_Tint", interpolatedColor); // "_Tint" dépend du shader utilisé
        });

    }

    public void ChangeLightNight()
    {
        TransitionManager.InterpolateFloat(dayIntensity, nightIntensity, 5, intensity => {
            sunLight.intensity = intensity;
        });

        TransitionManager.InterpolateFloat(dayAmbiantIntensity, nightAmbiantIntensity, 5, intensity =>
        {
            RenderSettings.ambientIntensity = intensity;
        });
    }

    public void ChangeLightDay()
    {
        TransitionManager.InterpolateFloat(nightIntensity, dayIntensity, 5, intensity => {
            sunLight.intensity = intensity;
        });

        TransitionManager.InterpolateFloat(nightAmbiantIntensity, dayAmbiantIntensity, 5, intensity =>
        {
            RenderSettings.ambientIntensity = intensity;
        });
    }

    public void ChangeColorSkybox(string color)
    {
        ChangeColorSkybox(color,2);
    }

    public void ChangeColorSkybox(string color, float duration)
    {
        // Récupère la couleur actuelle du skybox
        Color baseColor = RenderSettings.skybox.GetColor("_Tint"); 

        // Conversion string -> Color
        if (!ColorUtility.TryParseHtmlString(color, out Color targetColor))
        {
            Debug.LogWarning($"Impossible de convertir '{color}' en Color. Utiliser un format HTML (#RRGGBB ou #RRGGBBAA).");
            return;
        }

        // Interpolation via ton TransitionManager
        TransitionManager.InterpolateFloat(0, 1, duration, t =>
        {
            Color interpolatedColor = Color.Lerp(baseColor, targetColor, t);
            RenderSettings.skybox.SetColor("_Tint", interpolatedColor);
        });
    }

}