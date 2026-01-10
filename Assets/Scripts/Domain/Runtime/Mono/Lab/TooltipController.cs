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
        [Header("UI Components")]
        public GameObject tooltipPanel;
        public TMP_Text partNameText;
        public TMP_Text hpText;
        public TMP_Text speedText;
        public TMP_Text abilityText;
        public Image abilityIcon;

        public Image bleedResistanceIcon;
        public Image poisonResistanceIcon;
        public Image fireResistanceIcon;

        [Header("Settings")]
        [HideInInspector] public Vector2 offset = new Vector2(0, 20);

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

        public GameObject BodyPartBack;
        public GameObject BodyPartBack2;
        public GridLayoutGroup shiftsGrid;
        public Image shiftCellPrefab;
        public GameObject abilitySection;

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

            UpdateTextFields(data);
            UpdateAbilityUI(data);
            UpdateResistanceIcons(data);

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

        private void UpdateTextFields(BodyPartData data)
        {
            partNameText.text = LocalizationManager.Instance.GetLocalizedValue(data.partName, "Parts") ?? "Неизвестная часть";
            hpText.text = FormatHP(data);
            speedText.text = FormatSpeed(data);
        }

        private void UpdateResistanceIcons(BodyPartData data)
        {
            SetResistanceSprite(bleedResistanceIcon, GetBleedSprite(data.res_bleed));
            SetResistanceSprite(poisonResistanceIcon, GetPoisonSprite(data.res_poison));
            SetResistanceSprite(fireResistanceIcon, GetFireSprite(data.res_fire));
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



            float tmp_offset = 240f;

            if (currentData.type == BODYPART_TYPE.TORSO)
            {
                tmp_offset = 70f;
            }

            screenPosition.y += isInUpperThird ? -tmp_offset : tmp_offset;



            float leftBoundary = panelSize.x * 0.5f + 20f;
            if (screenPosition.x < leftBoundary)
            {
                screenPosition.x = leftBoundary + 70f;
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

            // Активируем соответствующий фон
            if (BodyPartBack != null)
                BodyPartBack.SetActive(!hasAbility);

            if (BodyPartBack2 != null)
                BodyPartBack2.SetActive(hasAbility);

            // Скрываем/показываем секцию навыка
            if (abilitySection != null)
                abilitySection.SetActive(hasAbility);

            // Обновляем иконку если есть
            if (hasAbility && data.ability_icon != null)
            {
                abilityIcon.sprite = data.ability_icon;
            }

            // Обновляем текст навыка если есть
            if (hasAbility && abilityText != null)
            {
                abilityText.text = FormatAbility(data);
            }
            else if (abilityText != null)
            {
                abilityText.text = "";
            }

            // Обновляем сетку shifts если есть
            if (hasAbility && shiftsGrid != null)
            {
                UpdateShiftGrid(data.ability_shifts);
            }
            else
            {
                ClearShiftGrid();
            }
        }

        private string FormatAbility(BodyPartData data)
        {
            string abilityInfo = $"<b>{LocalizationManager.Instance.GetLocalizedValue("AbilityMovement_Name", "Parts")}</b>\n";

            if (data.type == BODYPART_TYPE.LEG)
            {
                return abilityInfo;
            }

            abilityInfo = $"<b>{LocalizationManager.Instance.GetLocalizedValue(data.ability_name, "Parts")}</b>\n";

            if (!string.IsNullOrEmpty(data.ability_desc))
            {
                abilityInfo += $"{LocalizationManager.Instance.GetLocalizedValue(data.ability_desc, "Parts")}";
            }

            return abilityInfo;
        }

        private void UpdateShiftGrid(Vector2Int[] ability_shifts)
        {
            ClearShiftGrid();

            foreach (Vector2Int shift in ability_shifts)
            {
                if (shift == new Vector2Int(-2, 2))
                {
                    ActivateShift(0);
                }
                if (shift == new Vector2Int(-1, 2))
                {
                    ActivateShift(1);
                }
                if (shift == new Vector2Int(0, 2))
                {
                    ActivateShift(2);
                }
                if (shift == new Vector2Int(1, 2))
                {
                    ActivateShift(3);
                }
                if (shift == new Vector2Int(2, 2))
                {
                    ActivateShift(4);
                }

                if (shift == new Vector2Int(-2, 1))
                {
                    ActivateShift(5);
                }
                if (shift == new Vector2Int(-1, 1))
                {
                    ActivateShift(6);
                }
                if (shift == new Vector2Int(0, 1))
                {
                    ActivateShift(7);
                }
                if (shift == new Vector2Int(1, 1))
                {
                    ActivateShift(8);
                }
                if (shift == new Vector2Int(2, 1))
                {
                    ActivateShift(9);
                }

                if (shift == new Vector2Int(-2, 0))
                {
                    ActivateShift(10);
                }
                if (shift == new Vector2Int(-1, 0))
                {
                    ActivateShift(11);
                }
                if (shift == new Vector2Int(0, 0))
                {
                    ActivateShift(12);
                }
                if (shift == new Vector2Int(1, 0))
                {
                    ActivateShift(13);
                }
                if (shift == new Vector2Int(2, 0))
                {
                    ActivateShift(14);
                }

                if (shift == new Vector2Int(-2, -1))
                {
                    ActivateShift(15);
                }
                if (shift == new Vector2Int(-1, -1))
                {
                    ActivateShift(16);
                }
                if (shift == new Vector2Int(0, -1))
                {
                    ActivateShift(17);
                }
                if (shift == new Vector2Int(1, -1))
                {
                    ActivateShift(18);
                }
                if (shift == new Vector2Int(2, -1))
                {
                    ActivateShift(19);
                }

                if (shift == new Vector2Int(-2, -2))
                {
                    ActivateShift(20);
                }
                if (shift == new Vector2Int(-1, -2))
                {
                    ActivateShift(21);
                }
                if (shift == new Vector2Int(0, -2))
                {
                    ActivateShift(22);
                }
                if (shift == new Vector2Int(1, -2))
                {
                    ActivateShift(23);
                }
                if (shift == new Vector2Int(2, -2))
                {
                    ActivateShift(24);
                }


            }
        }

        private void ActivateShift(int ind)
        {
            gridTransform.GetChild(ind).GetComponent<Image>().color = Color.red;
        }


        private void ClearShiftGrid()
        {
            for (int i = 0; i < gridTransform.childCount; i++)
            {
                //gridTransform.GetChild(i).gameObject.SetActive(false);
                gridTransform.GetChild(i).GetComponent<Image>().color = Color.green;
            }
        }
    }
}