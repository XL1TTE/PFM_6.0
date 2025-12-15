using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class LocalizedText : MonoBehaviour
{
    [Header("Localization Settings")]
    public string localizationKey;
    public string sheetName = "UI_Menu";
    public bool updateOnAwake = true;
    public bool updateOnLanguageChange = true;

    [Header("Format Arguments")]
    [SerializeField] private string[] formatArgs; // Для форматированных строк

    private TextMeshProUGUI tmpText;
    private TMP_Text legacyTMPText;
    private Text uiText;

    private bool isSubscribed = false;
    private bool isRegistered = false;

    private void Awake()
    {
        Debug.Log($"Я ПРОСНУЛСЯ. ВОТ Я {localizationKey}");
        tmpText = GetComponent<TextMeshProUGUI>();
        legacyTMPText = GetComponent<TMP_Text>();
        uiText = GetComponent<Text>();

        if (tmpText == null && legacyTMPText == null && uiText == null)
        {
            Debug.LogError($"{gameObject.name}: No text component found! Need TextMeshProUGUI, TMP_Text or UI.Text");
            return;
        }

        // Всегда пытаемся зарегистрироваться немедленно
        TryRegisterImmediately();

        if (updateOnAwake)
        {
            StartCoroutine(UpdateTextWhenReady());
        }

        if (updateOnLanguageChange)
        {
            StartCoroutine(SubscribeToLanguageChange());
        }
    }

    private void TryRegisterImmediately()
    {
        if (LocalizationManager.Instance != null && !isRegistered)
        {
            LocalizationManager.RegisterLocalizedText(this);
            isRegistered = true;
            Debug.Log($"ЗАРЕГАЛСЯ НЕМЕДЛЕННО {localizationKey}");
        }
        else if (!isRegistered)
        {
            // Если менеджер еще не создан, запускаем корутину
            StartCoroutine(RegisterWhenReady());
        }
    }

    private IEnumerator RegisterWhenReady()
    {
        // Ждем максимум 60 кадров или до появления менеджера
        int maxFrames = 60;
        int frameCount = 0;

        while (LocalizationManager.Instance == null && frameCount < maxFrames)
        {
            frameCount++;
            yield return null;
        }

        if (LocalizationManager.Instance != null && !isRegistered)
        {
            LocalizationManager.RegisterLocalizedText(this);
            isRegistered = true;
            Debug.Log($"ЗАРЕГАЛСЯ ПОСЛЕ ОЖИДАНИЯ {localizationKey}");
        }
        else if (!isRegistered)
        {
            Debug.LogWarning($"{gameObject.name}: Не удалось зарегистрироваться после ожидания");
        }
    }

    private IEnumerator UpdateTextWhenReady()
    {
        // Ждем, пока LocalizationManager будет готов И локализация загружена
        yield return new WaitUntil(() => LocalizationManager.Instance != null);

        // Дополнительная проверка, что локализация загружена
        int maxWaitFrames = 120; // Ждем максимум 2 секунды (60 FPS * 2)
        int frameCount = 0;

        while (frameCount < maxWaitFrames)
        {
            if (LocalizationManager.Instance != null)
            {
                UpdateText();
                break;
            }
            frameCount++;
            yield return null;
        }
    }

    private IEnumerator SubscribeToLanguageChange()
    {
        // Ждем, пока LocalizationManager будет готов
        yield return new WaitUntil(() => LocalizationManager.Instance != null);

        // Подписываемся на событие
        LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        isSubscribed = true;

        Debug.Log($"{gameObject.name}: Subscribed to language change");
    }

    private void Start()
    {
        // Дополнительная попытка регистрации на старте
        if (!isRegistered)
        {
            TryRegisterImmediately();
        }

        if (!updateOnAwake)
        {
            StartCoroutine(UpdateTextWhenReady());
        }
    }

    public void UpdateText()
    {
        Debug.Log($"НАЧАЛ ОБНОВЛЕНИЕ {localizationKey}");

        if (LocalizationManager.Instance == null)
        {
            Debug.LogWarning($"{gameObject.name}: LocalizationManager is null");
            return;
        }

        if (string.IsNullOrEmpty(localizationKey))
        {
            Debug.LogWarning($"{gameObject.name}: Localization key is empty!");
            return;
        }

        string localizedText;

        if (formatArgs != null && formatArgs.Length > 0)
        {
            object[] args = new object[formatArgs.Length];
            for (int i = 0; i < formatArgs.Length; i++)
            {
                args[i] = LocalizationManager.Instance.GetLocalizedValue(formatArgs[i], sheetName);
            }
            localizedText = LocalizationManager.Instance.GetFormattedValue(localizationKey, args);
        }
        else
        {
            localizedText = LocalizationManager.Instance.GetLocalizedValue(localizationKey, sheetName);
        }

        // Применяем текст к найденному компоненту
        if (tmpText != null)
        {
            tmpText.text = localizedText;
        }
        else if (legacyTMPText != null)
        {
            legacyTMPText.text = localizedText;
        }
        else if (uiText != null)
        {
            uiText.text = localizedText;
        }

        Debug.Log($"ОБНОВИЛСЯ {localizationKey}");
        Debug.Log($"{gameObject.name}: Updated to '{localizedText}'");
    }

    public void SetKey(string newKey, string newSheetName = null)
    {
        localizationKey = newKey;
        if (!string.IsNullOrEmpty(newSheetName))
        {
            sheetName = newSheetName;
        }
        UpdateText();
    }

    public void SetFormatArgs(params string[] args)
    {
        formatArgs = args;
        UpdateText();
    }

    private void OnEnable()
    {
        // При повторной активации объекта проверяем регистрацию
        if (!isRegistered)
        {
            TryRegisterImmediately();
        }

        // Обновляем текст при активации
        if (LocalizationManager.Instance != null)
        {
            UpdateText();
        }
    }

    private void OnDestroy()
    {
        // ОТПИСЫВАЕМСЯ и удаляем из менеджера
        if (LocalizationManager.Instance != null)
        {
            if (isSubscribed)
            {
                LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
            }
            LocalizationManager.UnregisterLocalizedText(this);
        }
    }

    // Метод для принудительной проверки
    public void DebugInfo()
    {
        Debug.Log($"{gameObject.name}:");
        Debug.Log($"  Key: {localizationKey}");
        Debug.Log($"  Sheet: {sheetName}");
        Debug.Log($"  UpdateOnAwake: {updateOnAwake}");
        Debug.Log($"  UpdateOnLanguageChange: {updateOnLanguageChange}");
        Debug.Log($"  IsSubscribed: {isSubscribed}");
        Debug.Log($"  IsRegistered: {isRegistered}");
        Debug.Log($"  LocalizationManager exists: {LocalizationManager.Instance != null}");

        if (LocalizationManager.Instance != null)
        {
            string currentValue = LocalizationManager.Instance.GetLocalizedValue(localizationKey, sheetName);
            Debug.Log($"  Current value: {currentValue}");
        }
    }
}