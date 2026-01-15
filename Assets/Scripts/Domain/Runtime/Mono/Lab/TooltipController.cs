using Domain.Map;
using Domain.Stats.Components;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class TooltipController : MonoBehaviour
    {
        [Header("General")]
        public GameObject tooltipPanel;
        public GameObject generalPanel;
        public TMP_Text nameText;
        public TMP_Text hpText;
        public TMP_Text speedText;
        public Image bleedResistanceIcon;
        public Image poisonResistanceIcon;
        public Image fireResistanceIcon;

        [Header("Ability")]
        public GameObject abilityPanel;
        public TMP_Text abilityNameText;
        public TMP_Text abilityDescText;
        public Image abilityIcon;

        public TMP_Text abilityPanel_nameText;
        public TMP_Text abilityPanel_hpText;
        public TMP_Text abilityPanel_speedText;
        public Image abilityPanel_bleedResistanceIcon;
        public Image abilityPanel_poisonResistanceIcon;
        public Image abilityPanel_fireResistanceIcon;

        public GridLayoutGroup shiftsGrid;
        public GameObject abilitySection;

        [Header("Settings")]
        [HideInInspector] public Vector2 offset = new Vector2(0, 20);

        public Color activeCellColor = Color.red;
        public Color inactiveCellColor = Color.green;
        public Color centerCellColor = new Color(1f, 0.5f, 0.8f);

        private LabReferences labRef;
        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private BodyPartData currentData;
        private Vector3 slotWorldPosition;

        [SerializeField] private Sprite bleedImmunedSprite;
        [SerializeField] private Sprite bleedNoneSprite;
        [SerializeField] private Sprite bleedResistantSprite;

        [SerializeField] private Sprite poisonImmunedSprite;
        [SerializeField] private Sprite poisonNoneSprite;
        [SerializeField] private Sprite poisonResistantSprite;

        [SerializeField] private Sprite fireImmunedSprite;
        [SerializeField] private Sprite fireNoneSprite;
        [SerializeField] private Sprite fireResistantSprite;

        private List<Image> shiftCells = new List<Image>();
        private bool isShowing = false;
        private Transform gridTransform;

        private void Awake()
        {
            labRef = LabReferences.Instance();
            gridTransform = shiftsGrid.transform;

            tooltipRect = tooltipPanel.GetComponent<RectTransform>();
            parentCanvas = GetComponentInParent<Canvas>();
            tooltipPanel.SetActive(false);

            ClearShiftGrid();
        }

        private void Update()
        {
            if (isShowing)
            {
                UpdateTooltipPosition();
            }
        }

        public void ShowTooltip(BodyPartData data, Vector3 worldPosition)
        {
            if (data == null || isShowing) return;

            currentData = data;
            slotWorldPosition = worldPosition;
            isShowing = true;

            UpdateGeneralPart(data, nameText, hpText, speedText, bleedResistanceIcon, poisonResistanceIcon, fireResistanceIcon);
            UpdateAbilityUI(data);

            tooltipPanel.SetActive(true);
            UpdateTooltipPosition();
        }

        public void HideTooltip()
        {
            if (!isShowing) return;

            isShowing = false;
            tooltipPanel.SetActive(false);
            currentData = null;
            ClearShiftGrid();
        }

        private void UpdateGeneralPart(BodyPartData data, TMP_Text bodyText, TMP_Text healthText, TMP_Text spdText, Image bleedIcon, Image poisonIcon, Image fireIcon)
        {
            bodyText.text = LocalizationManager.Instance.GetLocalizedValue(data.partName, "Parts") ?? "Неизвестная часть";
            healthText.text = FormatHP(data);
            spdText.text = FormatSpeed(data);
            SetResistanceSprite(bleedIcon, GetBleedSprite(data.res_bleed));
            SetResistanceSprite(poisonIcon, GetPoisonSprite(data.res_poison));
            SetResistanceSprite(fireIcon, GetFireSprite(data.res_fire));
        }

        private void SetResistanceSprite(Image image, Sprite sprite)
        {
            if (image == null) return;
            image.sprite = sprite;
            image.enabled = sprite != null;
        }

        private Sprite GetBleedSprite(IResistanceModiffier.Stage stage)
        {
            return stage switch
            {
                IResistanceModiffier.Stage.IMMUNE => bleedImmunedSprite,
                IResistanceModiffier.Stage.RESISTANT => bleedResistantSprite,
                _ => bleedNoneSprite
            };
        }

        private Sprite GetPoisonSprite(IResistanceModiffier.Stage stage)
        {
            return stage switch
            {
                IResistanceModiffier.Stage.IMMUNE => poisonImmunedSprite,
                IResistanceModiffier.Stage.RESISTANT => poisonResistantSprite,
                _ => poisonNoneSprite
            };
        }

        private Sprite GetFireSprite(IResistanceModiffier.Stage stage)
        {
            return stage switch
            {
                IResistanceModiffier.Stage.IMMUNE => fireImmunedSprite,
                IResistanceModiffier.Stage.RESISTANT => fireResistantSprite,
                _ => fireNoneSprite
            };
        }

        private string FormatHP(BodyPartData data)
        {
            return $"{data.hp_amount}";
        }

        private string FormatSpeed(BodyPartData data)
        {
            return $"{data.speed_amount}";
        }

        private void UpdateTooltipPosition()
        {
            Vector2 panelSize = tooltipRect.sizeDelta;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(slotWorldPosition);

            bool isInUpperThird = screenPosition.y > Screen.height * 2f / 3f;

            screenPosition.y += isInUpperThird ? -180f : 180f;

            float leftBoundary = panelSize.x * 0.5f + 20f;

            if (currentData.type != BODYPART_TYPE.TORSO && screenPosition.x - 300 < leftBoundary)
            {
                screenPosition.x = leftBoundary + 220f;
            }
            else if (screenPosition.x < leftBoundary)
            {
                screenPosition.x = leftBoundary + 50f;
            }
            else
            {
                screenPosition.x += offset.x;
            }

            float halfWidth = panelSize.x * 0.5f;
            float halfHeight = panelSize.y * 0.5f;

            screenPosition.x = Mathf.Clamp(screenPosition.x, halfWidth + 10f, Screen.width - halfWidth - 10f);
            screenPosition.y = Mathf.Clamp(screenPosition.y, halfHeight + 10f, Screen.height - halfHeight - 10f);

            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.GetComponent<RectTransform>(),
                screenPosition,
                parentCanvas.worldCamera,
                out localPoint);

            tooltipRect.localPosition = localPoint;
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);
        }

        private void UpdateAbilityUI(BodyPartData data)
        {
            bool hasAbility = (data.type != BODYPART_TYPE.TORSO);

            generalPanel.SetActive(!hasAbility);

            abilityPanel.SetActive(hasAbility);

            UpdateGeneralPart(data, abilityPanel_nameText, abilityPanel_hpText, abilityPanel_speedText, abilityPanel_bleedResistanceIcon, abilityPanel_poisonResistanceIcon, abilityPanel_fireResistanceIcon);

            if (hasAbility && data.ability_icon != null)
            {
                abilityIcon.sprite = data.ability_icon;
            }

            if (hasAbility && abilityNameText != null)
            {
                FormatAbility(data);
            }
            else if (abilityNameText != null)
            {
                abilityNameText.text = "";
            }

            if (hasAbility && shiftsGrid != null)
            {
                UpdateShiftGrid(data.ability_shifts);
            }
            else
            {
                ClearShiftGrid();
            }
        }

        private void FormatAbility(BodyPartData data)
        {
            if (data.type == BODYPART_TYPE.LEG)
            {
                abilityNameText.text = $"<b>{LocalizationManager.Instance.GetLocalizedValue("AbilityMovement_Name", "Parts")}</b>\n";
            }
            else
            {
                abilityNameText.text = $"<b>{LocalizationManager.Instance.GetLocalizedValue(data.ability_name, "Parts")}</b>\n";

                abilityDescText.text = $"{LocalizationManager.Instance.GetLocalizedValue(data.ability_desc, "Parts")}";
            }

        }
        private void UpdateShiftGrid(Vector2Int[] ability_shifts)
        {
            ClearShiftGrid();

            foreach (Vector2Int shift in ability_shifts)
            {
                int index = -1;

                // Определяем индекс клетки на основе координат
                if (shift == new Vector2Int(-2, 2)) index = 0;
                if (shift == new Vector2Int(-1, 2)) index = 1;
                if (shift == new Vector2Int(0, 2)) index = 2;
                if (shift == new Vector2Int(1, 2)) index = 3;
                if (shift == new Vector2Int(2, 2)) index = 4;

                if (shift == new Vector2Int(-2, 1)) index = 5;
                if (shift == new Vector2Int(-1, 1)) index = 6;
                if (shift == new Vector2Int(0, 1)) index = 7;
                if (shift == new Vector2Int(1, 1)) index = 8;
                if (shift == new Vector2Int(2, 1)) index = 9;

                if (shift == new Vector2Int(-2, 0)) index = 10;
                if (shift == new Vector2Int(-1, 0)) index = 11;
                if (shift == new Vector2Int(0, 0)) index = 12; // Центральная клетка
                if (shift == new Vector2Int(1, 0)) index = 13;
                if (shift == new Vector2Int(2, 0)) index = 14;

                if (shift == new Vector2Int(-2, -1)) index = 15;
                if (shift == new Vector2Int(-1, -1)) index = 16;
                if (shift == new Vector2Int(0, -1)) index = 17;
                if (shift == new Vector2Int(1, -1)) index = 18;
                if (shift == new Vector2Int(2, -1)) index = 19;

                if (shift == new Vector2Int(-2, -2)) index = 20;
                if (shift == new Vector2Int(-1, -2)) index = 21;
                if (shift == new Vector2Int(0, -2)) index = 22;
                if (shift == new Vector2Int(1, -2)) index = 23;
                if (shift == new Vector2Int(2, -2)) index = 24;

                if (index >= 0)
                {
                    // Проверяем, является ли это центральной клеткой (0,0)
                    if (shift == Vector2Int.zero)
                    {
                        // Центральная клетка - розовый цвет
                        gridTransform.GetChild(index).GetComponent<Image>().color = centerCellColor;
                    }
                    else
                    {
                        // Остальные клетки - красный цвет
                        gridTransform.GetChild(index).GetComponent<Image>().color = activeCellColor;
                    }
                }
            }
        }


        private void ClearShiftGrid()
        {
            for (int i = 0; i < gridTransform.childCount; i++)
            {
                //gridTransform.GetChild(i).gameObject.SetActive(false);
                if (i == 12)
                {
                    gridTransform.GetChild(i).GetComponent<Image>().color = Color.red;
                }
                else
                {
                    gridTransform.GetChild(i).GetComponent<Image>().color = inactiveCellColor; // Зеленый
                }
            }
        }
    }
}