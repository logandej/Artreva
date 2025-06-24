using UnityEngine;

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

        TransitionManager.InterpolateFloat(dayIntensity, nightIntensity,5, intensity => {
            sunLight.intensity = intensity;
        });

        TransitionManager.InterpolateFloat(dayAmbiantIntensity, nightAmbiantIntensity, 5, intensity =>
        {
            RenderSettings.ambientIntensity = intensity;
        });

       
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
        TransitionManager.InterpolateFloat(nightIntensity, dayIntensity, 5, intensity => {
            sunLight.intensity = intensity;
        });

        TransitionManager.InterpolateFloat(nightAmbiantIntensity, dayAmbiantIntensity, 5, intensity =>
        {
            RenderSettings.ambientIntensity = intensity;
        });


        //RenderSettings.ambientLight = nightAmbient;

        TransitionManager.InterpolateFloat(0, 1, 5, t =>
        {
            Color interpolatedColor = Color.Lerp(nightSkyboxColor, sunSkyboxColor, t);
            Color lightLerpColor = Color.Lerp(nightColor, dayColor, t);
            sunLight.color = lightLerpColor;
            RenderSettings.skybox.SetColor("_Tint", interpolatedColor); // "_Tint" dépend du shader utilisé
        });

    }

}