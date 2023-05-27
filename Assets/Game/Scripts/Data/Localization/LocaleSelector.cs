using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool active = false;

    public void ChangeLocale(int localeID)
    {
        if (!active)
        {
            StartCoroutine(SetLocale(localeID));
            GlobalEventManager.Instance.LanguageChanged();
        }
    }

    private IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
    }
}
