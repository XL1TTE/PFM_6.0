// LanguageSelector.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageSelector : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text languageLabelText;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text currentLanguageText;

    [Header("Localization Keys")]
    [SerializeField] private string languageLabelKey = "SettingsMenu_TitleLanguage";

    private Language[] availableLanguages = { Language.English, Language.Russian };
    private int currentLanguageIndex = 0;

    private void Start()
    {
        // Находим сохраненный язык
        string savedLang = PlayerPrefs.GetString("SelectedLanguage", Language.English.ToString());
        Language savedLanguage = Language.English;
        System.Enum.TryParse(savedLang, out savedLanguage);

        // Находим индекс сохраненного языка
        for (int i = 0; i < availableLanguages.Length; i++)
        {
            if (availableLanguages[i] == savedLanguage)
            {
                currentLanguageIndex = i;
                break;
            }
        }

        InitializeUI();
        UpdateDisplay();

        // Подписываемся на события
        previousButton.onClick.AddListener(PreviousLanguage);
        nextButton.onClick.AddListener(NextLanguage);

        // Подписываемся на изменение языка для обновления текста
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateLocalizedTexts;
        }
    }

    private void InitializeUI()
    {
        // Настраиваем LocalizedText для label если он есть
        if (languageLabelText != null && languageLabelText.GetComponent<LocalizedText>() == null)
        {
            var localizedText = languageLabelText.gameObject.AddComponent<LocalizedText>();
            localizedText.localizationKey = languageLabelKey;
            localizedText.sheetName = "UI_Menu";
            localizedText.updateOnAwake = true;
            localizedText.updateOnLanguageChange = true;
        }
    }

    private void UpdateLocalizedTexts()
    {
        // Обновляем текст текущего языка
        UpdateCurrentLanguageText();
    }

    private void PreviousLanguage()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        currentLanguageIndex--;
        if (currentLanguageIndex < 0)
            currentLanguageIndex = availableLanguages.Length - 1;

        ChangeLanguage();
    }

    private void NextLanguage()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        currentLanguageIndex++;
        if (currentLanguageIndex >= availableLanguages.Length)
            currentLanguageIndex = 0;

        ChangeLanguage();
    }

    private void ChangeLanguage()
    {
        Language newLanguage = availableLanguages[currentLanguageIndex];
        LocalizationManager.Instance.SetLanguage(newLanguage);
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        UpdateCurrentLanguageText();
    }

    private void UpdateCurrentLanguageText()
    {
        if (currentLanguageText != null)
        {
            currentLanguageText.text = availableLanguages[currentLanguageIndex].ToString();
        }
    }

    private void OnDestroy()
    {
        if (previousButton != null)
            previousButton.onClick.RemoveListener(PreviousLanguage);

        if (nextButton != null)
            nextButton.onClick.RemoveListener(NextLanguage);

        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateLocalizedTexts;
        }
    }
}