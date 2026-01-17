using Domain.Map;
using Domain.Monster.Mono;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class MonsterTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private MonsterData monsterData;
        private MonsterTooltipController monsterTooltipController;
        private bool isHovering = false;
        private bool isInitialized = false;
        public bool isPreparationScreen = false;

        public void Initialize(MonsterData data)
        {
            monsterData = data;
            isPreparationScreen = false;

            var labRef = LabReferences.Instance();
            if (labRef != null)
            {
                monsterTooltipController = GetAppropriateTooltipController();
                isInitialized = monsterTooltipController != null;
            }
        }

        public void InitializeForPreparation(MonsterData data, MonsterTooltipController customController = null)
        {
            monsterData = data;
            isPreparationScreen = true;

            if (customController != null)
            {
                monsterTooltipController = customController;
            }
            else
            {
                var labRef = LabReferences.Instance();
                if (labRef != null && labRef.preparationMonsterTooltipController != null)
                {
                    monsterTooltipController = labRef.preparationMonsterTooltipController;
                }
            }

            isInitialized = monsterTooltipController != null;
        }

        private MonsterTooltipController GetAppropriateTooltipController()
        {
            var labRef = LabReferences.Instance();
            if (labRef == null) return null;

            if (IsOnPreparationScreen())
            {
                isPreparationScreen = true;
                return labRef.preparationMonsterTooltipController ?? labRef.monsterTooltipController;
            }

            isPreparationScreen = false;
            return labRef.monsterTooltipController;
        }

        private bool IsOnPreparationScreen()
        {
            if (transform.name.Contains("Preparation", System.StringComparison.OrdinalIgnoreCase))
                return true;

            Transform parent = transform;
            while (parent != null)
            {
                if (parent.name.Contains("PreparetionScreen", System.StringComparison.OrdinalIgnoreCase) ||
                    parent.name.Contains("PreparationScreen", System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                parent = parent.parent;
            }

            return false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isHovering = true;

            if (monsterData != null && monsterTooltipController != null && isInitialized)
            {
                if (!monsterTooltipController.IsTooltipShowing())
                {
                    monsterTooltipController.ShowMonsterTooltip(monsterData, transform.position, isPreparationScreen);
                }
                else if (monsterTooltipController.IsTooltipFixed())
                {
                    monsterTooltipController.monsterTooltipPanel.SetActive(true);
                }
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHovering && monsterTooltipController != null)
            {
                if (!monsterTooltipController.IsTooltipFixed())
                {
                    isHovering = false;
                    monsterTooltipController.HideMonsterTooltip();
                }
            }
        }

        public void OnDisable()
        {
            if (isHovering)
            {
                isHovering = false;
                if (monsterTooltipController != null && !monsterTooltipController.IsTooltipFixed())
                    monsterTooltipController.HideMonsterTooltip();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (monsterTooltipController != null && monsterTooltipController.isFixedByClick)
                {
                    bool wasFixed = monsterTooltipController.IsTooltipFixed();
                    monsterTooltipController.ToggleFix();

                    if (monsterTooltipController.IsTooltipFixed() && !wasFixed)
                    {
                        isHovering = true;
                        Debug.Log("Map tooltip fixed");
                    }
                }
            }
        }

        public void InitializeForMap(MonsterData data, MonsterTooltipController mapController)
        {
            monsterData = data;
            monsterTooltipController = mapController;
            isPreparationScreen = false;
            isInitialized = monsterTooltipController != null;

            if (isInitialized)
            {
                Debug.Log($"Map tooltip initialized for {data.m_MonsterName}");
            }
        }
    }
}