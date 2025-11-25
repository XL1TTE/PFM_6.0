using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicController : MonoBehaviour
{
    [Header("Scene Music Tracks")]
    [SerializeField] private string mainMenuMusic = "MainMenu";
    [SerializeField] private string laboratoryMusic = "Laboratory";
    [SerializeField] private string expeditionMusic = "Expedition";
    [SerializeField] private string battleMusic = "Battle";

    [Header("Music Behavior Settings")]
    [SerializeField] private float musicFadeDuration = 2f;

    private string currentSceneType;
    private string previousSceneType;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DetermineSceneType(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousSceneType = currentSceneType;
        DetermineSceneType(scene.name);
    }

    private void DetermineSceneType(string sceneName)
    {
        if (sceneName.Contains("MainMenu"))
        {
            SetMainMenuMusic();
        }
        else if (sceneName.Contains("Laboratory"))
        {
            SetLaboratoryMusic();
        }
        else if (sceneName.Contains("Expedition") || sceneName.Contains("Map"))
        {
            SetExpeditionMusic();
        }
        else if (sceneName.Contains("Battle") || sceneName.Contains("Combat"))
        {
            SetBattleMusic();
        }
        else
        {
            Debug.LogWarning($"Unknown scene type: {sceneName}");
        }
    }

    private void SetMainMenuMusic()
    {
        if (currentSceneType == "MainMenu") return;

        Debug.Log("Переход на музыку главного меню");
        AudioManager.Instance?.PlayMusic(mainMenuMusic, false, false, false);
        currentSceneType = "MainMenu";
    }

    private void SetLaboratoryMusic()
    {
        if (currentSceneType == "Laboratory") return;

        Debug.Log("Переход на музыку лаборатории");
        AudioManager.Instance?.PlayMusic(laboratoryMusic, false, false, false);
        currentSceneType = "Laboratory";
    }

    private void SetExpeditionMusic()
    {
        if (currentSceneType == "Expedition") return;

        Debug.Log("Переход на музыку похода");
        bool resumeFromLastPosition = true;
        AudioManager.Instance?.PlayMusic(expeditionMusic, false, resumeFromLastPosition, false);
        currentSceneType = "Expedition";
    }

    private void SetBattleMusic()
    {
        if (currentSceneType == "Battle") return;

        Debug.Log("Переход на музыку сражения");
        AudioManager.Instance?.PlayMusic(battleMusic, false, false, false);
        currentSceneType = "Battle";
    }

    private bool IsSceneLoading()
    {
        return LoadingScreen.Instance != null && LoadingScreen.Instance.GetIsLoading();
    }

    public void RestartCurrentMusic()
    {
        switch (currentSceneType)
        {
            case "MainMenu":
                AudioManager.Instance?.PlayMusic(mainMenuMusic, true, false);
                break;
            case "Laboratory":
                AudioManager.Instance?.PlayMusic(laboratoryMusic, true, false);
                break;
            case "Expedition":
                AudioManager.Instance?.PlayMusic(expeditionMusic, true, false);
                break;
            case "Battle":
                AudioManager.Instance?.PlayMusic(battleMusic, true, false);
                break;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public string GetCurrentSceneType() => currentSceneType;
    public string GetPreviousSceneType() => previousSceneType;
}