using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading Screen UI")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Image loadingSpinner;
    [SerializeField] private float spinnerRotationSpeed = 180f;

    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 0.5f;

    private static LoadingScreen instance;
    private AsyncOperation loadingOperation;
    private bool isLoading = false;

    public static LoadingScreen Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LoadingScreen>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("LoadingScreen");
                    instance = obj.AddComponent<LoadingScreen>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (loadingScreen != null)
                loadingScreen.SetActive(false);

            if (fadePanel != null)
            {
                fadePanel.alpha = 0f;
                fadePanel.blocksRaycasts = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isLoading && loadingSpinner != null)
        {
            loadingSpinner.transform.Rotate(0f, 0f, -spinnerRotationSpeed * Time.deltaTime);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
            StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoading = true;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        ShowLoadingScreen();

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration / 2f));

        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            if (loadingText != null)
                loadingText.text = $"Loading... {progress * 100:F0}%";

            if (loadingOperation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration / 2f));

        HideLoadingScreen();

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        isLoading = false;
    }

    private void ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            if (loadingText != null)
                loadingText.text = "Loading... 0%";
        }
    }

    private void HideLoadingScreen()
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
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
}