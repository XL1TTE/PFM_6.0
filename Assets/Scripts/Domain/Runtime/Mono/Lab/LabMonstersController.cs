using Domain.Monster.Mono;
using Persistence.DS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    [System.Serializable]
    public class MonsterSlotData
    {
        public MonsterData monsterData;
        public int slotIndex;
        public string slotName;
    }

    public class LabMonstersController : MonoBehaviour
    {
        [Header("Main Screen Slots")]
        public List<LabMonsterSlotMono> mainScreenSlots;

        [Header("Preparation Screen Slots")]
        public List<LabMonsterSlotMono> preparationScreenSlots;

        private PreparationMonsterPreviewController previewController;
        private LabMonsterCraftController craftController;

        void Start()
        {
            SortSlotsByHierarchy();

            previewController = FindObjectOfType<PreparationMonsterPreviewController>();
            craftController = FindObjectOfType<LabMonsterCraftController>();

            RefreshAllMonsterSlots();
        }

        private void SortSlotsByHierarchy()
        {
            if (mainScreenSlots != null)
            {
                mainScreenSlots = mainScreenSlots.OrderBy(s => s.transform.GetSiblingIndex()).ToList();
            }

            if (preparationScreenSlots != null)
            {
                preparationScreenSlots = preparationScreenSlots.OrderBy(s => s.transform.GetSiblingIndex()).ToList();
            }
        }

        public void UpdateStorageFromMainScreen()
        {
            var slotMonsters = new List<MonsterSlotData>();

            for (int i = 0; i < mainScreenSlots.Count; i++)
            {
                var slot = mainScreenSlots[i];
                if (slot.is_occupied && slot.currentMonsterData != null)
                {
                    slotMonsters.Add(new MonsterSlotData
                    {
                        monsterData = slot.currentMonsterData,
                        slotIndex = i,
                        slotName = slot.name
                    });
                }
            }

            SaveToStorage(slotMonsters);
        }
        public void UpdateStorageFromPreparationScreen()
        {
            var slotMonsters = new List<MonsterSlotData>();

            for (int i = 0; i < preparationScreenSlots.Count; i++)
            {
                var slot = preparationScreenSlots[i];
                if (slot.is_occupied && slot.currentMonsterData != null)
                {
                    slotMonsters.Add(new MonsterSlotData
                    {
                        monsterData = slot.currentMonsterData,
                        slotIndex = i,
                        slotName = slot.name
                    });
                }
            }

            SaveToStorage(slotMonsters);
        }

        private void SaveToStorage(List<MonsterSlotData> slotMonsters)
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            monstersStorage.storage_monsters = slotMonsters.Select(s => s.monsterData).ToList();

            SaveSlotPositions(slotMonsters);
        }

        private List<MonsterSlotData> GetMonstersWithSlotInfo()
        {
            ref var storageMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            var monsters = storageMonsters.storage_monsters ?? new List<MonsterData>();

            var slotMonsters = LoadSlotPositions(monsters);

            if (slotMonsters.Count == 0 || slotMonsters.Count != monsters.Count)
            {
                slotMonsters.Clear();

                for (int i = 0; i < monsters.Count && i < mainScreenSlots.Count; i++)
                {
                    slotMonsters.Add(new MonsterSlotData
                    {
                        monsterData = monsters[i],
                        slotIndex = i,
                        slotName = mainScreenSlots[i].name
                    });
                }

                SaveSlotPositions(slotMonsters);
            }

            foreach (var slotMonster in slotMonsters)
            {
                Debug.Log($"  - Monster: {slotMonster.monsterData.m_MonsterName}, Slot: {slotMonster.slotName} (index: {slotMonster.slotIndex})");
            }

            return slotMonsters;
        }

        private void RefreshMonsterSlotsFromList(List<LabMonsterSlotMono> slots, List<MonsterSlotData> slotMonsters, string screenName)
        {
            if (slots == null)
            {
                return;
            }

            foreach (var slot in slots)
            {
                slot.ClearSlot();
            }

            foreach (var slotMonster in slotMonsters)
            {
                if (slotMonster.slotIndex >= 0 && slotMonster.slotIndex < slots.Count)
                {
                    var slot = slots[slotMonster.slotIndex];
                    if (slotMonster.monsterData != null)
                    {
                        slot.OccupySelf(slotMonster.monsterData);
                    }
                }
                else
                {
                }
            }
        }
        private void SaveSlotPositions(List<MonsterSlotData> slotMonsters)
        {
            foreach (var slot in mainScreenSlots)
            {
                PlayerPrefs.DeleteKey($"MonsterSlot_{slot.name}");
            }

            foreach (var slotMonster in slotMonsters)
            {
                if (slotMonster.monsterData != null && !string.IsNullOrEmpty(slotMonster.monsterData.m_MonsterName))
                {
                    PlayerPrefs.SetString($"MonsterSlot_{slotMonster.slotName}", slotMonster.monsterData.m_MonsterName);
                }
            }

            string slotOrder = string.Join(",", slotMonsters.Select(sm => sm.slotName));
            PlayerPrefs.SetString("MonsterSlotOrder", slotOrder);

            PlayerPrefs.Save();
        }

        public void UpdateMonsterSlotsFromCrafting()
        {

            var slotMonsters = GetMonstersWithSlotInfo();

            RefreshMonsterSlotsFromList(mainScreenSlots, slotMonsters, "Main");
        }

        private List<MonsterSlotData> LoadSlotPositions(List<MonsterData> monsters)
        {
            var result = new List<MonsterSlotData>();

            string savedOrder = PlayerPrefs.GetString("MonsterSlotOrder", "");
            if (string.IsNullOrEmpty(savedOrder))
            {
                return result;
            }

            var slotNames = savedOrder.Split(',');

            var monsterDict = new Dictionary<string, MonsterData>();
            foreach (var monster in monsters)
            {
                if (!string.IsNullOrEmpty(monster.m_MonsterName))
                {
                    monsterDict[monster.m_MonsterName] = monster;
                }
            }

            foreach (string slotName in slotNames)
            {
                string monsterKey = $"MonsterSlot_{slotName}";
                string monsterName = PlayerPrefs.GetString(monsterKey, "");

                if (!string.IsNullOrEmpty(monsterName) && monsterDict.ContainsKey(monsterName))
                {
                    int slotIndex = mainScreenSlots.FindIndex(slot => slot.name == slotName);
                    if (slotIndex >= 0)
                    {
                        result.Add(new MonsterSlotData
                        {
                            monsterData = monsterDict[monsterName],
                            slotIndex = slotIndex,
                            slotName = slotName
                        });

                        monsterDict.Remove(monsterName);
                    }
                }
            }

            if (monsterDict.Count > 0)
            {

                foreach (var newMonster in monsterDict.Values)
                {
                    int emptySlotIndex = FindFirstEmptySlotIndex(result);
                    if (emptySlotIndex >= 0)
                    {
                        var emptySlot = mainScreenSlots[emptySlotIndex];
                        result.Add(new MonsterSlotData
                        {
                            monsterData = newMonster,
                            slotIndex = emptySlotIndex,
                            slotName = emptySlot.name
                        });
                    }
                }
            }
            return result;
        }

        private int FindFirstEmptySlotIndex(List<MonsterSlotData> currentMonsters)
        {
            var occupiedIndices = new HashSet<int>();
            foreach (var monster in currentMonsters)
            {
                occupiedIndices.Add(monster.slotIndex);
            }

            for (int i = 0; i < mainScreenSlots.Count; i++)
            {
                if (!occupiedIndices.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public void SwitchToPreparationScreen()
        {
            Debug.Log("=== SwitchToPreparationScreen ===");
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(preparationScreenSlots, slotMonsters, "Preparation");
            RefreshPreparationPreviews();
            Debug.Log($"Preparation screen updated with {slotMonsters.Count} monsters from storage");
        }

        public void SwitchToMainScreen()
        {
            Debug.Log("=== SwitchToMainScreen ===");
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(mainScreenSlots, slotMonsters, "Main");
            Debug.Log($"Main screen updated with {slotMonsters.Count} monsters from storage");
        }

        private void RefreshPreparationPreviews()
        {
            if (previewController != null)
            {
                previewController.RefreshMonsterPreviews();
            }
        }

        public void OnMainScreenChanged()
        {
            UpdateStorageFromMainScreen();
            RefreshPreparationPreviews();

            if (previewController != null)
            {
                previewController.OnMonsterOrderChanged();
            }
        }

        public void OnPreparationScreenChanged()
        {
            UpdateStorageFromPreparationScreen();

            if (previewController != null)
            {
                previewController.OnMonsterOrderChanged();
            }
            else
            {
                previewController = FindObjectOfType<PreparationMonsterPreviewController>();
                if (previewController != null)
                {
                    previewController.OnMonsterOrderChanged();
                }
            }
        }

        public void SaveCurrentState()
        {
            var uiController = FindObjectOfType<LabUIController>();
            if (uiController != null && uiController.IsPreparationScreenActive())
            {
                UpdateStorageFromPreparationScreen();
            }
            else
            {
                UpdateStorageFromMainScreen();
            }
        }

        public void RefreshAllMonsterSlots()
        {
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(mainScreenSlots, slotMonsters, "Main");
            RefreshMonsterSlotsFromList(preparationScreenSlots, slotMonsters, "Preparation");
            RefreshPreparationPreviews();
        }

        public void DebugCurrentState()
        {
            Debug.Log("=== DEBUG CURRENT STATE ===");

            Debug.Log("MAIN SCREEN:");
            foreach (var slot in mainScreenSlots)
            {
                Debug.Log($"  {slot.name}: occupied={slot.is_occupied}, monster={(slot.currentMonsterData != null ? slot.currentMonsterData.m_MonsterName : "null")}");
            }

            Debug.Log("PREPARATION SCREEN:");
            foreach (var slot in preparationScreenSlots)
            {
                Debug.Log($"  {slot.name}: occupied={slot.is_occupied}, monster={(slot.currentMonsterData != null ? slot.currentMonsterData.m_MonsterName : "null")}");
            }

            var storageMonsters = GetMonstersWithSlotInfo();
            Debug.Log($"STORAGE: {storageMonsters.Count} monsters with slot info");
        }

        public void DeleteMonster(MonsterData monsterData)
        {
            if (monsterData == null) return;


            ref var monstersStorageBefore = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            foreach (var m in monstersStorageBefore.storage_monsters)
            {
                Debug.Log($"  - {m.m_MonsterName} (Head: {m.Head_id})");
            }

            if (craftController != null)
            {
                craftController.DeleteMonsterAndReturnParts(monsterData);
            }

            RemoveMonsterFromStorage(monsterData);

            RefreshAllMonsterSlots();

            ref var monstersStorageAfter = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            foreach (var m in monstersStorageAfter.storage_monsters)
            {
                Debug.Log($"  - {m.m_MonsterName} (Head: {m.Head_id})");
            }
        }

        public void RemoveMonsterFromStorage(MonsterData monsterToRemove)
        {
            if (monsterToRemove == null)
            {
                return;
            }


            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            for (int i = 0; i < monstersStorage.storage_monsters.Count; i++)
            {
                var monster = monstersStorage.storage_monsters[i];
            }

            var updatedMonsters = new List<MonsterData>();
            int removedCount = 0;

            foreach (var monster in monstersStorage.storage_monsters)
            {
                if (!IsExactlySameMonster(monster, monsterToRemove))
                {
                    updatedMonsters.Add(monster);
                }
                else
                {
                    removedCount++;
                }
            }

            monstersStorage.storage_monsters = updatedMonsters;


            RemoveMonsterFromSavedPositions(monsterToRemove);

            SaveCurrentState();
        }

        private bool IsExactlySameMonster(MonsterData a, MonsterData b)
        {
            if (a == null || b == null) return false;

            bool same = a.Head_id == b.Head_id &&
                        a.Body_id == b.Body_id &&
                        a.NearArm_id == b.NearArm_id &&
                        a.FarArm_id == b.FarArm_id &&
                        a.NearLeg_id == b.NearLeg_id &&
                        a.FarLeg_id == b.FarLeg_id &&
                        a.m_MonsterName == b.m_MonsterName;

            if (same)
            {
                Debug.Log($"Exact monster match: {a.m_MonsterName}");
            }

            return same;
        }

        private void RemoveMonsterFromSavedPositions(MonsterData monsterToRemove)
        {
            PlayerPrefs.DeleteKey($"MonsterSlot_{monsterToRemove.m_MonsterName}");

            string savedOrder = PlayerPrefs.GetString("MonsterSlotOrder", "");
            if (!string.IsNullOrEmpty(savedOrder))
            {
                var slotNames = savedOrder.Split(',');
                var newOrder = new List<string>();

                foreach (string slotName in slotNames)
                {
                    string monsterName = PlayerPrefs.GetString($"MonsterSlot_{slotName}", "");
                    if (monsterName != monsterToRemove.m_MonsterName)
                    {
                        newOrder.Add(slotName);
                    }
                    else
                    {
                        PlayerPrefs.DeleteKey($"MonsterSlot_{slotName}");
                    }
                }

                PlayerPrefs.SetString("MonsterSlotOrder", string.Join(",", newOrder));
            }

            PlayerPrefs.Save();
        }
    }
}