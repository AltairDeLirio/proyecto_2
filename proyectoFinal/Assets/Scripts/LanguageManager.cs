using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;

        // Load saved language
        int languageID = PlayerPrefs.GetInt("Language", 0);

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[languageID];
    }

    public void SetLanguage(int languageID)
    {
        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[languageID];

        // Save choice permanently
        PlayerPrefs.SetInt("Language", languageID);
        PlayerPrefs.Save();
    }
}