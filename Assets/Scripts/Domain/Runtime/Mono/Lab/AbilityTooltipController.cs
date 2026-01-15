using Domain.Map;
using Persistence.Components;
using Persistence.DB;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class AbilityTooltipController : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject abilityTooltipPanel;
        public TMP_Text abilityNameText;
        public TMP_Text abilityDescriptionText;
        public Image abilityIcon;
        public GridLayoutGroup shiftsGrid;

        [Header("Colors")]
        public Color activeCellColor = Color.red;
        public Color inactiveCellColor = Color.green;
        public Color combinedCellColor = Color.blue;
        public Color centerCellColor = new Color(1f, 0.5f, 0.8f);

        [Header("Position Settings")]
        public Vector2 offset = new Vector2(0, 60f);
        public bool clampToScreen = true;
        public Canvas targetCanvas;

        public bool isPreparationScreen = false;

        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private BodyPartData currentPartData;
        private Vector3 triggerWorldPosition;
        private bool isShowing = false;
        private Transform gridTransform;
        private Dictionary<string, BodyPartData> cachedPartsData = new Dictionary<string, BodyPartData>();
        private HashSet<Vector2Int> combinedShifts = new HashSet<Vector2Int>();

        // Ссылки на парные части для комбинации
        private BodyPartData pairedPartData;
        private bool isCombinedView = false;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (abilityTooltipPanel != null)
            {
                tooltipRect = abilityTooltipPanel.GetComponent<RectTransform>();

                // Находим Canvas
                if (targetCanvas != null)
                {
                    parentCanvas = targetCanvas;
                }
                else
                {
                    parentCanvas = GetComponentInParent<Canvas>();
                    if (parentCanvas == null)
                    {
                        parentCanvas = FindObjectOfType<Canvas>();
                    }
                }

                abilityTooltipPanel.SetActive(false);

                if (shiftsGrid != null)
                {
                    gridTransform = shiftsGrid.transform;
                    InitializeGridCells();
                }
            }
        }

        // Основные публичные методы для показа тултипа
        public void ShowTooltip(string abilityId, Vector3 worldPosition)
        {
            if (string.IsNullOrEmpty(abilityId) || isShowing) return;

            BodyPartData abilityData = GetAbilityData(abilityId);
            if (abilityData == null) return;

            ShowTooltipInternal(abilityData, worldPosition);
        }

        public void ShowTooltip(BodyPartData abilityData, Vector3 worldPosition)
        {
            if (abilityData == null || isShowing) return;

            ShowTooltipInternal(abilityData, worldPosition);
        }

        public void ShowTooltipForPart(string partId, Vector3 worldPosition)
        {
            if (string.IsNullOrEmpty(partId) || isShowing) return;

            BodyPartData partData = GetBodyPartData(partId);
            if (partData == null || partData.type == BODYPART_TYPE.TORSO) return;

            ShowTooltipInternal(partData, worldPosition);
        }

        // Упрощенный метод для прямого указания данных
        public void ShowTooltip(string abilityName, string abilityDescription, Sprite icon, Vector2Int[] shifts, Vector3 worldPosition)
        {
            if (isShowing) return;

            BodyPartData tempData = new BodyPartData
            {
                ability_name = abilityName,
                ability_desc = abilityDescription,
                ability_icon = icon,
                ability_shifts = shifts
            };

            ShowTooltipInternal(tempData, worldPosition);
        }

        public void ShowCombinedLegsTooltip(string leftLegId, string rightLegId, Vector3 worldPosition)
        {
            if (string.IsNullOrEmpty(leftLegId) || string.IsNullOrEmpty(rightLegId) || isShowing) return;

            BodyPartData leftLegData = GetBodyPartData(leftLegId);
            BodyPartData rightLegData = GetBodyPartData(rightLegId);

            if (leftLegData == null || rightLegData == null ||
                leftLegData.type != BODYPART_TYPE.LEG ||
                rightLegData.type != BODYPART_TYPE.LEG) return;

            ShowCombinedLegsInternal(leftLegData, rightLegData, worldPosition);
        }

        // НОВЫЙ МЕТОД: Показать комбинированный тултип с готовыми данными
        public void ShowCombinedLegsTooltip(BodyPartData leftLegData, BodyPartData rightLegData, Vector3 worldPosition)
        {
            if (leftLegData == null || rightLegData == null || isShowing) return;
            if (leftLegData.type != BODYPART_TYPE.LEG || rightLegData.type != BODYPART_TYPE.LEG) return;

            ShowCombinedLegsInternal(leftLegData, rightLegData, worldPosition);
        }

        private void ShowCombinedLegsInternal(BodyPartData leftLegData, BodyPartData rightLegData, Vector3 worldPosition)
        {
            if (!IsInitialized()) Initialize();
            if (abilityTooltipPanel == null || tooltipRect == null) return;

            triggerWorldPosition = worldPosition;
            isShowing = true;
            isCombinedView = true;
            pairedPartData = rightLegData;

            UpdateCombinedLegsUI(leftLegData, rightLegData);

            abilityTooltipPanel.SetActive(true);

            if (isPreparationScreen)
            {
                UpdateTooltipPositionForPreparation();
            }
            else
            {
                UpdateTooltipPosition();
            }
        }

        private void ShowTooltipInternal(BodyPartData data, Vector3 worldPosition)
        {
            if (!IsInitialized()) Initialize();
            if (abilityTooltipPanel == null || tooltipRect == null) return;

            currentPartData = data;
            triggerWorldPosition = worldPosition;
            isShowing = true;
            isCombinedView = false;
            pairedPartData = null;

            UpdateAbilityUI(data);

            abilityTooltipPanel.SetActive(true);
            if (isPreparationScreen)
            {
                UpdateTooltipPositionForPreparation();
            }
            else
            {
                UpdateTooltipPosition();
            }
        }

        private void UpdateCombinedLegsUI(BodyPartData leftLegData, BodyPartData rightLegData)
        {
            if (abilityNameText != null)
            {
                abilityNameText.text = "<b>Movement (Combined)</b>";
            }

            if (abilityDescriptionText != null)
            {
                string leftDesc = !string.IsNullOrEmpty(leftLegData.ability_desc) ? leftLegData.ability_desc : "Movement ability";
                string rightDesc = !string.IsNullOrEmpty(rightLegData.ability_desc) ? rightLegData.ability_desc : "Movement ability";

                string description = $"<color=#FF6B6B>Left Leg:</color> {leftDesc}\n<color=#4ECDC4>Right Leg:</color> {rightDesc}";
                abilityDescriptionText.text = description;
            }

            if (abilityIcon != null)
            {
                if (leftLegData.ability_icon != null)
                {
                    abilityIcon.sprite = leftLegData.ability_icon;
                    abilityIcon.enabled = true;
                }
                else if (rightLegData.ability_icon != null)
                {
                    abilityIcon.sprite = rightLegData.ability_icon;
                    abilityIcon.enabled = true;
                }
                else
                {
                    abilityIcon.enabled = false;
                }
            }

            if (shiftsGrid != null)
            {
                UpdateCombinedShiftGrid(leftLegData.ability_shifts, rightLegData.ability_shifts);
            }
            else
            {
                ClearShiftGrid();
            }
        }

        private void UpdateAbilityUI(BodyPartData data)
        {
            if (abilityNameText != null)
            {
                string abilityName = !string.IsNullOrEmpty(data.ability_name) ?
                    data.ability_name : GetDefaultAbilityName(data.type);
                abilityNameText.text = $"<b>{abilityName}</b>";
            }

            if (abilityDescriptionText != null && !string.IsNullOrEmpty(data.ability_desc))
            {
                abilityDescriptionText.text = data.ability_desc;
            }

            if (abilityIcon != null)
            {
                if (data.ability_icon != null)
                {
                    abilityIcon.sprite = data.ability_icon;
                    abilityIcon.enabled = true;
                }
                else
                {
                    abilityIcon.enabled = false;
                }
            }

            if (shiftsGrid != null && data.ability_shifts != null && data.ability_shifts.Length > 0)
            {
                UpdateShiftGrid(data.ability_shifts);
            }
            else
            {
                ClearShiftGrid();
            }
        }

        private string GetDefaultAbilityName(BODYPART_TYPE type)
        {
            return type switch
            {
                BODYPART_TYPE.HEAD => "Head Ability",
                BODYPART_TYPE.ARM => "Arm Ability",
                BODYPART_TYPE.LEG => "Movement",
                _ => "Ability"
            };
        }

        private void UpdateCombinedShiftGrid(Vector2Int[] leftShifts, Vector2Int[] rightShifts)
        {
            ClearShiftGrid();
            combinedShifts.Clear();

            if (leftShifts == null && rightShifts == null) return;

            // Собираем все уникальные сдвиги
            HashSet<Vector2Int> allShifts = new HashSet<Vector2Int>();
            HashSet<Vector2Int> leftShiftsSet = new HashSet<Vector2Int>();
            HashSet<Vector2Int> rightShiftsSet = new HashSet<Vector2Int>();

            if (leftShifts != null)
            {
                foreach (Vector2Int shift in leftShifts)
                {
                    allShifts.Add(shift);
                    leftShiftsSet.Add(shift);
                }
            }

            if (rightShifts != null)
            {
                foreach (Vector2Int shift in rightShifts)
                {
                    allShifts.Add(shift);
                    rightShiftsSet.Add(shift);
                }
            }

            // Отображаем все сдвиги с разными цветами
            foreach (Vector2Int shift in allShifts)
            {
                int cellIndex = ShiftToGridIndex(shift);
                if (cellIndex >= 0 && cellIndex < gridTransform.childCount)
                {
                    Image cellImage = gridTransform.GetChild(cellIndex).GetComponent<Image>();
                    if (cellImage != null)
                    {
                        bool inLeft = leftShiftsSet.Contains(shift);
                        bool inRight = rightShiftsSet.Contains(shift);

                        if (shift == Vector2Int.zero)
                        {
                            // Центральная клетка всегда розовая, независимо от комбинации
                            cellImage.color = centerCellColor;
                        }
                        else if (inLeft && inRight)
                        {
                            // Сдвиг есть в обеих ногах - синий цвет
                            cellImage.color = combinedCellColor;
                        }
                        else if (inLeft)
                        {
                            // Только в левой ноге - красный с прозрачностью
                            cellImage.color = activeCellColor;
                        }
                        else if (inRight)
                        {
                            // Только в правой ноге - другой оттенок красного
                            cellImage.color = activeCellColor;
                        }
                    }
                }
            }
        }

        private void UpdateShiftGrid(Vector2Int[] abilityShifts)
        {
            ClearShiftGrid();

            if (abilityShifts == null) return;

            foreach (Vector2Int shift in abilityShifts)
            {
                int cellIndex = ShiftToGridIndex(shift);
                if (cellIndex >= 0 && cellIndex < gridTransform.childCount)
                {
                    Image cellImage = gridTransform.GetChild(cellIndex).GetComponent<Image>();
                    if (cellImage != null)
                    {
                        if (shift == Vector2Int.zero)
                        {
                            // Центральная клетка - розовый цвет
                            cellImage.color = centerCellColor;
                        }
                        else
                        {
                            // Остальные клетки - красный цвет
                            cellImage.color = activeCellColor;
                        }
                    }
                }
            }
        }

        private int ShiftToGridIndex(Vector2Int shift)
        {
            // 5x5 grid: (-2,-2) to (2,2)
            // Convert to 0-24 index
            int gridX = shift.x + 2;
            int gridY = shift.y + 2;

            if (gridX < 0 || gridX > 4 || gridY < 0 || gridY > 4)
                return -1;

            return gridY * 5 + gridX;
        }

        private void InitializeGridCells()
        {
            if (gridTransform == null) return;

            for (int i = 0; i < gridTransform.childCount; i++)
            {
                Image cellImage = gridTransform.GetChild(i).GetComponent<Image>();
                if (cellImage != null)
                {
                    if(i == 12)
                    {
                        cellImage.color = Color.red;
                    }
                    else
                    {
                        cellImage.color = inactiveCellColor; 
                    }
                }
            }
        }

        private void ClearShiftGrid()
        {
            if (gridTransform == null) return;

            for (int i = 0; i < gridTransform.childCount; i++)
            {
                Image cellImage = gridTransform.GetChild(i).GetComponent<Image>();
                if (cellImage != null)
                {
                    if (i == 12)
                    {
                        cellImage.color = Color.red;
                    }
                    else
                    {
                        cellImage.color = inactiveCellColor;
                    }
                }
            }
        }

        private void UpdateTooltipPosition()
        {
            if (tooltipRect == null || parentCanvas == null || Camera.main == null) return;

            try
            {
                Vector2 panelSize = tooltipRect.sizeDelta;
                Vector3 screenPosition;

                // Всегда используем позицию триггера (центра иконки)
                screenPosition = Camera.main.WorldToScreenPoint(triggerWorldPosition);

                // Смещаем тултип над иконкой
                screenPosition += new Vector3(offset.x, offset.y, 0);

                if (clampToScreen)
                {
                    float halfWidth = panelSize.x * 0.5f;
                    float halfHeight = panelSize.y * 0.5f;

                    // Центрируем по горизонтали относительно иконки
                    screenPosition.x = Mathf.Clamp(screenPosition.x,
                        halfWidth,
                        Screen.width - halfWidth);

                    screenPosition.y -= (panelSize.y + offset.y + 60f);
                    

                    screenPosition.y = Mathf.Clamp(screenPosition.y,
                        halfHeight,
                        Screen.height - halfHeight);
                }

                UpdateRectTransformPosition(screenPosition);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating ability tooltip position: {e.Message}");
            }
        }

        private void UpdateTooltipPositionForPreparation()
        {
            if (tooltipRect == null || parentCanvas == null || Camera.main == null) return;

            try
            {
                Vector2 panelSize = tooltipRect.sizeDelta;
                Vector3 screenPosition;

                // Всегда используем позицию триггера (центра иконки)
                screenPosition = Camera.main.WorldToScreenPoint(triggerWorldPosition);

                // Смещаем тултип над иконкой
                screenPosition += new Vector3(offset.x, offset.y, 0);

                if (clampToScreen)
                {
                    float halfWidth = panelSize.x * 0.5f;
                    float halfHeight = panelSize.y * 0.5f;

                    // Центрируем по горизонтали относительно иконки
                    screenPosition.x = Mathf.Clamp(screenPosition.x,
                        halfWidth,
                        Screen.width - halfWidth) + 743f +80f + 100f+20f+17f;

                    screenPosition.y -= (panelSize.y + offset.y + 60f);


                    screenPosition.y = Mathf.Clamp(screenPosition.y,
                        halfHeight,
                        Screen.height - halfHeight)+300f+200f + 100f+260f;
                }

                UpdateRectTransformPosition(screenPosition);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating ability tooltip position: {e.Message}");
            }
        }

        private void UpdateRectTransformPosition(Vector3 screenPosition)
        {
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

        public void HideTooltip()
        {
            if (!isShowing) return;

            isShowing = false;
            isCombinedView = false;
            abilityTooltipPanel.SetActive(false);
            currentPartData = null;
            pairedPartData = null;
            ClearShiftGrid();
        }

        public bool IsTooltipShowing()
        {
            return isShowing;
        }

        public bool IsCombinedTooltip()
        {
            return isShowing && isCombinedView;
        }

        // Методы для получения данных (опциональны, можно удалить если не нужно)
        private BodyPartData GetAbilityData(string abilityId)
        {
            if (string.IsNullOrEmpty(abilityId)) return null;

            if (cachedPartsData.TryGetValue(abilityId, out BodyPartData cachedData))
            {
                return cachedData;
            }

            BodyPartData data = new BodyPartData();

            if (DataBase.TryFindRecordByID(abilityId, out var e_record))
            {
                if (DataBase.TryGetRecord<Name>(e_record, out var e_abt_name))
                {
                    data.ability_name = e_abt_name.m_Value;
                }
                if (DataBase.TryGetRecord<Description>(e_record, out var e_abt_desc))
                {
                    data.ability_desc = e_abt_desc.m_Value;
                }
                if (DataBase.TryGetRecord<IconUI>(e_record, out var e_abt_icon))
                {
                    data.ability_icon = e_abt_icon.m_Value;
                }
                if (DataBase.TryGetRecord<AbilityDefenition>(e_record, out var e_abt_definitions))
                {
                    data.ability_shifts = e_abt_definitions.m_Shifts;
                }
            }

            if (data.ability_name != null)
            {
                cachedPartsData[abilityId] = data;
                return data;
            }

            return null;
        }

        private BodyPartData GetBodyPartData(string partId)
        {
            if (string.IsNullOrEmpty(partId)) return null;

            if (cachedPartsData.TryGetValue(partId, out BodyPartData cachedData))
            {
                return cachedData;
            }

            BodyPartData data = null;

            if (DataBase.TryFindRecordByID(partId, out var e_record))
            {
                data = new BodyPartData();
                data.db_id = partId;

                if (DataBase.TryGetRecord<TagHead>(e_record, out var e_tmh))
                {
                    data.type = BODYPART_TYPE.HEAD;
                }
                if (DataBase.TryGetRecord<TagTorso>(e_record, out var e_tmt))
                {
                    data.type = BODYPART_TYPE.TORSO;
                }
                if (DataBase.TryGetRecord<TagArm>(e_record, out var e_tma))
                {
                    data.type = BODYPART_TYPE.ARM;
                }
                if (DataBase.TryGetRecord<TagLeg>(e_record, out var e_tml))
                {
                    data.type = BODYPART_TYPE.LEG;
                }

                if (DataBase.TryGetRecord<AbilityProvider>(e_record, out var e_abt_id))
                {
                    string abt_id = e_abt_id.m_AbilityTemplateID;

                    if (DataBase.TryFindRecordByID(abt_id, out var e_abt_record))
                    {
                        if (DataBase.TryGetRecord<Name>(e_abt_record, out var e_abt_name))
                        {
                            data.ability_name = e_abt_name.m_Value;
                        }
                        if (DataBase.TryGetRecord<Description>(e_abt_record, out var e_abt_desc))
                        {
                            data.ability_desc = e_abt_desc.m_Value;
                        }
                        if (DataBase.TryGetRecord<IconUI>(e_abt_record, out var e_abt_icon))
                        {
                            data.ability_icon = e_abt_icon.m_Value;
                        }
                        if (DataBase.TryGetRecord<AbilityDefenition>(e_abt_record, out var e_abt_definitions))
                        {
                            data.ability_shifts = e_abt_definitions.m_Shifts;
                        }
                    }
                }
            }

            if (data != null)
            {
                cachedPartsData[partId] = data;
            }

            return data;
        }

        private bool IsInitialized()
        {
            return abilityTooltipPanel != null && tooltipRect != null;
        }

        public void ClearCache()
        {
            cachedPartsData.Clear();
        }

        void OnDestroy()
        {
            ClearCache();
        }

        // Статический метод для удобного доступа (опционально)
        private static AbilityTooltipController instance;
        public static AbilityTooltipController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AbilityTooltipController>();
                }
                return instance;
            }
        }
    }
}