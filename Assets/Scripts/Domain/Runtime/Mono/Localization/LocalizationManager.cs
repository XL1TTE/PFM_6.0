using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    [SerializeField] private Language currentLanguage = Language.English;
    [SerializeField] private bool useGoogleSheets = true;

    private string googleSheetsURL = "https://docs.google.com/spreadsheets/d/1bQsGJFZ-rK9mrZmduIVEIq-D6W8D7AK6P4mv0x0Mexk/export?format=tsv";

    private Dictionary<string, Dictionary<string, string>> localizationDictionary =
        new Dictionary<string, Dictionary<string, string>>();

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
        Debug.Log("Loading localization from Google Sheets (TSV)...");

        UnityWebRequest webRequest = UnityWebRequest.Get(googleSheetsURL);

        yield return webRequest.SendWebRequest();

        bool hasError = false;
        string errorMessage = "";

        try
        {
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string tsvData = webRequest.downloadHandler.text;

                Debug.Log($"First 100 chars: {tsvData.Substring(0, Mathf.Min(100, tsvData.Length))}");

                ProcessTSVData(tsvData);
                Debug.Log("Localization loaded from Google Sheets successfully");

                SaveLocalCopy(tsvData);
            }
            else
            {
                hasError = true;
                errorMessage = $"Failed to load Google Sheets: {webRequest.error}";
            }
        }
        catch (System.Exception e)
        {
            hasError = true;
            errorMessage = $"Error loading from Google Sheets: {e.Message}";
        }
        finally
        {
            webRequest.Dispose();
        }

        if (hasError)
        {
            Debug.LogError(errorMessage);
            LoadFromResources();
        }

        isLocalizationLoaded = true;

        OnLanguageChanged?.Invoke();

        yield return new WaitForSeconds(0.1f);
        ForceRegisterAllTexts();
        UpdateAllLocalizedTexts();
    }

    private void ProcessTSVData(string tsvData)
    {
        localizationDictionary.Clear();

        if (string.IsNullOrEmpty(tsvData))
        {
            Debug.LogError("TSV data is empty!");
            LoadFromResources();
            return;
        }

        string[] lines = tsvData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2)
        {
            Debug.LogError($"TSV has insufficient lines! Only {lines.Length} lines.");
            return;
        }

        Debug.Log($"Total lines in TSV: {lines.Length}");

        string[] headers = ParseTSVLine(lines[0]);
        Debug.Log($"Headers: {string.Join(", ", headers)}");

        for (int i = 1; i < headers.Length; i++)
        {
            string language = headers[i].ToLower();
            if (!localizationDictionary.ContainsKey(language))
            {
                localizationDictionary[language] = new Dictionary<string, string>();
                Debug.Log($"Created dictionary for language: {language}");
            }
        }

        int processedCount = 0;
        for (int row = 1; row < lines.Length; row++)
        {
            if (string.IsNullOrWhiteSpace(lines[row])) continue;

            string[] values = ParseTSVLine(lines[row]);
            if (values.Length < 2) continue;

            string key = values[0].Trim();

            if (string.IsNullOrEmpty(key)) continue;

            for (int col = 1; col < values.Length && col < headers.Length; col++)
            {
                string language = headers[col].ToLower();
                string value = values[col];

                if (!string.IsNullOrEmpty(value) && localizationDictionary.ContainsKey(language))
                {
                    string fullKey = $"{key}||{language}";

                    value = ReplaceNewlineMarkers(value);

                    localizationDictionary[language][fullKey] = value;

                    if (key.Contains("menu_") || key.Contains("settings_") || key.Contains("Demo"))
                    {
                        Debug.Log($"Added: {fullKey}");
                        Debug.Log($"Value (after newline replace): {value}");
                    }
                }
            }

            processedCount++;
        }

        Debug.Log($"Processed {processedCount} localization entries");

        foreach (var lang in localizationDictionary.Keys)
        {
            Debug.Log($"Language '{lang}': {localizationDictionary[lang].Count} entries");
        }

        DebugAvailableKeys();
    }

    private string ReplaceNewlineMarkers(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        string result = text;

        result = result.Replace("\\n", "\n");

        result = result.Replace("[NEWLINE]", "\n");
        result = result.Replace("[newline]", "\n");

        result = result.Replace("<br>", "\n");
        result = result.Replace("<br/>", "\n");
        result = result.Replace("<br />", "\n");

        result = result.Replace("%%n%%", "\n");

        result = result.Replace("\\\\n", "\\n");

        return result;
    }

    private string[] ParseTSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";
        char lastChar = '\0';

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == '\t' && !inQuotes)
            {
                if (current.StartsWith("\"") && current.EndsWith("\""))
                {
                    current = current.Substring(1, current.Length - 2);
                }

                result.Add(current.Trim());
                current = "";
            }
            else
            {
                current += c;
            }

            lastChar = c;
        }

        if (current.StartsWith("\"") && current.EndsWith("\""))
        {
            current = current.Substring(1, current.Length - 2);
        }

        result.Add(current.Trim());

        return result.ToArray();
    }

    private void DebugAvailableKeys()
    {
        Debug.Log("=== Available Localization Keys ===");

        foreach (var language in localizationDictionary.Keys)
        {
            Debug.Log($"Language: {language}");
            var keys = localizationDictionary[language].Keys.Take(10).ToList();
            foreach (var key in keys)
            {
                Debug.Log($"  {key}");
            }
            if (localizationDictionary[language].Count > 10)
            {
                Debug.Log($"  ... and {localizationDictionary[language].Count - 10} more");
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

        if (localizationDictionary.TryGetValue(languageCode, out var sheet))
        {
            string fullKey = $"{key}||{languageCode}";

            if (sheet.TryGetValue(fullKey, out var value))
            {
                return value;
            }
            else
            {
                foreach (var kvp in sheet)
                {
                    if (kvp.Key.StartsWith(key + "||"))
                    {
                        return kvp.Value;
                    }
                }
            }
        }

        if (languageCode != "en" && localizationDictionary.TryGetValue("en", out var enSheet))
        {
            foreach (var kvp in enSheet)
            {
                if (kvp.Key.StartsWith(key + "||"))
                {
                    Debug.LogWarning($"Using English fallback for: {key}");
                    return kvp.Value;
                }
            }
        }

        Debug.LogWarning($"Localization key not found: '{key}' for language: {languageCode}");
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

    private void SaveLocalCopy(string tsvData)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, "localization_backup.tsv");
            File.WriteAllText(path, tsvData);
            Debug.Log($"Localization backup saved to: {path}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save backup: {e.Message}");
        }
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
                    foreach (var entry in sheet.entries)
                    {
                        foreach (var translation in entry.translations)
                        {
                            string language = translation.language.ToLower();
                            if (!localizationDictionary.ContainsKey(language))
                            {
                                localizationDictionary[language] = new Dictionary<string, string>();
                            }

                            string fullKey = $"{entry.key}||{language}";
                            localizationDictionary[language][fullKey] = translation.text;
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
}