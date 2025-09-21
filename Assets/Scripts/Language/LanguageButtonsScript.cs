using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageButtonsScript : MonoBehaviour
{
    public Button plButton;
    public Button engButton;

    public Sprite plNormal;
    public Sprite plSelected;
    public Sprite engNormal;
    public Sprite engSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(InitAndUpdate());
    }

    IEnumerator InitAndUpdate()
    {
        yield return LocalizationSettings.InitializationOperation;
        UpdateButtonSprites();
    }

    public void UpdateButtonSprites()
    {
        string currentLang = LocalizationSettings.SelectedLocale.Identifier.Code;
        if (currentLang == "pl")
        {
            plButton.image.sprite = plSelected;
            engButton.image.sprite = engNormal;
        }
        else if (currentLang == "en")
        {
            plButton.image.sprite = plNormal;
            engButton.image.sprite = engSelected;
        }
    }

    public void SetPolish()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("pl");
        PlayerPrefs.SetString("SelectedLanguage", "pl");
        PlayerPrefs.Save();
        UpdateButtonSprites();
    }

    public void SetEnglish()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
        PlayerPrefs.SetString("SelectedLanguage", "en");
        PlayerPrefs.Save();
        UpdateButtonSprites();
    }
}
