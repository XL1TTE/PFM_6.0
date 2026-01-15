using Domain.Map;
using Domain.Monster.Mono;
using Persistence.DS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project
{
    public class MapMonstersController : MonoBehaviour
    {
        [Header("Map Screen Slots")]
        public List<MapMonsterSlotMono> mapScreenSlots;

        private bool isInitialized = false;

        void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (isInitialized) return;

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
            mapScreenSlots = mapScreenSlots?.OrderBy(s => s.transform.GetSiblingIndex()).ToList();
        }

        public List<MonsterSlotData> GetMonstersWithSlotInfo()
        {
            ref var storageMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            var monsters = storageMonsters.storage_monsters ?? new List<MonsterData>();

            var slotMonsters = LoadSlotPositions(monsters);

            if (slotMonsters.Count == 0 || slotMonsters.Count != monsters.Count)
            {
                slotMonsters.Clear();

                for (int i = 0; i < monsters.Count && i < mapScreenSlots.Count; i++)
                {
                    slotMonsters.Add(new MonsterSlotData
                    {
                        monsterData = monsters[i],
                        slotIndex = i,
                        slotName = mapScreenSlots[i].name
                    });
                }

                SaveSlotPositions(slotMonsters);
            }

            return slotMonsters;
        }

        private void RefreshMonsterSlotsFromList(List<MapMonsterSlotMono> slots, List<MonsterSlotData> slotMonsters)
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
            foreach (var slot in mapScreenSlots)
            {
                PlayerPrefs.DeleteKey($"MapMonsterSlot_{slot.name}");
            }

            foreach (var slotMonster in slotMonsters)
            {
                if (slotMonster.monsterData != null && !string.IsNullOrEmpty(slotMonster.monsterData.m_MonsterName))
                {
                    PlayerPrefs.SetString($"MapMonsterSlot_{slotMonster.slotName}", slotMonster.monsterData.m_MonsterName);
                }
            }

            string slotOrder = string.Join(",", slotMonsters.Select(sm => sm.slotName));
            PlayerPrefs.SetString("MapMonsterSlotOrder", slotOrder);
            PlayerPrefs.Save();
        }

        private List<MonsterSlotData> LoadSlotPositions(List<MonsterData> monsters)
        {
            var result = new List<MonsterSlotData>();
            string savedOrder = PlayerPrefs.GetString("MapMonsterSlotOrder", "");
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
                string monsterKey = $"MapMonsterSlot_{slotName}";
                string monsterName = PlayerPrefs.GetString(monsterKey, "");

                if (!string.IsNullOrEmpty(monsterName) && monsterDict.ContainsKey(monsterName))
                {
                    int slotIndex = mapScreenSlots.FindIndex(slot => slot.name == slotName);
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
                    var emptySlot = mapScreenSlots[emptySlotIndex];
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

            for (int i = 0; i < mapScreenSlots.Count; i++)
            {
                if (!occupiedIndices.Contains(i))
                {
                    return i;
                }
            }
            return -1;
        }

        public void RefreshAllMonsterSlots()
        {
            var slotMonsters = GetMonstersWithSlotInfo();
            RefreshMonsterSlotsFromList(mapScreenSlots, slotMonsters);
        }

        public void DebugCurrentState()
        {
            Debug.Log("=== MAP DEBUG CURRENT STATE ===");
            Debug.Log("MAP SCREEN:");
            foreach (var slot in mapScreenSlots)
            {
                Debug.Log($"  {slot.name}: occupied={slot.is_occupied}, monster={(slot.currentMonsterData != null ? slot.currentMonsterData.m_MonsterName : "null")}");
            }

            var storageMonsters = GetMonstersWithSlotInfo();
            Debug.Log($"STORAGE: {storageMonsters.Count} monsters with slot info");
        }
    }
}