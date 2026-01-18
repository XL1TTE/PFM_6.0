using Persistence.DS;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GameObject displayObject; // Объект, который будет скрываться/показываться
    [SerializeField] private float updateInterval = 0.5f;
    [SerializeField] private int maxDisplayValue = 999999; // Максимальное отображаемое значение
    [SerializeField] private string mainMenuSceneName = "MainMenu"; // Название сцены главного меню

    private Inventory inventoryReference;
    private ResourcesStorage bodyResourcesStorage;
    private Coroutine updateCoroutine;
    private bool isInMainMenu = false;

    private void Start()
    {
        // Проверяем наличие текстового компонента
        if (goldText == null)
        {
            goldText = GetComponent<TMP_Text>();

            if (goldText == null)
            {
                Debug.LogError("GoldDisplay: Не найден Text компонент!");
                return;
            }
        }

        // Если displayObject не назначен, используем текущий объект
        if (displayObject == null)
        {
            displayObject = gameObject;
        }

        // Проверяем, находимся ли мы в главном меню
        CheckScene();

        // Подписываемся на событие загрузки сцены
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        // Начинаем периодическое обновление
        updateCoroutine = StartCoroutine(UpdateGoldRoutine());
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        CheckScene();
    }

    private void CheckScene()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        // Проверяем, является ли текущая сцена главным меню
        isInMainMenu = sceneName == mainMenuSceneName || sceneName.Contains("Menu");

        UpdateDisplayVisibility();
    }

    private IEnumerator UpdateGoldRoutine()
    {
        while (true)
        {
            UpdateGoldText();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    private void UpdateGoldText()
    {
        try
        {
            bodyResourcesStorage = DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>();

            int displayGold = bodyResourcesStorage.gold;
            if (displayGold > maxDisplayValue)
            {
                displayGold = maxDisplayValue;
            }

            goldText.text = $"{displayGold}";
        }
        catch (System.Exception e)
        {
            Debug.LogError($"GoldDisplay: Ошибка при обновлении золота: {e.Message}");
            goldText.text = "Error";
        }
    }

    // Метод для обновления видимости объекта
    private void UpdateDisplayVisibility()
    {
        if (displayObject != null)
        {
            // Если мы в главном меню - скрываем, иначе показываем
            displayObject.SetActive(!isInMainMenu);
        }
    }

    // Метод для принудительного обновления статуса
    public void ForceUpdateDisplay()
    {
        CheckScene();
        UpdateGoldText();
    }

    // Метод для обновления статуса главного меню из других скриптов
    public void SetMainMenuStatus(bool inMainMenu)
    {
        isInMainMenu = inMainMenu;
        UpdateDisplayVisibility();
    }

    // Публичный метод для принудительного обновления денег
    public void ForceUpdateGold()
    {
        UpdateGoldText();
    }

    // Метод для получения текущего количества золота (с ограничением)
    public int GetDisplayGold()
    {
        try
        {
            bodyResourcesStorage = DataStorage.GetRecordFromFile<Inventory, ResourcesStorage>();

            int gold = bodyResourcesStorage.gold;
            return gold > maxDisplayValue ? maxDisplayValue : gold;
        }
        catch
        {
            return 0;
        }
    }

    // Метод для проверки, находимся ли в главном меню
    public bool IsInMainMenu()
    {
        return isInMainMenu;
    }

    private void OnDestroy()
    {
        // Останавливаем корутину при уничтожении объекта
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
        }

        // Отписываемся от события
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Опционально: Если нужно реагировать на изменения паузы
    private void OnApplicationPause(bool pauseStatus)
    {
        // Можно добавить логику при паузе/возобновлении игры
    }
}