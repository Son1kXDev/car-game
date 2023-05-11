using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace UnityEngine.Localization.Custom
{
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
    public class LocalizedString
    {
        public string EN;
        public string RU;
    }

    public enum Lang
    { English, Russian }

}