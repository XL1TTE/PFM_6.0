using Domain.Map;
using Domain.Monster.Mono;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class ExpeditionController : MonoBehaviour
    {
        [Header("Expedition Slots")]
        public List<LabMonsterExpeditionSlotMono> expeditionSlots;

        private LabReferences labRef;

        public event Action OnExpeditionChanged;

        void Start()
        {
            labRef = LabReferences.Instance();
            InitializeExpeditionSlots();
        }

        private void InitializeExpeditionSlots()
        {
            if (expeditionSlots != null)
            {
                foreach (var slot in expeditionSlots)
                {
                    slot.Initialize();
                }
            }
        }

        public void OnExpeditionSlotsChanged()
        {
            for (int i = 0; i < expeditionSlots.Count; i++)
            {
                var slot = expeditionSlots[i];
            }
            SaveExpeditionState();

            OnExpeditionChanged?.Invoke();
        }

        public List<MonsterData> GetExpeditionMonsters()
        {
            var monsters = expeditionSlots
                .Where(slot => slot.is_occupied)
                .Select(slot => slot.currentMonsterData)
                .ToList();

            return monsters;
        }

        public int GetExpeditionMonsterCount()
        {
            int count = expeditionSlots.Count(slot => slot.is_occupied);
            return count;
        }

        public bool IsMonsterInExpedition(MonsterData monsterData)
        {
            if (monsterData == null) return false;

            return expeditionSlots.Any(slot =>
                slot.is_occupied &&
                slot.currentMonsterData != null &&
                IsExactlySameMonster(slot.currentMonsterData, monsterData));
        }

        public void DebugExpeditionMonsters()
        {
            var monsters = GetExpeditionMonsters();
            Debug.Log($"Expedition has {monsters.Count} monsters:");
            foreach (var monster in monsters)
            {
                Debug.Log($" - {monster.m_MonsterName}");
            }
        }

        public void ReturnMonsterToPreparation(MonsterData monsterData, LabMonsterSlotMono targetSlot = null)
        {
            if (monsterData == null) return;

            if (targetSlot != null && !targetSlot.is_occupied)
            {
                targetSlot.OccupySelf(monsterData);
            }
            else
            {
                var preparationSlots = labRef.monstersController.preparationScreenSlots;
                var emptySlot = preparationSlots.FirstOrDefault(slot => !slot.is_occupied);

                if (emptySlot != null)
                {
                    emptySlot.OccupySelf(monsterData);
                }
            }

            OnExpeditionSlotsChanged();

            if (labRef.monstersController != null)
            {
                labRef.monstersController.OnPreparationScreenChanged();
            }
        }

        private void SaveExpeditionState()
        {
            for (int i = 0; i < expeditionSlots.Count; i++)
            {
                var slot = expeditionSlots[i];
                string key = $"ExpeditionSlot_{i}";

                if (slot.is_occupied && slot.currentMonsterData != null)
                {
                    PlayerPrefs.SetString(key, slot.currentMonsterData.m_MonsterName);
                }
                else
                {
                    PlayerPrefs.DeleteKey(key);
                }
            }
            PlayerPrefs.Save();
        }

        public void LoadExpeditionState()
        {
            var storageMonsters = labRef.monstersController.GetMonstersWithSlotInfo();
            var monsterDict = storageMonsters.ToDictionary(m => m.monsterData.m_MonsterName, m => m.monsterData);

            for (int i = 0; i < expeditionSlots.Count; i++)
            {
                string key = $"ExpeditionSlot_{i}";
                if (PlayerPrefs.HasKey(key))
                {
                    string monsterName = PlayerPrefs.GetString(key);
                    if (monsterDict.ContainsKey(monsterName))
                    {
                        expeditionSlots[i].OccupySelf(monsterDict[monsterName]);
                    }
                }
            }
        }

        public void OnStartExpeditionClicked()
        {
            var expeditionMonsters = GetExpeditionMonsters();
            if (expeditionMonsters.Count > 0)
            {
                Debug.Log($"Starting expedition with {expeditionMonsters.Count} monsters:");
                foreach (var monster in expeditionMonsters)
                {
                    Debug.Log($" - {monster.m_MonsterName}");
                }
            }
        }

        public void RemoveDeletedMonsterFromExpedition(MonsterData deletedMonster)
        {
            if (deletedMonster == null || expeditionSlots == null)
            {
                Debug.Log("RemoveDeletedMonsterFromExpedition: null parameters");
                return;
            }

            int removedCount = 0;

            foreach (var slot in expeditionSlots)
            {
                if (slot != null && slot.is_occupied && slot.currentMonsterData != null)
                {
                    if (IsExactlySameMonster(slot.currentMonsterData, deletedMonster))
                    {
                        Debug.Log($"Removing deleted monster from expedition slot: {slot.name}");
                        slot.ClearSlot();
                        removedCount++;
                    }
                }
            }

            if (removedCount > 0)
            {
                SaveExpeditionState();
            }
            else
            {
                Debug.Log($"Deleted monster not found in expedition slots");
            }
        }

        private bool IsExactlySameMonster(MonsterData a, MonsterData b)
        {
            if (a == null || b == null)
            {
                return false;
            }

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

        public void ValidateExpeditionMonsters(List<MonsterData> validMonsters)
        {
            if (expeditionSlots == null) return;

            int clearedCount = 0;

            foreach (var slot in expeditionSlots)
            {
                if (slot != null && slot.is_occupied && slot.currentMonsterData != null)
                {
                    bool monsterExists = false;
                    foreach (var validMonster in validMonsters)
                    {
                        if (IsExactlySameMonster(slot.currentMonsterData, validMonster))
                        {
                            monsterExists = true;
                            break;
                        }
                    }

                    if (!monsterExists)
                    {
                        slot.ClearSlot();
                        clearedCount++;
                    }
                }
            }

            if (clearedCount > 0)
            {
                SaveExpeditionState();
            }
        }

        public void ClearExpeditionSlotsAfterExpedition()
        {
            foreach (var slot in expeditionSlots)
            {
                slot.ClearSlot();
            }
            SaveExpeditionState();
        }
    }
}