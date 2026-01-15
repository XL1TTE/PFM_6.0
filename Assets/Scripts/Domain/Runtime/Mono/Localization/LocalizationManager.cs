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
                failedCount++;
            }
        }

    }

    public void ForceRegisterAllTexts()
    {

        var allTexts = FindObjectsOfType<LocalizedText>(true);

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
        UnityWebRequest webRequest = UnityWebRequest.Get(googleSheetsURL);
        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            byte[] excelData = webRequest.downloadHandler.data;
            ProcessExcelData(excelData);
        }
        else
        {
            LoadFromResources();
        }

        webRequest.Dispose();
    }

    private void ProcessExcelData(byte[] excelData)
    {
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

                localizationDictionary.Clear();

                foreach (DataTable table in result.Tables)
                {
                    string sheetName = table.TableName;

                    if (!localizationDictionary.ContainsKey(sheetName))
                    {
                        localizationDictionary[sheetName] = new Dictionary<string, Dictionary<string, string>>();
                    }

                    List<string> columnHeaders = new List<string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        columnHeaders.Add(column.ColumnName.ToString());
                    }

                    if (columnHeaders.Count == 0)
                    {
                        continue;
                    }

                    for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
                    {
                        DataRow row = table.Rows[rowIndex];

                        string key = row[0]?.ToString();
                        if (string.IsNullOrEmpty(key))
                        {
                            continue;
                        }

                        for (int colIndex = 1; colIndex < columnHeaders.Count; colIndex++)
                        {
                            string languageCode = columnHeaders[colIndex];
                            string value = row[colIndex]?.ToString();

                            if (!localizationDictionary[sheetName].ContainsKey(languageCode))
                            {
                                localizationDictionary[sheetName][languageCode] = new Dictionary<string, string>();
                            }

                            if (!string.IsNullOrEmpty(value))
                            {
                                localizationDictionary[sheetName][languageCode][key] = value;
                            }
                        }
                    }

                }

                isLocalizationLoaded = true;

                UpdateAllLocalizedTexts();
            }
        }
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
        PlayerPrefs.SetString("SelectedLanguage", language.ToString());
        PlayerPrefs.Save();

        OnLanguageChanged?.Invoke();

        UpdateAllLocalizedTexts();
    }

    public string GetLocalizedValue(string key, string sheetName = "UI_Menu")
    {
        if (!isLocalizationLoaded)
        {
            return $"[{key}]";
        }

        if (!languageCodes.TryGetValue(currentLanguage, out string languageCode))
        {
            languageCode = "en";
        }

        if (!string.IsNullOrEmpty(sheetName) && localizationDictionary.ContainsKey(sheetName))
        {
            var sheet = localizationDictionary[sheetName];
            if (sheet.TryGetValue(languageCode, out var langDict) && langDict.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        foreach (var sheet in localizationDictionary.Values)
        {
            if (sheet.TryGetValue(languageCode, out var langDict) && langDict.TryGetValue(key, out var value))
            {
                return value;
            }
        }

        if (languageCode != "en")
        {
            foreach (var sheet in localizationDictionary.Values)
            {
                if (sheet.TryGetValue("en", out var langDict) && langDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
        }

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

            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse JSON: {e.Message}");
            }
        }

        isLocalizationLoaded = true;
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