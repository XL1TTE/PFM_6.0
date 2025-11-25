using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Domain.StateMachine.Mono;
using Domain.StateMachine.Components;

namespace Project
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject pauseMenuPanel;
        public Button continueButton;
        public Button settingsButton;
        public Button backToMenuButton;
        public Button pauseButton;

        [Header("Settings Panel")]
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Button settingsBackButton;

        [Header("Canvas Settings")]
        [SerializeField] private int pauseMenuSortOrder = 10;
        [SerializeField] private int pauseButtonSortOrder = 5;

        [Header("Settings")]
        public string mainMenuSceneName = "MainMenu";

        [Header("Sound Settings")]
        [SerializeField] private string buttonClickSound = "ButtonClick";

        private bool isPaused = false;
        private string currentSceneName;
        private Canvas pauseMenuCanvas;
        private Canvas pauseButtonCanvas;

        void Start()
        {
            SetupCanvases();

            currentSceneName = SceneManager.GetActiveScene().name;
            pauseMenuPanel.SetActive(false);

            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }

            // ��������� ������ ��������� ���� �����
            continueButton.onClick.AddListener(ContinueGame);
            settingsButton.onClick.AddListener(OpenSettings);
            continueButton.onClick.AddListener(NotifyGameUnpaused);


            backToMenuButton.onClick.AddListener(BackToMainMenu);

            // ��������� ������ ��������
            if (settingsBackButton != null)
            {
                settingsBackButton.onClick.AddListener(CloseSettings);
            }

            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TogglePause);
                pauseButton.onClick.AddListener(NotifyGamePaused);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            UpdatePauseButtonVisibility();
        }

        private void SetupCanvases()
        {
            pauseMenuCanvas = pauseMenuPanel.GetComponent<Canvas>();
            if (pauseMenuCanvas == null)
            {
                pauseMenuCanvas = pauseMenuPanel.AddComponent<Canvas>();
            }
            pauseMenuCanvas.overrideSorting = true;
            pauseMenuCanvas.sortingOrder = pauseMenuSortOrder;

            if (pauseMenuPanel.GetComponent<GraphicRaycaster>() == null)
            {
                pauseMenuPanel.AddComponent<GraphicRaycaster>();
            }

            if (pauseButton != null)
            {
                pauseButtonCanvas = pauseButton.GetComponent<Canvas>();
                if (pauseButtonCanvas == null)
                {
                    pauseButtonCanvas = pauseButton.gameObject.AddComponent<Canvas>();
                }
                pauseButtonCanvas.overrideSorting = true;
                pauseButtonCanvas.sortingOrder = pauseButtonSortOrder;

                if (pauseButton.GetComponent<GraphicRaycaster>() == null)
                {
                    pauseButton.gameObject.AddComponent<GraphicRaycaster>();
                }
            }
        }

        private void NotifyGamePaused()
        {
            SM.EnterState<PauseState>();
        }
        private void NotifyGameUnpaused()
        {
            SM.ExitState<PauseState>();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentSceneName = scene.name;
            ForceHideMenu();
            UpdatePauseButtonVisibility();
        }

        private void UpdatePauseButtonVisibility()
        {
            if (pauseButton != null)
            {
                bool shouldShow = !IsMainMenuScene();
                pauseButton.gameObject.SetActive(shouldShow);

                if (pauseButtonCanvas != null)
                {
                    pauseButtonCanvas.enabled = shouldShow;
                }
            }
        }

        void Update()
        {
            if (IsMainMenuScene()) return;
            if (IsLoadingScreenActive()) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // ���� ������� ��������� - ��������� ��, ����� �����
                if (settingsPanel != null && settingsPanel.activeInHierarchy)
                {
                    CloseSettings();
                }
                else
                {
                    TogglePause();
                }
            }
        }

        private bool IsMainMenuScene()
        {
            return currentSceneName == mainMenuSceneName;
        }

        private bool IsLoadingScreenActive()
        {
            LoadingScreen loadingScreen = LoadingScreen.Instance;
            return loadingScreen != null && loadingScreen.GetIsLoading();
        }

        public void TogglePause()
        {
            if (IsMainMenuScene()) return;
            if (IsLoadingScreenActive()) return;

            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            isPaused = !isPaused;

            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ContinueGame();
            }
        }

        private void PauseGame()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;

            if (pauseButton != null)
            {
                pauseButton.interactable = false;
            }
        }

        public void ContinueGame()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
            pauseMenuPanel.SetActive(false);
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
            Time.timeScale = 1f;
            isPaused = false;

            if (pauseButton != null)
            {
                pauseButton.interactable = true;
            }
        }

        private void OpenSettings()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
            }
        }

        private void CloseSettings()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
        }

        private void BackToMainMenu()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);
            Time.timeScale = 1f;
            isPaused = false;

            if (!string.IsNullOrEmpty(mainMenuSceneName))
            {
                LoadingScreen.Instance.LoadScene(mainMenuSceneName);
            }
            else
            {
                Debug.LogWarning("Main menu scene name is not set!");
            }
        }

        public void SetPaused(bool paused)
        {
            if (IsMainMenuScene()) return;
            if (IsLoadingScreenActive()) return;

            if (paused && !isPaused)
            {
                PauseGame();
            }
            else if (!paused && isPaused)
            {
                ContinueGame();
            }
        }

        public bool IsGamePaused
        {
            get
            {
                if (IsMainMenuScene()) return false;
                return isPaused;
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Time.timeScale = 1f;
        }

        public void ForceHideMenu()
        {
            pauseMenuPanel.SetActive(false);
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
            }
            isPaused = false;
            Time.timeScale = 1f;

            if (pauseButton != null)
            {
                pauseButton.interactable = true;
            }
        }
    }
}
