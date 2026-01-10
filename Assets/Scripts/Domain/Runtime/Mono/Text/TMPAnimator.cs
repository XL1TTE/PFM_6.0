using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using System.Text;

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

    // Настройки для эффекта capitals
    [Header("Capitals Effect Settings")]
    private string capitalsFontName = "MedievalDropCapFont";
    [SerializeField] private float capitalsFontSizeMultiplier = 1.5f;

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
        // capitalsEffects больше не нужен, так как мы меняем текст напрямую
    }

    [System.Serializable]
    public class WaveEffect
    {
        public float speed;
        public float amplitude;
        public float frequency;
        public float offset;

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

        if (string.IsNullOrEmpty(sourceTextWithTags))
        {
            sourceTextWithTags = textComponent.text;
        }

        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return StartCoroutine(WaitForLocalizationManager());

        if (useLocalization && !string.IsNullOrEmpty(localizationKey))
        {
            isLocalized = true;

            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
                isSubscribed = true;
            }

            yield return StartCoroutine(WaitForLocalizationData());
            UpdateTextFromLocalization();
        }
        else
        {
            ParseTextFromSource();
        }

        isInitialized = true;
    }

    private IEnumerator WaitForLocalizationManager()
    {
        int maxWaitFrames = 120;
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

        int maxWaitFrames = 120;
        int frameCount = 0;

        while (frameCount < maxWaitFrames)
        {
            if (LocalizationManager.Instance != null)
            {
                yield return new WaitForSeconds(0.1f);
                break;
            }
            frameCount++;
            yield return null;
        }
    }

    private void Start()
    {
        if (!isInitialized)
        {
            StartCoroutine(Initialize());
        }
    }

    private void OnLanguageChanged()
    {
        if (useLocalization && !string.IsNullOrEmpty(localizationKey) && isInitialized)
        {
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

        string localizedText = LocalizationManager.Instance.GetLocalizedValue(localizationKey, sheetName);

        if (!string.IsNullOrEmpty(localizedText) && localizedText != $"[{localizationKey}]")
        {
            sourceTextWithTags = localizedText;
            ParseTextFromSource();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: Failed to get localized text for key: {localizationKey}");
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
        Stack<bool> capitalsStack = new Stack<bool>();

        int parsedIndex = 0;
        int i = 0;

        while (i < originalText.Length)
        {
            if (originalText[i] == '<')
            {
                int endTag = originalText.IndexOf('>', i);
                if (endTag != -1)
                {
                    string fullTag = originalText.Substring(i, endTag - i + 1);
                    string tagContent = originalText.Substring(i + 1, endTag - i - 1).ToLower();

                    if (tagContent.StartsWith("wave"))
                    {
                        WaveEffect effect = ParseWaveTag(fullTag);
                        waveStack.Push(effect);
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent.StartsWith("shake"))
                    {
                        ShakeEffect effect = ParseShakeTag(fullTag);
                        shakeStack.Push(effect);
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent.StartsWith("capitals"))
                    {
                        capitalsStack.Push(true);
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent == "/wave")
                    {
                        if (waveStack.Count > 0) waveStack.Pop();
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent == "/shake")
                    {
                        if (shakeStack.Count > 0) shakeStack.Pop();
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent == "/capitals")
                    {
                        if (capitalsStack.Count > 0) capitalsStack.Pop();
                        i = endTag + 1;
                        continue;
                    }
                    else if (IsStandardTmProTag(tagContent))
                    {
                        parsedText += fullTag;
                        i = endTag + 1;
                        continue;
                    }
                }
            }

            // Обрабатываем capitals эффект - вставляем тег <font> для заглавных букв
            if (capitalsStack.Count > 0)
            {
                char currentChar = originalText[i];
                if (char.IsLetter(currentChar) && char.IsUpper(currentChar))
                {
                    // Вставляем тег для изменения шрифта заглавной буквы
                    parsedText += $"<font=\"{capitalsFontName}\">";
                    parsedText += currentChar;
                    parsedText += "</font>";
                }
                else
                {
                    parsedText += originalText[i];
                }
            }
            else
            {
                parsedText += originalText[i];
            }

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

        textComponent.text = parsedText;
        textComponent.ForceMeshUpdate();
    }

    // Альтернативный метод, который обрабатывает целые слова
    public void ParseTextFromSourceWithWordProcessing()
    {
        if (textComponent == null) return;

        string originalText = sourceTextWithTags;
        StringBuilder parsedText = new StringBuilder();

        effectData.waveEffects.Clear();
        effectData.shakeEffects.Clear();

        Stack<WaveEffect> waveStack = new Stack<WaveEffect>();
        Stack<ShakeEffect> shakeStack = new Stack<ShakeEffect>();
        Stack<bool> capitalsStack = new Stack<bool>();

        int parsedIndex = 0;
        int i = 0;

        while (i < originalText.Length)
        {
            if (originalText[i] == '<')
            {
                int endTag = originalText.IndexOf('>', i);
                if (endTag != -1)
                {
                    string fullTag = originalText.Substring(i, endTag - i + 1);
                    string tagContent = originalText.Substring(i + 1, endTag - i - 1).ToLower();

                    if (tagContent.StartsWith("wave"))
                    {
                        WaveEffect effect = ParseWaveTag(fullTag);
                        waveStack.Push(effect);
                        i = endTag + 1;
                        parsedText.Append(fullTag);
                        continue;
                    }
                    else if (tagContent.StartsWith("shake"))
                    {
                        ShakeEffect effect = ParseShakeTag(fullTag);
                        shakeStack.Push(effect);
                        i = endTag + 1;
                        parsedText.Append(fullTag);
                        continue;
                    }
                    else if (tagContent.StartsWith("capitals"))
                    {
                        capitalsStack.Push(true);
                        i = endTag + 1;
                        continue;
                    }
                    else if (tagContent == "/wave")
                    {
                        if (waveStack.Count > 0) waveStack.Pop();
                        i = endTag + 1;
                        parsedText.Append(fullTag);
                        continue;
                    }
                    else if (tagContent == "/shake")
                    {
                        if (shakeStack.Count > 0) shakeStack.Pop();
                        i = endTag + 1;
                        parsedText.Append(fullTag);
                        continue;
                    }
                    else if (tagContent == "/capitals")
                    {
                        if (capitalsStack.Count > 0) capitalsStack.Pop();
                        i = endTag + 1;
                        continue;
                    }
                    else if (IsStandardTmProTag(tagContent))
                    {
                        parsedText.Append(fullTag);
                        i = endTag + 1;
                        continue;
                    }
                }
            }

            // Если внутри тега <capitals>, обрабатываем текст по-особому
            if (capitalsStack.Count > 0)
            {
                // Собираем слово до пробела или конца текста
                StringBuilder wordBuilder = new StringBuilder();
                int wordStart = i;

                while (i < originalText.Length && originalText[i] != ' ' && originalText[i] != '<')
                {
                    wordBuilder.Append(originalText[i]);
                    i++;
                }

                string word = wordBuilder.ToString();

                // Обрабатываем слово: первую заглавную букву оформляем особым шрифтом
                if (word.Length > 0)
                {
                    bool hasCapital = false;
                    for (int j = 0; j < word.Length; j++)
                    {
                        if (char.IsLetter(word[j]) && char.IsUpper(word[j]))
                        {
                            parsedText.Append($"<font=\"{capitalsFontName}\">");
                            parsedText.Append(word[j]);
                            parsedText.Append("</font>");
                            hasCapital = true;
                        }
                        else
                        {
                            parsedText.Append(word[j]);
                        }

                        // Применяем другие эффекты к этому символу
                        if (waveStack.Count > 0)
                        {
                            effectData.waveEffects[parsedIndex] = waveStack.Peek();
                        }
                        if (shakeStack.Count > 0)
                        {
                            effectData.shakeEffects[parsedIndex] = shakeStack.Peek();
                        }
                        parsedIndex++;
                    }

                    // Добавляем пробел, если он был
                    if (i < originalText.Length && originalText[i] == ' ')
                    {
                        parsedText.Append(' ');
                        i++;
                    }
                }
            }
            else
            {
                // Вне тега <capitals> - просто добавляем символ
                parsedText.Append(originalText[i]);

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
        }

        textComponent.text = parsedText.ToString();
        textComponent.ForceMeshUpdate();
    }

    private bool IsStandardTmProTag(string tagContent)
    {
        string cleanTag = tagContent.StartsWith("/") ? tagContent.Substring(1) : tagContent;

        if (standardTmProTags.Contains(cleanTag))
            return true;

        foreach (var tag in standardTmProTags)
        {
            if (cleanTag.StartsWith(tag))
                return true;
        }

        return false;
    }

    private WaveEffect ParseWaveTag(string tag)
    {
        WaveEffect effect = new WaveEffect(
            defaultWaveSpeed,
            defaultWaveAmplitude,
            defaultWaveFrequency,
            defaultWaveOffset
        );

        tag = tag.Substring(1, tag.Length - 2);
        string[] parts = tag.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < parts.Length; i++)
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
                    }
                }
            }
        }

        return effect;
    }

    private ShakeEffect ParseShakeTag(string tag)
    {
        ShakeEffect effect = new ShakeEffect(
            defaultShakeIntensity,
            defaultShakeSpeed,
            defaultShakeRandomness
        );

        tag = tag.Substring(1, tag.Length - 2);
        string[] parts = tag.Split(new char[] { ' ', '\t' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 1; i < parts.Length; i++)
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
                    }
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

            float offsetY = Mathf.Sin(Time.time * effect.speed +
                                     charIndex * effect.frequency + effect.offset) * effect.amplitude;

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

        mesh.vertices = vertices;
        textComponent.canvasRenderer.SetMesh(mesh);
    }

    public void UpdateText(string newTextWithTags)
    {
        sourceTextWithTags = newTextWithTags;
        ParseTextFromSource();
    }

    public void RefreshText()
    {
        ParseTextFromSource();
    }

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
            ParseTextFromSource();
        }
    }

    public void SetCapitalsFontName(string fontName)
    {
        capitalsFontName = fontName;
    }

    private void OnDestroy()
    {
        if (isSubscribed && LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (textComponent == null)
            textComponent = GetComponent<TMP_Text>();

        if (textComponent != null)
        {
            if (string.IsNullOrEmpty(sourceTextWithTags))
            {
                sourceTextWithTags = textComponent.text;
            }

            if (!Application.isPlaying)
            {
                string cleanText = CleanOnlyCustomTags(sourceTextWithTags);
                textComponent.text = cleanText;
            }
        }
    }

    private string CleanOnlyCustomTags(string text)
    {
        string result = Regex.Replace(text, @"<\/?wave[^>]*>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<\/?shake[^>]*>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<\/?capitals[^>]*>", "", RegexOptions.IgnoreCase);
        return result;
    }
#endif
}