using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject demoMessagePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Demo Message Settings")]
    [SerializeField] private float demoMessageDuration = 5f;
    [SerializeField] private TMP_Text skipTimerText;

    [Header("External Links")]
    [SerializeField] private string debugFormURL = "https://docs.google.com/forms/your-debug-form-link";
    [SerializeField] private string feedbackFormURL = "https://docs.google.com/forms/your-feedback-form-link";

    [Header("Transition Settings")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1f;

    private bool demoMessageActive = true;
    private bool canSkipDemoMessage = false;
    private float demoMessageTimer = 0f;
    private bool isTransitioning = false;

    private void Start()
    {
        InitializeUI();
        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        if (demoMessageActive && canSkipDemoMessage && Input.anyKeyDown && !isTransitioning)
        {
            SkipDemoMessage();
        }
    }

    private void InitializeUI()
    {
        if (fadePanel != null)
        {
            fadePanel.alpha = 1f;
            fadePanel.blocksRaycasts = true;
            fadePanel.gameObject.SetActive(true);
        }

        demoMessagePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(0.1f);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        demoMessageTimer = demoMessageDuration;

        while (demoMessageTimer > 0)
        {
            demoMessageTimer -= Time.deltaTime;
            skipTimerText.text = $"You can skip in {demoMessageTimer:F1} seconds";
            yield return null;
        }

        skipTimerText.text = "Press any button to continue";
        canSkipDemoMessage = true;

        float autoSkipTimer = 10f;
        while (autoSkipTimer > 0 && demoMessageActive && !Input.anyKeyDown)
        {
            autoSkipTimer -= Time.deltaTime;
            yield return null;
        }

        if (demoMessageActive && !isTransitioning)
        {
            SkipDemoMessage();
        }
    }

    private void SkipDemoMessage()
    {
        if (canSkipDemoMessage && !isTransitioning)
        {
            demoMessageActive = false;
            StartCoroutine(TransitionFromDemoToMainMenu());
        }
    }

    private IEnumerator TransitionFromDemoToMainMenu()
    {
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        demoMessagePanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isTransitioning = false;
    }

    private IEnumerator TransitionToCredits()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isTransitioning = false;
    }

    private IEnumerator TransitionFromCreditsToMainMenu()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isTransitioning = false;
    }

    private IEnumerator LoadGameScene()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        LoadingScreen.Instance.LoadScene("MapGeneration");

        isTransitioning = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        if (fadePanel == null) yield break;

        fadePanel.gameObject.SetActive(true);
        fadePanel.blocksRaycasts = true;

        float timer = 0f;
        fadePanel.alpha = startAlpha;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            yield return null;
        }

        fadePanel.alpha = endAlpha;

        if (endAlpha == 0f)
        {
            fadePanel.blocksRaycasts = false;
        }
    }


    public void OnPlayButtonClicked()
    {
        if (!isTransitioning)
            StartCoroutine(LoadGameScene());
    }

    public void OnDebugButtonClicked()
    {
        if (!isTransitioning)
            OpenURL(debugFormURL);
    }

    public void OnFeedbackButtonClicked()
    {
        if (!isTransitioning)
            OpenURL(feedbackFormURL);
    }

    public void OnSettingsButtonClicked()
    {
    }

    public void OnCreditsButtonClicked()
    {
        if (!isTransitioning)
            StartCoroutine(TransitionToCredits());
    }

    public void OnExitButtonClicked()
    {
        if (!isTransitioning)
        {
            Debug.Log("Exiting game...");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }


    public void OnBackButtonClicked()
    {
        if (!isTransitioning)
            StartCoroutine(TransitionFromCreditsToMainMenu());
    }


    private void OpenURL(string url)
    {
        if (string.IsNullOrEmpty(url)) return;

        try
        {
            Application.OpenURL(url);
            Debug.Log($"Opening URL: {url}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to open URL: {e.Message}");
        }
    }
}