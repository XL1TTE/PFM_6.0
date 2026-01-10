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
            Transform gridTransform = shiftsGrid.transform;

            tooltipRect = tooltipPanel.GetComponent<RectTransform>();
            parentCanvas = GetComponentInParent<Canvas>();
            tooltipPanel.SetActive(false);

            //ClearShiftGrid();
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
            //ClearShiftGrid();
        }

        private void UpdateTextFields(BodyPartData data)
        {
            partNameText.text = data.partName ?? "Неизвестная часть";
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
            screenPosition.y += isInUpperThird ? -150f : 150f;

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
            bool hasAbility = !string.IsNullOrEmpty(data.ability_name);

            // Активируем соответствующий фон
            if (BodyPartBack != null)
                BodyPartBack.SetActive(!hasAbility);

            if (BodyPartBack2 != null)
                BodyPartBack2.SetActive(hasAbility);

            // Скрываем/показываем секцию навыка
            if (abilitySection != null)
                abilitySection.SetActive(hasAbility);

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
            //if (hasAbility && shiftsGrid != null)
            //{
            //    UpdateShiftGrid(data.ability_shifts);
            //}
            //else
            //{
            //    ClearShiftGrid();
            //}
        }

        private string FormatAbility(BodyPartData data)
        {
            string abilityInfo = $"<b>{data.ability_name}</b>\n";

            if (!string.IsNullOrEmpty(data.ability_desc))
            {
                abilityInfo += $"{data.ability_desc}";
            }

            return abilityInfo;
        }

        private void UpdateShiftGrid(Vector2Int[] ability_shifts)
        {
            //ClearShiftGrid();

            if (shiftsGrid == null || shiftCellPrefab == null) return;

            foreach (Vector2Int shift in ability_shifts)
            {
                if (shift == new Vector2Int(-2, 2))
                {
                    gridTransform.GetChild(0).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(-1, 2))
                {
                    gridTransform.GetChild(1).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(0, 2))
                {
                    gridTransform.GetChild(2).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(1, 2))
                {
                    gridTransform.GetChild(3).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(2, 2))
                {
                    gridTransform.GetChild(4).gameObject.SetActive(true);
                }

                if (shift == new Vector2Int(-2, 1))
                {
                    gridTransform.GetChild(5).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(-1, 1))
                {
                    gridTransform.GetChild(6).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(0, 1))
                {
                    gridTransform.GetChild(7).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(1, 1))
                {
                    gridTransform.GetChild(8).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(2, 1))
                {
                    gridTransform.GetChild(9).gameObject.SetActive(true);
                }

                if (shift == new Vector2Int(-2, 0))
                {
                    gridTransform.GetChild(10).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(-1, 0))
                {
                    gridTransform.GetChild(11).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(0, 0))
                {
                    gridTransform.GetChild(12).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(1, 0))
                {
                    gridTransform.GetChild(13).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(2, 0))
                {
                    gridTransform.GetChild(14).gameObject.SetActive(true);
                }

                if (shift == new Vector2Int(-2, -1))
                {
                    gridTransform.GetChild(15).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(-1, -1))
                {
                    gridTransform.GetChild(16).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(0, -1))
                {
                    gridTransform.GetChild(17).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(1, -1))
                {
                    gridTransform.GetChild(18).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(2, -1))
                {
                    gridTransform.GetChild(19).gameObject.SetActive(true);
                }

                if (shift == new Vector2Int(-2, -2))
                {
                    gridTransform.GetChild(20).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(-1, -2))
                {
                    gridTransform.GetChild(21).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(0, -2))
                {
                    gridTransform.GetChild(22).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(1, -2))
                {
                    gridTransform.GetChild(23).gameObject.SetActive(true);
                }
                if (shift == new Vector2Int(2, -2))
                {
                    gridTransform.GetChild(24).gameObject.SetActive(true);
                }


            }
        }

        //private void ClearShiftGrid()
        //{
        //    for (int i = 0; i < gridTransform.childCount; i++)
        //    {
        //        gridTransform.GetChild(0).gameObject.SetActive(false);
        //    }
        //}
    }
}