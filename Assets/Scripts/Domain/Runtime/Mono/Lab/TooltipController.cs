using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class TooltipController : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject tooltipPanel;
        public TMP_Text partNameText;
        public TMP_Text detailsText;

        [Header("Settings")]
        public Vector2 offset = new Vector2(20, 20);

        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private BodyPartData currentData;
        private Vector3 slotWorldPosition;

        private void Awake()
        {
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

            partNameText.text = data.partName;

            detailsText.text = FormatDetails(data);

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
            return $"{data.description}\n\n" +
                   $"Type: {data.type}\n" +
                   $"ID: {data.db_id}\n" +
                   $"{GetAdditionalInfo(data)}";
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

            if (screenPosition.y + panelSize.y > Screen.height)
            {
                screenPosition.y = Screen.height - panelSize.y - 10f;
            }
            else if (screenPosition.y < 0)
            {
                screenPosition.y = 10f;
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