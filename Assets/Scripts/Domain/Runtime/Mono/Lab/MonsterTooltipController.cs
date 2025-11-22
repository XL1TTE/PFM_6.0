using Domain.Monster.Mono;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class MonsterTooltipController : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject monsterTooltipPanel;
        public TMP_Text monsterNameText;
        public TMP_Text monsterDetailsText;

        [Header("Screen Settings")]
        public bool isForPreparationScreen = false;
        public Vector2 offset = new Vector2(20, 20);
        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private MonsterData currentMonsterData;
        private Vector3 slotWorldPosition;
        private bool isInitialized = false;
        private LabUIController uiController;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            uiController = FindObjectOfType<LabUIController>();
        }

        private void Initialize()
        {
            if (isInitialized) return;

            if (monsterTooltipPanel != null)
            {
                tooltipRect = monsterTooltipPanel.GetComponent<RectTransform>();
                parentCanvas = GetComponentInParent<Canvas>();

                if (parentCanvas == null)
                {
                    parentCanvas = FindObjectOfType<Canvas>();
                }

                monsterTooltipPanel.SetActive(false);
                isInitialized = true;
            }
        }

        private void Update()
        {
            if (monsterTooltipPanel != null && monsterTooltipPanel.activeInHierarchy)
            {
                UpdateTooltipPosition();
            }
        }

        public void ShowMonsterTooltip(MonsterData monsterData, Vector3 worldPosition)
        {
            if (monsterData == null)
            {
                return;
            }

            if (!ShouldShowTooltip())
            {
                return;
            }

            if (!isInitialized)
            {
                Initialize();
            }

            if (monsterTooltipPanel == null || tooltipRect == null || parentCanvas == null)
            {
                return;
            }

            currentMonsterData = monsterData;
            slotWorldPosition = worldPosition;

            if (monsterNameText != null)
                monsterNameText.text = GetMonsterName(monsterData);

            if (monsterDetailsText != null)
                monsterDetailsText.text = FormatMonsterDetails(monsterData);

            monsterTooltipPanel.SetActive(true);
            UpdateTooltipPosition();

        }

        private bool ShouldShowTooltip()
        {
            return true;

            //if (uiController == null)
            //{
            //    uiController = FindObjectOfType<LabUIController>();
            //}

            //if (uiController != null)
            //{
            //    bool isPreparationScreenActive = uiController.IsPreparationScreenActive();

            //    // ƒ≈Ѕј√: ¬ыводим состо€ние экранов
            //    Debug.Log($"ShouldShowTooltip check - isForPreparationScreen: {isForPreparationScreen}, isPreparationScreenActive: {isPreparationScreenActive}, tooltipName: {gameObject.name}");

            //    // “ултип дл€ подготовки должен показыватьс€ только на экране подготовки
            //    // ќсновной тултип - только на главном экране
            //    if (isForPreparationScreen && !isPreparationScreenActive)
            //    {
            //        Debug.Log("Blocking preparation tooltip - not on preparation screen");
            //        return false;
            //    }
            //    if (!isForPreparationScreen && isPreparationScreenActive)
            //    {
            //        Debug.Log("Blocking main tooltip - not on main screen");
            //        return false;
            //    }
            //}
            //else
            //{
            //    Debug.LogWarning("uiController is null in ShouldShowTooltip");
            //}

            //return true;
        }

        public void HideMonsterTooltip()
        {
            if (monsterTooltipPanel != null)
            {
                monsterTooltipPanel.SetActive(false);
            }
            currentMonsterData = null;
        }

        private string GetMonsterName(MonsterData data)
        {
            return !string.IsNullOrEmpty(data.m_MonsterName) ? data.m_MonsterName : "Unnamed Monster";
        }

        private string FormatMonsterDetails(MonsterData data)
        {
            string headName = GetPartName(data.Head_id);
            string bodyName = GetPartName(data.Body_id);
            string leftArmName = GetPartName(data.NearArm_id);
            string rightArmName = GetPartName(data.FarArm_id);
            string leftLegName = GetPartName(data.NearLeg_id);
            string rightLegName = GetPartName(data.FarLeg_id);

            return $"<b>Composition:</b>\n" +
                   $"Х Head: {headName}\n" +
                   $"Х Body: {bodyName}\n" +
                   $"Х Left Arm: {leftArmName}\n" +
                   $"Х Right Arm: {rightArmName}\n" +
                   $"Х Left Leg: {leftLegName}\n" +
                   $"Х Right Leg: {rightLegName}\n\n" +
                   $"<b>Parts IDs:</b>\n" +
                   $"Head: {data.Head_id}\n" +
                   $"Body: {data.Body_id}\n" +
                   $"Left Arm: {data.NearArm_id}\n" +
                   $"Right Arm: {data.FarArm_id}\n" +
                   $"Left Leg: {data.NearLeg_id}\n" +
                   $"Right Leg: {data.FarLeg_id}";
        }

        private string GetPartName(string partId)
        {
            if (Persistence.DB.DataBase.TryFindRecordByID(partId, out var record))
            {
                if (Persistence.DB.DataBase.TryGetRecord<Domain.Components.ID>(record, out var nameRecord))
                {
                    return nameRecord.m_Value;
                }
            }
            return "Unknown Part";
        }

        private void UpdateTooltipPosition()
        {
            if (tooltipRect == null || parentCanvas == null || Camera.main == null)
            {
                return;
            }

            try
            {
                Vector2 panelSize = tooltipRect.sizeDelta;

                Vector3 screenPosition = Camera.main.WorldToScreenPoint(slotWorldPosition);

                screenPosition.x += offset.x;
                screenPosition.y += offset.y;

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

                if (screenPosition.x + panelSize.x > Screen.width)
                {
                    screenPosition.x = Screen.width - panelSize.x - 10f;
                }
                else if (screenPosition.x < 0)
                {
                    screenPosition.x = 10f;
                }

                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentCanvas.GetComponent<RectTransform>(),
                    screenPosition,
                    parentCanvas.worldCamera,
                    out localPoint))
                {
                    tooltipRect.localPosition = localPoint;
                    LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating tooltip position: {e.Message}");
            }
        }
    }
}