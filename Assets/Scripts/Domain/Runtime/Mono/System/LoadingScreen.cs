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
    [SerializeField] private float fadeDuration = 0.5f;

    private static LoadingScreen instance;
    private AsyncOperation loadingOperation;
    private bool isLoading = false;

    private FadeController fadeController;
    private CanvasGroup loadingCanvasGroup;

    public static LoadingScreen Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<LoadingScreen>();
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
            {
                loadingScreen.SetActive(false);

                // ��������� CanvasGroup ��� �������� ��������� loading screen
                loadingCanvasGroup = loadingScreen.GetComponent<CanvasGroup>();
                if (loadingCanvasGroup == null)
                {
                    loadingCanvasGroup = loadingScreen.AddComponent<CanvasGroup>();
                }
                loadingCanvasGroup.alpha = 0f;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        fadeController = FadeController.GetInstance();
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
    public bool GetIsLoading()
    {
        return isLoading;
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        isLoading = true;

        yield return StartCoroutine(ShowLoadingScreen());

        if (fadeController != null)
        {
            fadeController.SetFadeImmediate(1f);
        }

        if (fadeController != null)
        {
            yield return fadeController.FadeOutCoroutine();
        }

        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        float currentDisplayProgress = 0f;
        bool sceneActivated = false;

        while (!loadingOperation.isDone)
        {
            float realProgress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            currentDisplayProgress = Mathf.MoveTowards(currentDisplayProgress, realProgress, Time.deltaTime * 2f);

            if (loadingText != null)
                loadingText.text = $"Loading... {currentDisplayProgress * 100:F0}%";

            if (loadingOperation.progress >= 0.9f && !sceneActivated)
            {
                while (currentDisplayProgress < 1f)
                {
                    currentDisplayProgress = Mathf.MoveTowards(currentDisplayProgress, 1f, Time.deltaTime * 2f);
                    if (loadingText != null)
                        loadingText.text = $"Loading... {currentDisplayProgress * 100:F0}%";
                    yield return null;
                }

                yield return new WaitForSeconds(0.5f);
                loadingOperation.allowSceneActivation = true;
                sceneActivated = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        if (fadeController != null)
        {
            yield return fadeController.FadeInCoroutine();
        }

        yield return StartCoroutine(HideLoadingScreen());

        if (fadeController != null)
        {
            yield return fadeController.FadeOutCoroutine();
        }

        isLoading = false;
    }

    private IEnumerator ShowLoadingScreen()
    {
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            if (loadingText != null)
                loadingText.text = "Loading... 0%";

            if (loadingCanvasGroup != null)
            {
                float timer = 0f;
                loadingCanvasGroup.alpha = 0f;

                while (timer < fadeDuration)
                {
                    timer += Time.deltaTime;
                    loadingCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                    yield return null;
                }

                loadingCanvasGroup.alpha = 1f;
            }
        }
    }

    private IEnumerator HideLoadingScreen()
    {
        if (loadingScreen != null && loadingCanvasGroup != null)
        {
            float timer = 0f;
            loadingCanvasGroup.alpha = 1f;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                loadingCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                yield return null;
            }

            loadingCanvasGroup.alpha = 0f;
            loadingScreen.SetActive(false);
        }
        else if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
}
