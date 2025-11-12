using UnityEngine;
using UnityEngine.Localization.Settings;

public class OrbLanguage : MonoBehaviour
{
    [SerializeField] Material selectedM;
    [SerializeField] Material notSelectedM;

    [SerializeField] MeshRenderer mesh;

    [SerializeField] int languageIndex = 1;

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = 0.1f;
        IsSelected(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[languageIndex]);
    }
    public void IsSelected(bool selected)
    {
        mesh.material = selected ? this.selectedM : this.notSelectedM;
    }
}
