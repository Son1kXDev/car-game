using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class Localization : MonoBehaviour
{

    public static Lang GetCurrentLanguage()
    {
        Locale currentSelectedLocale = LocalizationSettings.SelectedLocale;
        ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;
        if (currentSelectedLocale == availableLocales.GetLocale("ru"))
            return Lang.Russian;
        else return Lang.English;
    }

}

[System.Serializable]
public class LanguageLocalizedString
{
    public string EN;
    public string RU;
}

public enum Lang
{ English, Russian }
