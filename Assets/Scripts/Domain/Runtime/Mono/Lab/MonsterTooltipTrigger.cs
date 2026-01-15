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
        private bool isPreparationScreen = false;

        // Основной метод инициализации
        public void Initialize(MonsterData data)
        {
            monsterData = data;
            isPreparationScreen = false; // По умолчанию основной экран

            var labRef = LabReferences.Instance();
            if (labRef != null)
            {
                // Автоматически определяем, какой контроллер использовать
                monsterTooltipController = GetAppropriateTooltipController();
                isInitialized = monsterTooltipController != null;
            }
        }

        // Метод для подготовки (можно передать контроллер явно)
        public void InitializeForPreparation(MonsterData data, MonsterTooltipController customController = null)
        {
            monsterData = data;
            isPreparationScreen = true; // Это экран подготовки

            if (customController != null)
            {
                // Используем переданный контроллер
                monsterTooltipController = customController;
            }
            else
            {
                // Или получаем из LabReferences
                var labRef = LabReferences.Instance();
                if (labRef != null && labRef.preparationMonsterTooltipController != null)
                {
                    monsterTooltipController = labRef.preparationMonsterTooltipController;
                }
            }

            isInitialized = monsterTooltipController != null;
        }

        // Метод для автоматического определения контроллера
        private MonsterTooltipController GetAppropriateTooltipController()
        {
            var labRef = LabReferences.Instance();
            if (labRef == null) return null;

            // ПРЯМАЯ проверка: если это экран подготовки, используем preparation контроллер
            if (IsOnPreparationScreen())
            {
                isPreparationScreen = true;
                return labRef.preparationMonsterTooltipController ?? labRef.monsterTooltipController;
            }

            // Иначе основной
            isPreparationScreen = false;
            return labRef.monsterTooltipController;
        }

        private bool IsOnPreparationScreen()
        {
            // Проверяем по имени или по родительской иерархии
            if (transform.name.Contains("Preparation", System.StringComparison.OrdinalIgnoreCase))
                return true;

            // Или проверяем родительскую панель
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
                    // Для карты используем специальную логику позиционирования
                    monsterTooltipController.ShowMonsterTooltip(monsterData, transform.position, false);
                }
                else if (monsterTooltipController.IsTooltipFixed())
                {
                    monsterTooltipController.monsterTooltipPanel.SetActive(true);
                }
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            // Важно: сбрасываем isHovering только если тултип не зафиксирован
            if (isHovering && monsterTooltipController != null)
            {
                if (!monsterTooltipController.IsTooltipFixed())
                {
                    isHovering = false;
                    monsterTooltipController.HideMonsterTooltip();
                }
                // Если тултип зафиксирован, не сбрасываем isHovering
            }
        }

        public void OnDisable()
        {
            // При отключении объекта сбрасываем состояние
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
            isPreparationScreen = false; // Это не экран подготовки
            isInitialized = monsterTooltipController != null;

            if (isInitialized)
            {
                Debug.Log($"Map tooltip initialized for {data.m_MonsterName}");
            }
        }
    }
}