using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguagesScript : MonoBehaviour
{
    [System.Serializable]
    public struct LanguageButton
    {
        public Button button;
        public Locale locale;
    }
    public LanguageButton[] languageButtons;
    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        LoadSavedLanguage();
        foreach (var langBttn in languageButtons)
        {
            langBttn.button.onClick.AddListener(() => ChangeLanguage(langBttn.locale));
        }
    }
    void LoadSavedLanguage()
    {
        string savedLangCode = PlayerPrefs.GetString("SelectedLanguage", "");

        if (!string.IsNullOrEmpty(savedLangCode))
        {
            Locale savedLocale = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier(savedLangCode));
            if (savedLocale != null)
            {
                LocalizationSettings.SelectedLocale = savedLocale;
                Debug.Log($"Loaded saved language: " + savedLangCode);
                return;
            }
        }

        /*Locale deviceLocale = LocalizationSettings.AvailableLocales.GetLocale(Application.systemLanguage);
        if (deviceLocale != null)
        {
            LocalizationSettings.SelectedLocale = deviceLocale;
            Debug.Log($"Using device language: " + Application.systemLanguage);
        }*/
        // Zawsze ustaw angielski jako domyślny
        Locale englishLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
        if (englishLocale != null)
        {
            LocalizationSettings.SelectedLocale = englishLocale;
            Debug.Log("Using default language: English");
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            Debug.Log("Using default language.");
        }
    }
    void ChangeLanguage(Locale targetLocale)
    {
        LocalizationSettings.SelectedLocale = targetLocale;
        PlayerPrefs.SetString("SelectedLanguage", targetLocale.Identifier.Code);
        PlayerPrefs.Save();
        Debug.Log($"Language saved: {targetLocale.Identifier.Code}");
    }
}