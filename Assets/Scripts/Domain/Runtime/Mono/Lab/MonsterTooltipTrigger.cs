using Domain.Map;
using Domain.Monster.Mono;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class MonsterTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private MonsterData monsterData;
        private MonsterTooltipController monsterTooltipController;
        private bool isInitialized = false;

        public void Initialize(MonsterData data)
        {
            monsterData = data;

            var labRef = LabReferences.Instance();
            if (labRef != null)
            {
                monsterTooltipController = labRef.monsterTooltipController;
                isInitialized = monsterTooltipController != null;
            }
        }

        public void InitializeForPreparation(MonsterData data, MonsterTooltipController preparationTooltipController)
        {
            monsterData = data;
            monsterTooltipController = preparationTooltipController;
            isInitialized = monsterTooltipController != null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (monsterData != null && monsterTooltipController != null)
            {
                monsterTooltipController.ShowMonsterTooltip(monsterData, transform.position);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (monsterTooltipController != null)
            {
                monsterTooltipController.HideMonsterTooltip();
            }
        }

        private void OnDisable()
        {
            if (!isInitialized) return;

            if (monsterTooltipController != null)
            {
                monsterTooltipController.HideMonsterTooltip();
            }
        }
    }
}