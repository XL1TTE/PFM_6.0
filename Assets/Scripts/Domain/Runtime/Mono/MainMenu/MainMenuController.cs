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
    [SerializeField] private GameObject settingsPanel;

    [Header("Demo Message Settings")]
    [SerializeField] private float demoMessageDuration = 5f;
    [SerializeField] private TMP_Text skipTimerText;

    [Header("External Links")]
    [SerializeField] private string debugFormURL = "https://docs.google.com/forms/your-debug-form-link";
    [SerializeField] private string feedbackFormURL = "https://docs.google.com/forms/your-feedback-form-link";

    private bool demoMessageActive = true;
    private bool canSkipDemoMessage = false;
    private float demoMessageTimer = 0f;
    private bool isTransitioning = false;

    private FadeController fadeController;

    private void Start()
    {
        fadeController = FadeController.GetInstance();

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
        if (fadeController != null)
        {
            fadeController.SetFadeImmediate(1f);
        }

        demoMessagePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(0.1f);

        if (fadeController != null)
        {
            yield return fadeController.FadeOutCoroutine();
        }

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

        if (fadeController != null)
        {
            yield return fadeController.FadeInCoroutine();
        }

        demoMessagePanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        if (fadeController != null)
        {
            yield return fadeController.FadeOutCoroutine();
        }

        isTransitioning = false;
    }

    private IEnumerator TransitionToSettings()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeInCoroutine();
        //}

        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeOutCoroutine();
        //}

        isTransitioning = false;
    }

    private IEnumerator TransitionFromSettingsToMainMenu()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeInCoroutine();
        //}

        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeOutCoroutine();
        //}

        isTransitioning = false;
    }

    private IEnumerator TransitionToCredits()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeInCoroutine();
        //}

        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeOutCoroutine();
        //}

        isTransitioning = false;
    }

    private IEnumerator TransitionFromCreditsToMainMenu()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeInCoroutine();
        //}

        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        //if (fadeController != null)
        //{
        //    yield return fadeController.FadeOutCoroutine();
        //}

        isTransitioning = false;
    }

    private IEnumerator LoadGameScene()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;

        LoadingScreen.Instance.LoadScene("Laboratory");

        isTransitioning = false;
    }

    public void OnPlayButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            StartCoroutine(LoadGameScene());
    }

    public void OnDebugButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            OpenURL(debugFormURL);
    }

    public void OnFeedbackButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            OpenURL(feedbackFormURL);
    }

    public void OnSettingsButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
        if (!isTransitioning)
            StartCoroutine(TransitionToSettings());
    }

    public void OnCreditsButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            StartCoroutine(TransitionToCredits());
    }

    public void OnExitButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
        {
            if (fadeController != null)
            {
                Debug.Log("Exiting game...");
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }

    public void OnBackButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            StartCoroutine(TransitionFromCreditsToMainMenu());
    }
    public void OnBackSettingsButtonClicked()
    {
        AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

        if (!isTransitioning)
            StartCoroutine(TransitionFromSettingsToMainMenu());
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