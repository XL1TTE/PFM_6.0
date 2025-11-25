using Domain.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class TooltipController : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject tooltipPanel;
        public TMP_Text partText;

        [Header("Settings")]
        public Vector2 offset = new Vector2(20, 20);

        private LabReferences labRef;
        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private BodyPartData currentData;
        private Vector3 slotWorldPosition;

        private void Awake()
        {
            labRef = LabReferences.Instance();

            tooltipRect = tooltipPanel.GetComponent<RectTransform>();
            parentCanvas = GetComponentInParent<Canvas>();
            tooltipPanel.SetActive(false);
        }

        private void Update()
        {
            if (tooltipPanel.activeInHierarchy)
            {
                UpdateTooltipPosition();
            }
        }

        public void ShowTooltip(BodyPartData data, Vector3 worldPosition)
        {
            if (data == null) return;

            currentData = data;
            slotWorldPosition = worldPosition;

            partText.text = FormatDetails(data);
            
            if (data.ability_name != null)
            {
                partText.text += $"Ability:" +
                   $"Name: {data.ability_name}\n" +
                   $"Desc: {data.ability_desc}\n" +
                   $"Icon: {data.ability_icon}\n" +
                   $"Shifts: {data.ability_shifts}\n";
            }

            tooltipPanel.SetActive(true);
            UpdateTooltipPosition();
        }

        public void HideTooltip()
        {
            tooltipPanel.SetActive(false);
            currentData = null;
        }

        private string FormatDetails(BodyPartData data)
        {
            return $"Name: {data.partName}\n" +
                   $"Type: {data.type}\n" +
                   $"HP: {data.hp_amount}\n" +
                   $"Speed: {data.speed_amount}\n" +
                   $"Fire: {data.res_fire}\n" +
                   $"Podion: {data.res_poison}\n" +
                   $"Bleed: {data.res_bleed}\n";
        }

        private void UpdateTooltipPosition()
        {
            Vector2 panelSize = tooltipRect.sizeDelta;

            Vector3 screenPosition = Camera.main.WorldToScreenPoint(slotWorldPosition);

            bool isInUpperThird = screenPosition.y > Screen.height * 2f / 3f;

            if (isInUpperThird)
            {
                screenPosition.y -= 250f;
            }
            else
            {
                screenPosition.y += 250f;
            }

            // ПРАВИЛЬНАЯ ПРОВЕРКА ЛЕВОЙ ГРАНИЦЫ:
            // 1. Учитываем половину ширины тултипа + отступ для безопасности
            float leftBoundary = panelSize.x * 0.5f + 20f;

            // 2. Альтернативно: проверяем, будет ли тултип выходить за левый край
            // если левый край тултипа (screenPosition.x - panelSize.x/2) будет меньше 0
            bool willExitLeft = (screenPosition.x - panelSize.x * 0.5f) < 0;

            // 3. Или проверяем текущую позицию относительно левого края
            bool isTooCloseToLeft = screenPosition.x < leftBoundary;

            if (isTooCloseToLeft)
            {
                // Смещаем тултип вправо так, чтобы он не выходил за левый край
                screenPosition.x = leftBoundary + 70f; // Добавляем дополнительный отступ
            }
            else
            {
                screenPosition.x += offset.x;
            }

            // ОГРАНИЧЕНИЯ ПО ГРАНИЦАМ ЭКРАНА:
            // Вычисляем фактические границы тултипа
            float halfWidth = panelSize.x * 0.5f;
            float halfHeight = panelSize.y * 0.5f;

            // Левый край
            if (screenPosition.x - halfWidth < 0)
            {
                screenPosition.x = halfWidth + 10f;
            }
            // Правый край
            else if (screenPosition.x + halfWidth > Screen.width)
            {
                screenPosition.x = Screen.width - halfWidth - 10f;
            }

            // Верхний край
            if (screenPosition.y + halfHeight > Screen.height)
            {
                screenPosition.y = Screen.height - halfHeight - 10f;
            }
            // Нижний край
            else if (screenPosition.y - halfHeight < 0)
            {
                screenPosition.y = halfHeight + 10f;
            }

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                screenPosition,
                parentCanvas.worldCamera,
                out localPoint);

            tooltipRect.localPosition = localPoint;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
        }

        private string GetAdditionalInfo(BodyPartData data)
        {
            return "Additional effects: None";
        }
    }
}