using Domain.Components;
using Domain.Map;
using Domain.Monster.Mono;
using Game;
using Persistence.Buiders;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class PreparationMonsterPreviewController : MonoBehaviour
    {
        [Header("Preview Containers")]
        public Transform monsterPreviewsContainer;
        public List<Transform> monsterPreviewSlots;

        private LabReferences labRef;

        private List<GameObject> currentPreviews = new List<GameObject>();
        private Dictionary<string, GameObject> slotPreviewMap = new Dictionary<string, GameObject>();

        void Start()
        {
            labRef = LabReferences.Instance();
            InitializePreviewSlots();
        }


        private void InitializePreviewSlots()
        {
            foreach (var preview in currentPreviews)
            {
                if (preview != null) Destroy(preview);
            }
            currentPreviews.Clear();
            slotPreviewMap.Clear();

            foreach (var slot in monsterPreviewSlots)
            {
                if (slot.childCount > 0)
                {
                    foreach (Transform child in slot)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        public void RefreshMonsterPreviews()
        {
            InitializePreviewSlots();

            if (labRef.monstersController == null || labRef.monstersController.preparationScreenSlots == null)
                return;

            for (int i = 0; i < labRef.monstersController.preparationScreenSlots.Count; i++)
            {
                var prepSlot = labRef.monstersController.preparationScreenSlots[i];
                if (prepSlot.is_occupied && prepSlot.currentMonsterData != null && i < monsterPreviewSlots.Count)
                {
                    CreateMonsterPreviewForSlot(prepSlot.currentMonsterData, prepSlot.name, monsterPreviewSlots[i]);
                }
            }
        }

        private void CreateMonsterPreviewForSlot(MonsterData monsterData, string preparationSlotName, Transform previewSlotTransform)
        {
            GameObject previewContainer = new GameObject($"MonsterPreview_{monsterData.m_MonsterName}_{preparationSlotName}");
            previewContainer.transform.SetParent(previewSlotTransform);
            previewContainer.transform.localPosition = Vector3.zero;
            previewContainer.transform.localScale = Vector3.one * 0.8f;

            var builder = new MonsterBuilder(ECS_Main_Lab.m_labWorld)
                .AttachHead(monsterData.Head_id)
                .AttachBody(monsterData.Body_id)
                .AttachFarArm(monsterData.FarArm_id)
                .AttachNearArm(monsterData.NearArm_id)
                .AttachNearLeg(monsterData.NearLeg_id)
                .AttachFarLeg(monsterData.FarLeg_id);

            var monsterEntity = builder.Build();
            var transformStash = ECS_Main_Lab.m_labWorld.GetStash<TransformRefComponent>();
            ref var monsterTransform = ref transformStash.Get(monsterEntity).Value;

            monsterTransform.position = previewSlotTransform.position;
            monsterTransform.parent = previewContainer.transform;

            currentPreviews.Add(previewContainer);
            slotPreviewMap[preparationSlotName] = previewContainer;
        }

        public void OnPreparationScreenOpened()
        {
            RefreshMonsterPreviews();
        }

        public void OnMonsterOrderChanged()
        {
            RefreshMonsterPreviews();
        }

        public void UpdatePreviewForSlot(string slotName, MonsterData monsterData)
        {
            if (slotPreviewMap.ContainsKey(slotName))
            {
                var oldPreview = slotPreviewMap[slotName];
                if (oldPreview != null)
                {
                    currentPreviews.Remove(oldPreview);
                    Destroy(oldPreview);
                }
                slotPreviewMap.Remove(slotName);
            }

            var prepSlotIndex = labRef.monstersController.preparationScreenSlots.FindIndex(s => s.name == slotName);
            if (prepSlotIndex >= 0 && prepSlotIndex < monsterPreviewSlots.Count && monsterData != null)
            {
                CreateMonsterPreviewForSlot(monsterData, slotName, monsterPreviewSlots[prepSlotIndex]);
            }
        }

        void OnDestroy()
        {
            foreach (var preview in currentPreviews)
            {
                if (preview != null) Destroy(preview);
            }
            currentPreviews.Clear();
            slotPreviewMap.Clear();
        }

        public void ForceRefreshPreviews()
        {
            RefreshMonsterPreviews();
        }

        public void DebugPreviews()
        {
            Debug.Log("=== DEBUG PREVIEWS ===");
            Debug.Log($"Current previews: {currentPreviews.Count}");
            Debug.Log($"Slot preview mappings: {slotPreviewMap.Count}");
        }
    }
}