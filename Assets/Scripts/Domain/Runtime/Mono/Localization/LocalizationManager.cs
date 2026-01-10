using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [SerializeField] private Language currentLanguage = Language.English;
    [SerializeField] private bool useGoogleSheets = true;

    private string googleSheetsURL = "https://docs.google.com/spreadsheets/d/1bQsGJFZ-rK9mrZmduIVEIq-D6W8D7AK6P4mv0x0Mexk/export?format=xlsx";

    // Замени словарь на трехмерную структуру
    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> localizationDictionary =
        new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

    private Dictionary<Language, string> languageCodes = new Dictionary<Language, string>
    {
        { Language.English, "en" },
        { Language.Russian, "ru" }
    };

    public event System.Action OnLanguageChanged;

    private static List<LocalizedText> allLocalizedTexts = new List<LocalizedText>();

    private bool isLocalizationLoaded = false;

    public static void RegisterLocalizedText(LocalizedText text)
    {
        if (text == null) return;

        if (!allLocalizedTexts.Contains(text))
        {
            allLocalizedTexts.Add(text);
            Debug.Log($"Registered LocalizedText: {text.gameObject.name} (Key: {text.localizationKey}) (Total: {allLocalizedTexts.Count})");

            if (Instance != null && Instance.isLocalizationLoaded)
            {
                text.UpdateText();
            }
        }
    }

    public static void UnregisterLocalizedText(LocalizedText text)
    {
        if (text != null && allLocalizedTexts.Contains(text))
        {
            allLocalizedTexts.Remove(text);
        }
    }

    public void UpdateAllLocalizedTexts()
    {
        Debug.Log($"=== Updating ALL localized texts ({allLocalizedTexts.Count} total) ===");

        int updatedCount = 0;
        int failedCount = 0;

        var textsToUpdate = new List<LocalizedText>(allLocalizedTexts);

        foreach (var text in textsToUpdate)
        {
            try
            {
                if (text != null && text.gameObject != null)
                {
                    text.UpdateText();
                    updatedCount++;
                }
                else
                {
                    allLocalizedTexts.Remove(text);
                    failedCount++;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating text {text?.gameObject?.name}: {e.Message}");
                failedCount++;
            }
        }

        Debug.Log($"Updated: {updatedCount}, Failed: {failedCount}, Remaining: {allLocalizedTexts.Count}");
    }

    public void ForceRegisterAllTexts()
    {
        Debug.Log("=== Force registering all LocalizedText components ===");

        var allTexts = FindObjectsOfType<LocalizedText>(true);
        Debug.Log($"Found {allTexts.Length} LocalizedText components in scene");

        foreach (var text in allTexts)
        {
            RegisterLocalizedText(text);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("LocalizationManager should be a root GameObject for DontDestroyOnLoad!");
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        LoadSavedLanguage();

        Initialize();
    }

    private void LoadSavedLanguage()
    {
        string savedLang = PlayerPrefs.GetString("SelectedLanguage", Language.English.ToString());
        if (Enum.TryParse(savedLang, out Language savedLanguage))
        {
            currentLanguage = savedLanguage;
            Debug.Log($"Loaded saved language: {savedLanguage}");
        }
    }

    private void Initialize()
    {
        if (useGoogleSheets)
        {
            StartCoroutine(LoadFromGoogleSheetsCoroutine());
        }
        else
        {
            LoadFromResources();
        }
    }

    private IEnumerator LoadFromGoogleSheetsCoroutine()
    {
        Debug.Log("Loading localization from Google Sheets (Excel)...");

        UnityWebRequest webRequest = UnityWebRequest.Get(googleSheetsURL);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            byte[] excelData = webRequest.downloadHandler.data;
            ProcessExcelData(excelData);
        }
        else
        {
            Debug.LogError($"Failed to load Excel file: {webRequest.error}");
            LoadFromResources();
        }

        webRequest.Dispose();
    }

    private void ProcessExcelData(byte[] excelData)
    {
        // Критически важная строка!
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        using (var stream = new MemoryStream(excelData))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                // Очищаем текущий словарь
                localizationDictionary.Clear();

                // Проходим по всем таблицам (вкладкам)
                foreach (DataTable table in result.Tables)
                {
                    string sheetName = table.TableName;

                    // Инициализируем словарь для этой вкладки
                    if (!localizationDictionary.ContainsKey(sheetName))
                    {
                        localizationDictionary[sheetName] = new Dictionary<string, Dictionary<string, string>>();
                    }

                    // Получаем заголовки столбцов (первая строка)
                    List<string> columnHeaders = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columnHeaders.Add(column.ColumnName.ToString());
                    }

                    // Если нет колонок, пропускаем таблицу
                    if (columnHeaders.Count == 0)
                    {
                        Debug.LogWarning($"Sheet '{sheetName}' has no columns, skipping");
                        continue;
                    }

                    // Проходим по всем строкам (начиная со второй, т.к. первая - заголовки)
                    for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                    {
                        DataRow row = table.Rows[rowIndex];

                        // Получаем ключ из первого столбца
                        string key = row[0]?.ToString();
                        if (string.IsNullOrEmpty(key))
                        {
                            continue; // Пропускаем строки без ключа
                        }

                        // Проходим по всем языковым колонкам (начиная со второй)
                        for (int colIndex = 1; colIndex < columnHeaders.Count; colIndex++)
                        {
                            string languageCode = columnHeaders[colIndex];
                            string value = row[colIndex]?.ToString();

                            // Инициализируем словарь для языка, если его еще нет
                            if (!localizationDictionary[sheetName].ContainsKey(languageCode))
                            {
                                localizationDictionary[sheetName][languageCode] = new Dictionary<string, string>();
                            }

                            // Добавляем значение
                            if (!string.IsNullOrEmpty(value))
                            {
                                localizationDictionary[sheetName][languageCode][key] = value;
                            }
                            else
                            {
                                // Для отладки: логируем пустые значения
                                Debug.LogWarning($"Empty value for key '{key}' in sheet '{sheetName}', language '{languageCode}'");
                            }
                        }
                    }

                    Debug.Log($"Loaded sheet '{sheetName}' with {table.Rows.Count} rows and {table.Columns.Count} columns");
                }

                Debug.Log($"Successfully loaded {result.Tables.Count} sheets");
                isLocalizationLoaded = true;

                // После загрузки обновляем все тексты
                UpdateAllLocalizedTexts();
            }
        }
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
        PlayerPrefs.SetString("SelectedLanguage", language.ToString());
        PlayerPrefs.Save();

        Debug.Log($"Language changed to: {language}");

        OnLanguageChanged?.Invoke();

        UpdateAllLocalizedTexts();
    }

    // Обновленный GetLocalizedValue для работы с новой структурой
    public string GetLocalizedValue(string key, string sheetName = "UI_Menu")
    {
        if (!isLocalizationLoaded)
        {
            Debug.LogWarning($"Localization not loaded yet, can't get value for key: {key}");
            return $"[{key}]";
        }

        if (!languageCodes.TryGetValue(currentLanguage, out string languageCode))
        {
            languageCode = "en";
        }

        // Пытаемся найти в указанной вкладке
        if (!string.IsNullOrEmpty(sheetName) && localizationDictionary.ContainsKey(sheetName))
        {
            var sheet = localizationDictionary[sheetName];
            if (sheet.TryGetValue(languageCode, out var langDict) && langDict.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        // Если не нашли, ищем во всех вкладках
        foreach (var sheet in localizationDictionary.Values)
        {
            if (sheet.TryGetValue(languageCode, out var langDict) && langDict.TryGetValue(key, out var value))
            {
                Debug.LogWarning($"Key '{key}' not found in sheet '{sheetName}', using from another sheet");
                return value;
            }
        }

        // Английский фолбэк
        if (languageCode != "en")
        {
            foreach (var sheet in localizationDictionary.Values)
            {
                if (sheet.TryGetValue("en", out var langDict) && langDict.TryGetValue(key, out var value))
                {
                    Debug.LogWarning($"Using English fallback for: {key}");
                    return value;
                }
            }
        }

        Debug.LogWarning($"Localization key not found: '{key}' in sheet '{sheetName}' for language: {languageCode}");
        return $"[{key}]";
    }
    
    public string GetFormattedValue(string key, params object[] args)
    {
        string text = GetLocalizedValue(key);
        try
        {
            return string.Format(text, args);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error formatting text for key '{key}': {e.Message}");
            return text;
        }
    }

    public string GetLocalizedTextEvent(string eventId, string field)
    {
        return GetLocalizedValue($"{eventId}_{field}", "TextEvents");
    }

    private void LoadFromResources()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Localization/localization_data");
        if (jsonFile != null)
        {
            try
            {
                Debug.Log("Loading localization from Resources...");
                var wrapper = JsonUtility.FromJson<LocalizationDataWrapper>(jsonFile.text);

                foreach (var sheet in wrapper.sheets)
                {
                    string sheetName = sheet.name;
                    if (!localizationDictionary.ContainsKey(sheetName))
                    {
                        localizationDictionary[sheetName] = new Dictionary<string, Dictionary<string, string>>();
                    }

                    foreach (var entry in sheet.entries)
                    {
                        foreach (var translation in entry.translations)
                        {
                            string language = translation.language.ToLower();
                            if (!localizationDictionary[sheetName].ContainsKey(language))
                            {
                                localizationDictionary[sheetName][language] = new Dictionary<string, string>();
                            }

                            localizationDictionary[sheetName][language][entry.key] = translation.text;
                        }
                    }
                }

                Debug.Log("Localization loaded from Resources successfully");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Localization file not found in Resources, creating default");
        }

        isLocalizationLoaded = true;
    }
    public void DebugCurrentData()
    {
        Debug.Log($"=== Current Language: {currentLanguage} ===");

        string langCode = languageCodes[currentLanguage];
        if (localizationDictionary.TryGetValue(langCode, out var dict))
        {
            Debug.Log($"Total keys: {dict.Count}");
            foreach (var kvp in dict)
            {
                Debug.Log($"{kvp.Key}: {kvp.Value}");
            }
        }
    }

    [System.Serializable]
    private class LocalizationDataWrapper
    {
        public List<LocalizationSheet> sheets;
    }

    [System.Serializable]
    private class LocalizationSheet
    {
        public string name;
        public List<LocalizationEntry> entries;
    }

    [System.Serializable]
    private class LocalizationEntry
    {
        public string key;
        public List<Translation> translations;
    }

    [System.Serializable]
    private class Translation
    {
        public string language;
        public string text;
    }
    public List<string> GetAvailableSheets()
    {
        return localizationDictionary.Keys.ToList();
    }
    public Language CurrentLanguage
    {
        get { return currentLanguage; }
    }
}