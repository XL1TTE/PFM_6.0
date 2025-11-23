using Domain.Map;
using Domain.Monster.Mono;
using Persistence.DS;
using System.Collections;
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

        private LabReferences labRef;

        private bool isInitialized = false;

        void Start()
        {
            Initialize();
        }
        public void Initialize()
        {
            if (isInitialized) return;

            labRef = LabReferences.Instance();

            SortSlotsByHierarchy();
            StartCoroutine(DelayedRefresh());

            isInitialized = true;
        }

        private IEnumerator DelayedRefresh()
        {
            yield return new WaitForEndOfFrame();
            RefreshAllMonsterSlots();
        }


        private void SortSlotsByHierarchy()
        {
            mainScreenSlots = mainScreenSlots?.OrderBy(s => s.transform.GetSiblingIndex()).ToList();
            preparationScreenSlots = preparationScreenSlots?.OrderBy(s => s.transform.GetSiblingIndex()).ToList();
        }

        public void UpdateStorageFromMainScreen()
        {
            SaveToStorage(GetOccupiedSlots(mainScreenSlots));
        }
        public void UpdateStorageFromPreparationScreen()
        {
            SaveToStorage(GetOccupiedSlots(preparationScreenSlots));
        }

        private void SaveToStorage(List<MonsterSlotData> slotMonsters)
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            monstersStorage.storage_monsters = slotMonsters.Select(s => s.monsterData).ToList();

            SaveSlotPositions(slotMonsters);
        }

        private List<MonsterSlotData> GetOccupiedSlots(List<LabMonsterSlotMono> slots)
        {
            var slotMonsters = new List<MonsterSlotData>();
            if (slots == null) return slotMonsters;

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
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
            return slotMonsters;
        }

        public List<MonsterSlotData> GetMonstersWithSlotInfo()
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

            return slotMonsters;
        }

        private void RefreshMonsterSlotsFromList(List<LabMonsterSlotMono> slots, List<MonsterSlotData> slotMonsters, string screenName)
        {
            if (slots == null) return;

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
            if (string.IsNullOrEmpty(savedOrder)) return result;

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
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(preparationScreenSlots, slotMonsters, "Preparation");
            RefreshPreparationPreviews();
        }

        public void SwitchToMainScreen()
        {
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(mainScreenSlots, slotMonsters, "Main");
        }

        private void RefreshPreparationPreviews()
        {
            if (labRef.previewController != null)
            {
                labRef.previewController.RefreshMonsterPreviews();
            }
        }

        public void OnMainScreenChanged()
        {
            UpdateStorageFromMainScreen();
            RefreshPreparationPreviews();
            labRef.previewController?.OnMonsterOrderChanged();
        }

        public void OnPreparationScreenChanged()
        {
            UpdateStorageFromPreparationScreen();
            labRef.previewController?.OnMonsterOrderChanged();
        }

        public void SaveCurrentState()
        {
            if (labRef.uiController != null && labRef.uiController.IsPreparationScreenActive())
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

            if (labRef.expeditionController != null)
            {
                ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
                labRef.expeditionController.ValidateExpeditionMonsters(monstersStorage.storage_monsters);
            }
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

            if (labRef.expeditionController != null)
            {
                labRef.expeditionController.RemoveDeletedMonsterFromExpedition(monsterData);
            }

            if (labRef.craftController != null)
            {
                labRef.craftController.DeleteMonsterAndReturnParts(monsterData);
            }

            RemoveMonsterFromStorage(monsterData);
            RefreshAllMonsterSlots();
        }

        public void RemoveMonsterFromStorage(MonsterData monsterToRemove)
        {
            if (monsterToRemove == null) return;

            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
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
            return a.Head_id == b.Head_id &&
                   a.Body_id == b.Body_id &&
                   a.NearArm_id == b.NearArm_id &&
                   a.FarArm_id == b.FarArm_id &&
                   a.NearLeg_id == b.NearLeg_id &&
                   a.FarLeg_id == b.FarLeg_id &&
                   a.m_MonsterName == b.m_MonsterName;
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