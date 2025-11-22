using Domain.Map;
using Persistence.DS;
using TMPro;
using UnityEngine;

namespace Project
{
    public class LabMonstersCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI monstersCounterText;
        private LabMonsterCraftController craftController;

        void Start()
        {
            craftController = LabReferences.Instance().craftController;
            UpdateMonstersCounter();
        }

        void Update()
        {
            UpdateMonstersCounter();
        }

        private void UpdateMonstersCounter()
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            int currentMonsters = monstersStorage.storage_monsters?.Count ?? 0;
            int maxMonsters = monstersStorage.max_capacity;

            if (monstersCounterText != null)
            {
                monstersCounterText.text = $"Monsters {currentMonsters}/{maxMonsters}";
            }
        }
    }
}