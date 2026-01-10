using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;

[RequireComponent(typeof(TMP_Text))]
public class TMPAnimator : MonoBehaviour
{
    private TMP_Text textComponent;
    private Mesh mesh;
    private Vector3[] vertices;

    // Храним исходный текст с тегами
    [SerializeField, TextArea(3, 10)]
    private string sourceTextWithTags = "";

    // Интеграция с локализацией
    [Header("Localization Settings")]
    [SerializeField] private bool useLocalization = true;
    [SerializeField] private string localizationKey = "";
    [SerializeField] private string sheetName = "UI_Menu";

    // Глобальные настройки по умолчанию для эффектов
    [Header("Wave Effect Defaults")]
    private float defaultWaveSpeed = 3f;
    private float defaultWaveAmplitude = 2f;
    private float defaultWaveFrequency = 0.5f;
    private float defaultWaveOffset = 0f;

    [Header("Shake Effect Defaults")]
    private float defaultShakeIntensity = 1f;
    private float defaultShakeSpeed = 15f;
    private float defaultShakeRandomness = 0.5f;

    // Эффекты с параметрами по умолчанию
    [System.Serializable]
    public class EffectData
    {
        public Dictionary<int, WaveEffect> waveEffects = new Dictionary<int, WaveEffect>();
        public Dictionary<int, ShakeEffect> shakeEffects = new Dictionary<int, ShakeEffect>();
    }

    [System.Serializable]
    public class WaveEffect
    {
        public float speed;
        public float amplitude;
        public float frequency;
        public float offset;

        // Конструктор с параметрами по умолчанию
        public WaveEffect(float defaultSpeed, float defaultAmplitude, float defaultFrequency, float defaultOffset)
        {
            speed = defaultSpeed;
            amplitude = defaultAmplitude;
            frequency = defaultFrequency;
            offset = defaultOffset;
        }
    }

    [System.Serializable]
    public class ShakeEffect
    {
        public float intensity;
        public float speed;
        public float randomness;

        // Конструктор с параметрами по умолчанию
        public ShakeEffect(float defaultIntensity, float defaultSpeed, float defaultRandomness)
        {
            intensity = defaultIntensity;
            speed = defaultSpeed;
            randomness = defaultRandomness;
        }
    }

    private EffectData effectData = new EffectData();

    // Список стандартных тегов TMPro, которые нужно сохранять
    private HashSet<string> standardTmProTags = new HashSet<string>
    {
        "color", "size", "b", "i", "u", "s", "sup", "sub", "align", "allcaps", "smallcaps",
        "font", "font-weight", "margin", "mark", "mspace", "nobr", "page", "pos", "space",
        "sprite", "style", "voffset", "width", "br", "font=", "color=", "size=", "align="
    };

    private bool isLocalized = false;
    private bool isSubscribed = false;
    private bool isInitialized = false;

    private void Awake()
    {
        textComponent = GetComponent<TMP_Text>();

        // Если sourceTextWithTags пустой, берем из TMP_Text
        if (string.IsNullOrEmpty(sourceTextWithTags))
        {
            sourceTextWithTags = textComponent.text;
        }

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        // Ждем инициализации LocalizationManager
        yield return StartCoroutine(WaitForLocalizationManager());

        if (useLocalization && !string.IsNullOrEmpty(localizationKey))
        {
            isLocalized = true;

            // Подписываемся на изменение языка
            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
                isSubscribed = true;
                Debug.Log($"{gameObject.name}: Subscribed to language change for TMPAnimator");
            }

            // Ждем загрузки локализации
            yield return StartCoroutine(WaitForLocalizationData());

            // Обновляем текст из локализации
            UpdateTextFromLocalization();
        }
        else
        {
            // Если не используем локализацию, парсим как есть
            ParseTextFromSource();
        }

        isInitialized = true;
    }

    private IEnumerator WaitForLocalizationManager()
    {
        int maxWaitFrames = 120; // Ждем максимум 2 секунды
        int frameCount = 0;

        while (LocalizationManager.Instance == null && frameCount < maxWaitFrames)
        {
            frameCount++;
            yield return null;
        }

        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning($"{gameObject.name}: LocalizationManager not found after waiting");
        }
    }

    private IEnumerator WaitForLocalizationData()
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning($"{gameObject.name}: LocalizationManager is null");
            yield break;
        }

        // Проверяем, что локализация загружена
        int maxWaitFrames = 120;
        int frameCount = 0;

        // Ждем пока локализация загрузится
        // В вашем LocalizationManager должно быть свойство isLocalizationLoaded
        while (frameCount < maxWaitFrames)
        {
            // Проверяем через рефлексию или добавьте свойство в LocalizationManager
            // Для простоты ждем 5 кадров после создания менеджера
            if (LocalizationManager.Instance != null)
            {
                // Ждем еще немного для гарантии загрузки данных
                yield return new WaitForSeconds(0.1f);
                break;
            }
            frameCount++;
            yield return null;
        }
    }

    private void Start()
    {
        // Если еще не инициализировались, запускаем инициализацию
        if (!isInitialized)
        {
            StartCoroutine(Initialize());
        }
    }

    private void OnLanguageChanged()
    {
        if (useLocalization && !string.IsNullOrEmpty(localizationKey) && isInitialized)
        {
            Debug.Log($"{gameObject.name}: Language changed, updating TMPAnimator text");
            UpdateTextFromLocalization();
        }
    }

    private void UpdateTextFromLocalization()
    {
        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning($"{gameObject.name}: LocalizationManager is null");
            return;
        }

        // Получаем локализованный текст
        string localizedText = LocalizationManager.Instance.GetLocalizedValue(localizationKey, sheetName);

        Debug.Log($"{gameObject.name}: Getting localization for key '{localizationKey}', result: '{localizedText}'");

        if (!string.IsNullOrEmpty(localizedText) && localizedText != $"[{localizationKey}]")
        {
            sourceTextWithTags = localizedText;
            ParseTextFromSource();
            Debug.Log($"{gameObject.name}: Updated text from localization: {localizationKey}");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Failed to get localized text for key: {localizationKey}");
            // Можно отобразить fallback текст
            if (!string.IsNullOrEmpty(sourceTextWithTags))
            {
                ParseTextFromSource();
            }
        }
    }

    public void ParseTextFromSource()
    {
        if (textComponent == null) return;

        string originalText = sourceTextWithTags;
        string parsedText = "";

        effectData.waveEffects.Clear();
        effectData.shakeEffects.Clear();

        Stack<WaveEffect> waveStack = new Stack<WaveEffect>();
        Stack<ShakeEffect> shakeStack = new Stack<ShakeEffect>();

        int parsedIndex = 0;
        int i = 0;

        while (i < originalText.Length)
        {
            // Если находим тег
            if (originalText[i] == '<')
            {
                int endTag = originalText.IndexOf('>', i);
                if (endTag != -1)
                {
                    string fullTag = originalText.Substring(i, endTag - i + 1);
                    string tagContent = originalText.Substring(i + 1, endTag - i - 1).ToLower();

                    // Проверяем, является ли это нашим тегом wave
                    if (tagContent.StartsWith("wave"))
                    {
                        WaveEffect effect = ParseWaveTag(fullTag);
                        waveStack.Push(effect);
                        i = endTag + 1;
                        continue;
                    }

                    // Проверяем, является ли это нашим тегом shake
                    else if (tagContent.StartsWith("shake"))
                    {
                        ShakeEffect effect = ParseShakeTag(fullTag);
                        shakeStack.Push(effect);
                        i = endTag + 1;
                        continue;
                    }

                    // Проверяем, является ли это закрывающим тегом wave
                    else if (tagContent == "/wave")
                    {
                        if (waveStack.Count > 0)
                        {
                            waveStack.Pop();
                        }
                        i = endTag + 1;
                        continue;
                    }

                    // Проверяем, является ли это закрывающим тегом shake
                    else if (tagContent == "/shake")
                    {
                        if (shakeStack.Count > 0)
                        {
                            shakeStack.Pop();
                        }
                        i = endTag + 1;
                        continue;
                    }

                    // Если это стандартный тег TMPro - сохраняем его как есть
                    else if (IsStandardTmProTag(tagContent))
                    {
                        parsedText += fullTag;
                        i = endTag + 1;
                        continue;
                    }
                }
            }

            // Добавляем символ в очищенный текст
            parsedText += originalText[i];

            // Применяем активные эффекты к текущему символу
            if (waveStack.Count > 0)
            {
                effectData.waveEffects[parsedIndex] = waveStack.Peek();
            }

            if (shakeStack.Count > 0)
            {
                effectData.shakeEffects[parsedIndex] = shakeStack.Peek();
            }

            parsedIndex++;
            i++;
        }

        // Устанавливаем очищенный текст в TMP_Text
        textComponent.text = parsedText;
        textComponent.ForceMeshUpdate();

        Debug.Log($"{gameObject.name}: Parsed text: '{parsedText}'");
    }

    // Проверяем, является ли тег стандартным тегом TMPro
    private bool IsStandardTmProTag(string tagContent)
    {
        // Убираем / если есть закрывающий тег
        string cleanTag = tagContent.StartsWith("/") ? tagContent.Substring(1) : tagContent;

        // Проверяем основные теги
        if (standardTmProTags.Contains(cleanTag))
            return true;

        // Проверяем теги с параметрами (color=, size= и т.д.)
        foreach (var tag in standardTmProTags)
        {
            if (cleanTag.StartsWith(tag))
                return true;
        }

        return false;
    }

    private WaveEffect ParseWaveTag(string tag)
    {
        // Создаем эффект с параметрами по умолчанию
        WaveEffect effect = new WaveEffect(
            defaultWaveSpeed,
            defaultWaveAmplitude,
            defaultWaveFrequency,
            defaultWaveOffset
        );

        // Убираем < и >
        tag = tag.Substring(1, tag.Length - 2);

        // Разделяем параметры
        string[] parts = tag.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < parts.Length; i++) // начинаем с 1, чтобы пропустить "wave"
        {
            string[] paramParts = parts[i].Split('=');
            if (paramParts.Length == 2)
            {
                string paramName = paramParts[0].ToLower();
                string paramValue = paramParts[1];

                // Пытаемся распарсить значение
                if (float.TryParse(paramValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                {
                    switch (paramName)
                    {
                        case "speed":
                            effect.speed = value;
                            break;
                        case "amp":
                        case "amplitude":
                            effect.amplitude = value;
                            break;
                        case "freq":
                        case "frequency":
                            effect.frequency = value;
                            break;
                        case "offset":
                            effect.offset = value;
                            break;
                        default:
                            Debug.LogWarning($"Неизвестный параметр wave эффекта: {paramName}");
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning($"Не удалось распарсить значение параметра {paramName}: {paramValue}");
                }
            }
        }

        return effect;
    }

    private ShakeEffect ParseShakeTag(string tag)
    {
        // Создаем эффект с параметрами по умолчанию
        ShakeEffect effect = new ShakeEffect(
            defaultShakeIntensity,
            defaultShakeSpeed,
            defaultShakeRandomness
        );

        // Убираем < и >
        tag = tag.Substring(1, tag.Length - 2);

        // Разделяем параметры
        string[] parts = tag.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < parts.Length; i++) // начинаем с 1, чтобы пропустить "shake"
        {
            string[] paramParts = parts[i].Split('=');
            if (paramParts.Length == 2)
            {
                string paramName = paramParts[0].ToLower();
                string paramValue = paramParts[1];

                if (float.TryParse(paramValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                {
                    switch (paramName)
                    {
                        case "intensity":
                            effect.intensity = value;
                            break;
                        case "speed":
                            effect.speed = value;
                            break;
                        case "random":
                        case "randomness":
                            effect.randomness = value;
                            break;
                        default:
                            Debug.LogWarning($"Неизвестный параметр shake эффекта: {paramName}");
                            break;
                    }
                }
                else
                {
                    Debug.LogWarning($"Не удалось распарсить значение параметра {paramName}: {paramValue}");
                }
            }
        }

        return effect;
    }

    private void Update()
    {
        if (textComponent == null || !isInitialized) return;

        textComponent.ForceMeshUpdate();
        mesh = textComponent.mesh;
        vertices = mesh.vertices;

        TMP_TextInfo textInfo = textComponent.textInfo;

        // Применяем wave эффект
        foreach (var kvp in effectData.waveEffects)
        {
            int charIndex = kvp.Key;
            WaveEffect effect = kvp.Value;

            if (charIndex >= textInfo.characterCount) continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;

            // Вычисляем смещение волны
            float offsetY = Mathf.Sin(Time.time * effect.speed +
                                     charIndex * effect.frequency + effect.offset) * effect.amplitude;

            // Применяем ко всем 4 вершинам символа
            for (int j = 0; j < 4; j++)
            {
                vertices[vertexIndex + j].y += offsetY;
            }
        }

        // Применяем shake эффект
        foreach (var kvp in effectData.shakeEffects)
        {
            int charIndex = kvp.Key;
            ShakeEffect effect = kvp.Value;

            if (charIndex >= textInfo.characterCount) continue;

            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
            if (!charInfo.isVisible) continue;

            int vertexIndex = charInfo.vertexIndex;

            // Вычисляем тряску (разная для каждой вершины)
            for (int j = 0; j < 4; j++)
            {
                float time = Time.time * effect.speed;
                float seed = (vertexIndex + j) * effect.randomness;

                Vector3 shakeOffset = new Vector3(
                    Mathf.PerlinNoise(seed, time) * 2 - 1,
                    Mathf.PerlinNoise(seed + 10, time) * 2 - 1,
                    0
                ) * effect.intensity;

                vertices[vertexIndex + j] += shakeOffset;
            }
        }

        // Обновляем меш
        mesh.vertices = vertices;
        textComponent.canvasRenderer.SetMesh(mesh);
    }

    // Метод для обновления текста (например, через локализацию)
    public void UpdateText(string newTextWithTags)
    {
        sourceTextWithTags = newTextWithTags;
        ParseTextFromSource();
    }

    // Метод для принудительного обновления текущего текста
    public void RefreshText()
    {
        ParseTextFromSource();
    }

    // Методы для работы с локализацией
    public void SetLocalizationKey(string key, string sheet = null)
    {
        useLocalization = true;
        localizationKey = key;
        if (!string.IsNullOrEmpty(sheet))
        {
            sheetName = sheet;
        }

        if (isInitialized && LocalizationManager.Instance != null)
        {
            UpdateTextFromLocalization();
        }
    }

    public void EnableLocalization(bool enable)
    {
        useLocalization = enable;
        if (enable && !string.IsNullOrEmpty(localizationKey) && isInitialized && LocalizationManager.Instance != null)
        {
            UpdateTextFromLocalization();
        }
        else if (!enable && isInitialized)
        {
            // Если отключаем локализацию, парсим текущий sourceTextWithTags
            ParseTextFromSource();
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        if (isSubscribed && LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            Debug.Log($"{gameObject.name}: Unsubscribed from language change");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        if (textComponent != null)
        {
            // Если sourceTextWithTags пустой, инициализируем из TMP_Text
            if (string.IsNullOrEmpty(sourceTextWithTags))
            {
                sourceTextWithTags = textComponent.text;
            }

            // В режиме редактирования нужно очистить только наши теги, оставив стандартные
            if (!Application.isPlaying)
            {
                string cleanText = CleanOnlyCustomTags(sourceTextWithTags);
                textComponent.text = cleanText;
            }
        }
    }

    // Очищает только наши кастомные теги, оставляя стандартные TMPro теги
    private string CleanOnlyCustomTags(string text)
    {
        // Удаляем только теги wave и shake, оставляя остальные
        string result = Regex.Replace(text, @"<\/?wave[^>]*>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<\/?shake[^>]*>", "", RegexOptions.IgnoreCase);
        return result;
    }
#endif
}