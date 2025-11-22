using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

        [Header("Canvas Settings")]
        [SerializeField] private int pauseMenuSortOrder = 10;
        [SerializeField] private int pauseButtonSortOrder = 5;

        [Header("Settings")]
        public string mainMenuSceneName = "MainMenu";

        private bool isPaused = false;
        private string currentSceneName;
        private Canvas pauseMenuCanvas;
        private Canvas pauseButtonCanvas;

        void Start()
        {
            SetupCanvases();

            currentSceneName = SceneManager.GetActiveScene().name;
            pauseMenuPanel.SetActive(false);

            continueButton.onClick.AddListener(ContinueGame);
            settingsButton.onClick.AddListener(OpenSettings);
            backToMenuButton.onClick.AddListener(BackToMainMenu);

            if (pauseButton != null)
            {
                pauseButton.onClick.AddListener(TogglePause);
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

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }

        private bool IsMainMenuScene()
        {
            return currentSceneName == mainMenuSceneName;
        }

        public void TogglePause()
        {
            if (IsMainMenuScene()) return;

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
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;

            if (pauseButton != null)
            {
                pauseButton.interactable = false;
            }
        }

        public void ContinueGame()
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;

            if (pauseButton != null)
            {
                pauseButton.interactable = true;
            }
        }

        private void OpenSettings()
        {
        }

        private void BackToMainMenu()
        {
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
            isPaused = false;
            Time.timeScale = 1f;

            if (pauseButton != null)
            {
                pauseButton.interactable = true;
            }
        }
    }
}