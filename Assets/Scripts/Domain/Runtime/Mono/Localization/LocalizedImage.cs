using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class LocalizedImage : MonoBehaviour
{
    [Header("Language Sprites")]
    [SerializeField] private Sprite englishSprite;
    [SerializeField] private Sprite russianSprite;

    [Header("Settings")]
    [SerializeField] private bool updateOnStart = true;
    [SerializeField] private bool updateOnLanguageChange = true;
    private bool setNativeSize = true;

    private Image imageComponent;
    private bool isSubscribed = false;

    private void Awake()
    {
        imageComponent = GetComponent<Image>();

        if (imageComponent == null)
        {
            Debug.LogWarning($"{gameObject.name}: No Image component found!");
            return;
        }

        if (englishSprite == null && imageComponent.sprite != null)
        {
            englishSprite = imageComponent.sprite;
        }

        if (updateOnLanguageChange)
        {
            SubscribeToLanguageChange();
        }
    }

    private void Start()
    {
        if (updateOnStart)
        {
            UpdateImage();
        }
    }

    private void SubscribeToLanguageChange()
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateImage;
            isSubscribed = true;
        }
        else
        {
            StartCoroutine(SubscribeWhenReady());
        }
    }

    private System.Collections.IEnumerator SubscribeWhenReady()
    {
        int maxAttempts = 60;
        int attempts = 0;

        while (LocalizationManager.Instance == null && attempts < maxAttempts)
        {
            attempts++;
            yield return null;
        }

        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.OnLanguageChanged += UpdateImage;
            isSubscribed = true;
        }
    }

    public void UpdateImage()
    {
        if (imageComponent == null)
            return;

        Sprite spriteToUse = GetSpriteForCurrentLanguage();

        if (spriteToUse != null)
        {
            imageComponent.sprite = spriteToUse;

            if (setNativeSize)
            {
                imageComponent.SetNativeSize();
            }
        }
    }

    private Sprite GetSpriteForCurrentLanguage()
    {
        Language currentLanguage = GetCurrentLanguage();

        switch (currentLanguage)
        {
            case Language.English:
                return englishSprite != null ? englishSprite : englishSprite;
            case Language.Russian:
                return russianSprite != null ? russianSprite : englishSprite;
            default:
                return englishSprite;
        }
    }

    private Language GetCurrentLanguage()
    {
        string savedLang = PlayerPrefs.GetString("SelectedLanguage", Language.English.ToString());
        if (System.Enum.TryParse(savedLang, out Language savedLanguage))
        {
            return savedLanguage;
        }

        return Language.English;
    }

    public void SetSprites(Sprite english, Sprite russian, Sprite fallback = null)
    {
        englishSprite = english;
        russianSprite = russian;

        if (fallback != null)
        {
            englishSprite = fallback;
        }

        UpdateImage();
    }

    private void OnEnable()
    {
        if (imageComponent != null)
        {
            UpdateImage();
        }
    }

    private void OnDestroy()
    {
        if (LocalizationManager.Instance != null && isSubscribed)
        {
            LocalizationManager.Instance.OnLanguageChanged -= UpdateImage;
        }
    }
}