using Domain.Map;
using Persistence.DS;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project
{
    public enum CounterType
    {
        TotalMonsters,
        ExpeditionMonsters
    }

    [System.Serializable]
    public class CounterEntry
    {
        public CounterType counterType;
        public TextMeshProUGUI counterText;
        [Tooltip("Максимальное количество (только для ExpeditionMonsters)")]
        public int maxCount = 3;
    }

    public class LabMonstersCounter : MonoBehaviour
    {
        [Header("Counter Settings")]
        [SerializeField] private List<CounterEntry> counters = new List<CounterEntry>();

        [Header("Localization Keys")]
        [SerializeField] private string monstersKey = "Laboratory_MonstersCounter";
        [SerializeField] private string hikeKey = "Laboratory_HikeCounter";
        [SerializeField] private string counterFormatKey = "lab_counter_format";

        private LabReferences labRef;
        private bool isInitialized = false;

        void Start()
        {
            labRef = LabReferences.Instance();

            // Ждем инициализации LocalizationManager
            StartCoroutine(InitializeAfterLocalization());
        }

        private System.Collections.IEnumerator InitializeAfterLocalization()
        {
            // Ждем, пока LocalizationManager будет готов
            yield return new WaitUntil(() => LocalizationManager.Instance != null);

            // Дополнительная проверка, что локализация загружена
            int maxFrames = 60;
            int frameCount = 0;

            while (frameCount < maxFrames)
            {
                if (LocalizationManager.Instance != null)
                {
                    SubscribeToEvents();
                    UpdateAllCounters();
                    isInitialized = true;

                    // Подписываемся на изменение языка
                    LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
                    break;
                }
                frameCount++;
                yield return null;
            }
        }

        void Update()
        {
            if (isInitialized)
            {
                UpdateAllCounters();
            }
        }

        private void OnLanguageChanged()
        {
            UpdateAllCounters();
        }

        private void SubscribeToEvents()
        {
            if (labRef.expeditionController != null)
            {
                labRef.expeditionController.OnExpeditionChanged += OnExpeditionChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (labRef.expeditionController != null)
            {
                labRef.expeditionController.OnExpeditionChanged -= OnExpeditionChanged;
            }

            if (LocalizationManager.Instance != null)
            {
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
            }
        }

        private void OnExpeditionChanged()
        {
            foreach (var counter in counters)
            {
                if (counter.counterType == CounterType.ExpeditionMonsters && counter.counterText != null)
                {
                    UpdateCounterText(counter);
                }
            }
        }

        private void UpdateAllCounters()
        {
            if (!isInitialized || LocalizationManager.Instance == null) return;

            foreach (var counter in counters)
            {
                if (counter.counterText != null)
                {
                    UpdateCounterText(counter);
                }
            }
        }

        private void UpdateCounterText(CounterEntry counter)
        {
            switch (counter.counterType)
            {
                case CounterType.TotalMonsters:
                    UpdateTotalMonstersCounter(counter);
                    break;
                case CounterType.ExpeditionMonsters:
                    UpdateExpeditionMonstersCounter(counter);
                    break;
            }
        }

        private void UpdateTotalMonstersCounter(CounterEntry counter)
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            int currentMonsters = monstersStorage.storage_monsters?.Count ?? 0;
            int maxMonsters = monstersStorage.max_capacity;

            // Получаем локализованный текст
            string localizedLabel = LocalizationManager.Instance.GetLocalizedValue(monstersKey, "UI_Menu");
            string localizedFormat = LocalizationManager.Instance.GetLocalizedValue(counterFormatKey, "UI_Menu");

            // Используем форматирование: "{0} {1}/{2}" где 0 - метка, 1 - текущее, 2 - максимум
            counter.counterText.text = string.Format(localizedFormat, localizedLabel, currentMonsters, maxMonsters);
        }

        private void UpdateExpeditionMonstersCounter(CounterEntry counter)
        {
            int currentExpeditionMonsters = labRef.expeditionController?.GetExpeditionMonsterCount() ?? 0;

            // Получаем локализованный текст
            string localizedLabel = LocalizationManager.Instance.GetLocalizedValue(hikeKey, "UI_Menu");
            string localizedFormat = LocalizationManager.Instance.GetLocalizedValue(counterFormatKey, "UI_Menu");

            // Используем форматирование
            counter.counterText.text = string.Format(localizedFormat, localizedLabel, currentExpeditionMonsters, counter.maxCount);
        }

        public void AddCounter(CounterType type, TextMeshProUGUI text, int maxCount = 3)
        {
            counters.Add(new CounterEntry
            {
                counterType = type,
                counterText = text,
                maxCount = maxCount
            });

            if (isInitialized)
            {
                UpdateCounterText(counters[counters.Count - 1]);
            }
        }

        public void RemoveCounter(TextMeshProUGUI text)
        {
            counters.RemoveAll(c => c.counterText == text);
        }

        public void UpdateCounter(TextMeshProUGUI text)
        {
            if (!isInitialized) return;

            var counter = counters.Find(c => c.counterText == text);
            if (counter != null)
            {
                UpdateCounterText(counter);
            }
        }

        public void ForceUpdateAllCounters()
        {
            UpdateAllCounters();
        }

        public void SetMaxCountForExpedition(TextMeshProUGUI text, int maxCount)
        {
            var counter = counters.Find(c => c.counterText == text && c.counterType == CounterType.ExpeditionMonsters);
            if (counter != null)
            {
                counter.maxCount = maxCount;
                if (isInitialized)
                {
                    UpdateCounterText(counter);
                }
            }
        }

        public (int current, int max) GetCounterInfo(TextMeshProUGUI text)
        {
            var counter = counters.Find(c => c.counterText == text);
            if (counter != null)
            {
                switch (counter.counterType)
                {
                    case CounterType.TotalMonsters:
                        ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
                        int currentMonsters = monstersStorage.storage_monsters?.Count ?? 0;
                        int maxMonsters = monstersStorage.max_capacity;
                        return (currentMonsters, maxMonsters);

                    case CounterType.ExpeditionMonsters:
                        int currentExpedition = labRef.expeditionController?.GetExpeditionMonsterCount() ?? 0;
                        return (currentExpedition, counter.maxCount);
                }
            }
            return (0, 0);
        }

        void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        // Вспомогательный метод для отладки
        public void DebugLocalizationInfo()
        {
            if (LocalizationManager.Instance != null)
            {
                Debug.Log($"Monsters key: {LocalizationManager.Instance.GetLocalizedValue(monstersKey, "UI_Menu")}");
                Debug.Log($"Hike key: {LocalizationManager.Instance.GetLocalizedValue(hikeKey, "UI_Menu")}");
                Debug.Log($"Format key: {LocalizationManager.Instance.GetLocalizedValue(counterFormatKey, "UI_Menu")}");
            }
        }
    }
}