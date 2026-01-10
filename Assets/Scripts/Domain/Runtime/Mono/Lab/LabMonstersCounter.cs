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

        private LabReferences labRef;
        private bool isInitialized = false;

        void Start()
        {
            labRef = LabReferences.Instance();

            StartCoroutine(InitializeAfterLocalization());
        }

        private System.Collections.IEnumerator InitializeAfterLocalization()
        {
            yield return new WaitUntil(() => LocalizationManager.Instance != null);

            int maxFrames = 60;
            int frameCount = 0;

            while (frameCount < maxFrames)
            {
                if (LocalizationManager.Instance != null)
                {
                    SubscribeToEvents();
                    UpdateAllCounters();
                    isInitialized = true;

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

            string localizedLabel = LocalizationManager.Instance.GetLocalizedValue("Laboratory_MonstersCounter", "UI_Menu");

            counter.counterText.text = string.Format("{0} {1}/{2}", localizedLabel, currentMonsters, maxMonsters);
        }

        private void UpdateExpeditionMonstersCounter(CounterEntry counter)
        {
            int currentExpeditionMonsters = labRef.expeditionController?.GetExpeditionMonsterCount() ?? 0;

            string localizedLabel = LocalizationManager.Instance.GetLocalizedValue("Laboratory_HikeCounter", "UI_Menu");

            counter.counterText.text = string.Format("{0} {1}/{2}", localizedLabel, currentExpeditionMonsters, counter.maxCount);
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
    }
}