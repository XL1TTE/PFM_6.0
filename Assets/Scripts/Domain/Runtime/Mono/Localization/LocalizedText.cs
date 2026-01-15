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
    [SerializeField] private string[] formatArgs;

    private TextMeshProUGUI tmpText;
    private TMP_Text legacyTMPText;
    private Text uiText;

    private bool isSubscribed = false;
    private bool isRegistered = false;

    private void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        legacyTMPText = GetComponent<TMP_Text>();
        uiText = GetComponent<Text>();


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
        }
        else if (!isRegistered)
        {
            StartCoroutine(RegisterWhenReady());
        }
    }

    private IEnumerator RegisterWhenReady()
    {
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
        }
    }

    private IEnumerator UpdateTextWhenReady()
    {
        yield return new WaitUntil(() => LocalizationManager.Instance != null);

        int maxWaitFrames = 120;
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
        yield return new WaitUntil(() => LocalizationManager.Instance != null);

        LocalizationManager.Instance.OnLanguageChanged += UpdateText;
        isSubscribed = true;
    }

    private void Start()
    {
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
        if (LocalizationManager.Instance == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(localizationKey))
        {
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
        if (!isRegistered)
        {
            TryRegisterImmediately();
        }

        if (LocalizationManager.Instance != null)
        {
            UpdateText();
        }
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null)
        {
            if (isSubscribed)
            {
                LocalizationManager.Instance.OnLanguageChanged -= UpdateText;
            }
            LocalizationManager.UnregisterLocalizedText(this);
        }
    }
}