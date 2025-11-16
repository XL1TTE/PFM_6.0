using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private GameObject fadePanelPrefab;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private bool destroyAfterFade = false;

    private CanvasGroup fadePanel;
    private GameObject fadePanelInstance;
    private bool isFading = false;
    private static FadeController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CreateFadePanel()
    {
        if (fadePanelInstance == null && fadePanelPrefab != null)
        {
            fadePanelInstance = Instantiate(fadePanelPrefab);
            fadePanelInstance.name = "FadePanel";

            Canvas mainCanvas = FindObjectOfType<Canvas>();
            if (mainCanvas == null)
            {
                Debug.LogError("No Canvas found in scene!");
                return;
            }

            fadePanelInstance = Instantiate(fadePanelPrefab, mainCanvas.transform);
            fadePanelInstance.name = "FadePanel";

            fadePanel = fadePanelInstance.GetComponent<CanvasGroup>();

            if (fadePanel == null)
            {
                Debug.LogError("FadePanel prefab doesn't have CanvasGroup component!");
                Destroy(fadePanelInstance);
                return;
            }

            RectTransform rectTransform = fadePanelInstance.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.one;
                rectTransform.offsetMin = Vector2.zero;
                rectTransform.offsetMax = Vector2.zero;
            }

            fadePanel.alpha = 0f;
            fadePanel.blocksRaycasts = false;
            fadePanelInstance.SetActive(false);
        }
    }

    private void DestroyFadePanel()
    {
        if (fadePanelInstance != null)
        {
            Destroy(fadePanelInstance);
            fadePanelInstance = null;
            fadePanel = null;
        }
    }

    public void FadeIn(System.Action onComplete = null)
    {
        if (!isFading)
            StartCoroutine(FadeRoutine(0f, 1f, onComplete));
    }

    public void FadeOut(System.Action onComplete = null)
    {
        if (!isFading)
            StartCoroutine(FadeRoutine(1f, 0f, onComplete));
    }

    public IEnumerator FadeInCoroutine()
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f));
    }

    public IEnumerator FadeOutCoroutine()
    {
        yield return StartCoroutine(FadeRoutine(1f, 0f));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, System.Action onComplete = null)
    {
        if (isFading) yield break;

        isFading = true;

        CreateFadePanel();

        if (fadePanel == null)
        {
            Debug.LogError("FadePanel not available!");
            isFading = false;
            yield break;
        }

        fadePanelInstance.SetActive(true);
        fadePanel.blocksRaycasts = true;

        float timer = 0f;
        fadePanel.alpha = startAlpha;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }

        fadePanel.alpha = endAlpha;

        if (endAlpha == 0f)
        {
            fadePanel.blocksRaycasts = false;
            if (destroyAfterFade)
            {
                fadePanelInstance.SetActive(false);
            }
        }

        isFading = false;
        onComplete?.Invoke();

        if (destroyAfterFade && endAlpha == 0f)
        {
            DestroyFadePanel();
        }
    }

    public void FadeInOut(System.Action onMiddle = null, System.Action onComplete = null)
    {
        if (!isFading)
            StartCoroutine(FadeInOutRoutine(onMiddle, onComplete));
    }

    private IEnumerator FadeInOutRoutine(System.Action onMiddle = null, System.Action onComplete = null)
    {
        yield return StartCoroutine(FadeRoutine(0f, 1f, null));

        onMiddle?.Invoke();

        yield return StartCoroutine(FadeRoutine(1f, 0f, onComplete));
    }

    public bool IsFading()
    {
        return isFading;
    }

    public void SetFadeImmediate(float alpha)
    {
        CreateFadePanel();

        if (fadePanel != null)
        {
            fadePanel.alpha = alpha;
            fadePanel.blocksRaycasts = alpha > 0f;
            fadePanelInstance.SetActive(true);
        }
    }

    public void Cleanup()
    {
        DestroyFadePanel();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            Cleanup();
        }
    }

    public static FadeController GetInstance()
    {
        return instance;
    }
}