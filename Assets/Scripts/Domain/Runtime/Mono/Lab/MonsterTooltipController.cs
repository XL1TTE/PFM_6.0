using Domain.Components;
using Domain.Map;
using Domain.Monster.Mono;
using Domain.Stats.Components;
using Game;
using Persistence.Buiders;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Project
{
    public class MonsterTooltipController : MonoBehaviour, IPointerClickHandler
    {
        [Header("UI Components")]
        public GameObject monsterTooltipPanel;
        public TMP_Text monsterNameText;
        public TMP_Text hpText;
        public TMP_Text speedText;
        public Image fireResistanceIcon;
        public Image bleedResistanceIcon;
        public Image poisonResistanceIcon;

        [Header("Ability Icons")]
        public Image headAbilityIcon;
        public Image leftArmAbilityIcon;
        public Image rightArmAbilityIcon;
        public Image legAbilityIcon;

        [Header("Monster Preview")]
        public Transform monsterPreviewContainer;
        public float monsterPreviewScale = 0.8f;

        [Header("Settings")]
        [HideInInspector] public Vector2 offset = new Vector2(0, 20);
        public bool isFixedByClick = false;

        [Header("Resistance Sprites")]
        [SerializeField] private Sprite bleedImmunedSprite;
        [SerializeField] private Sprite bleedNoneSprite;
        [SerializeField] private Sprite bleedResistantSprite;

        [SerializeField] private Sprite fireImmunedSprite;
        [SerializeField] private Sprite fireNoneSprite;
        [SerializeField] private Sprite fireResistantSprite;

        [SerializeField] private Sprite poisonImmunedSprite;
        [SerializeField] private Sprite poisonNoneSprite;
        [SerializeField] private Sprite poisonResistantSprite;

        private LabReferences labRef;
        private RectTransform tooltipRect;
        private Canvas parentCanvas;
        private MonsterData currentMonsterData;
        private Vector3 slotWorldPosition;
        private bool isShowing = false;
        private bool isInitialized = false;
        private bool isFixed = false;
        private GameObject currentMonsterPreview;
        private GraphicRaycaster graphicRaycaster;

        private Dictionary<string, BodyPartData> cachedPartsData = new Dictionary<string, BodyPartData>();

        private Vector2 lastTooltipPosition;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            labRef = LabReferences.Instance();
            graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        }

        public bool IsTooltipShowing()
        {
            return isShowing;
        }

        public bool IsTooltipFixed()
        {
            return isFixed;
        }

        public void ToggleFix()
        {
            if (isShowing)
            {
                isFixed = !isFixed;
                Debug.Log($"Tooltip {(isFixed ? "fixed" : "unfixed")}");
            }
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

                // Добавляем компонент для обработки кликов на тултип
                if (monsterTooltipPanel.GetComponent<PointerClickHandler>() == null)
                {
                    monsterTooltipPanel.AddComponent<PointerClickHandler>();
                }

                isInitialized = true;
            }
        }

        private void Update()
        {
            if (isShowing)
            {
                //// Обновляем позицию только если тултип НЕ зафиксирован
                //if (!isFixed)
                //{
                //    UpdateTooltipPosition();
                //}

                // Проверяем клик вне тултипа, если он зафиксирован
                if (isFixed && Input.GetMouseButtonDown(0))
                {
                    CheckClickOutsideTooltip();
                }
            }
        }

        public void ShowMonsterTooltip(MonsterData monsterData, Vector3 worldPosition, bool isPreparationScreen = false)
        {
            if (monsterData == null || isShowing) return;

            if (!ShouldShowTooltip()) return;

            if (!isInitialized) Initialize();

            if (monsterTooltipPanel == null || tooltipRect == null || parentCanvas == null) return;

            currentMonsterData = monsterData;
            slotWorldPosition = worldPosition;
            isShowing = true;
            isFixed = false; // Сбрасываем фиксацию при новом показе

            UpdateMonsterInfo(monsterData);
            UpdateResistanceIcons(monsterData);
            UpdateAbilityIcons(monsterData);

            // Создаем превью монстра
            CreateMonsterPreview(monsterData);

            monsterTooltipPanel.SetActive(true);

            Debug.Log($"Showing tooltip for {monsterData.m_MonsterName}. isPreparationScreen: {isPreparationScreen}, World Position: {worldPosition}");

            if (isPreparationScreen)
            {
                Debug.Log("Using preparation screen positioning");
                UpdateTooltipPositionForPreparation();
            }
            else
            {
                Debug.Log("Using main screen positioning");
                UpdateTooltipPosition();
            }

            lastTooltipPosition = tooltipRect.anchoredPosition;
        }

        private void UpdateTooltipPositionForPreparation()
        {
            if (tooltipRect == null || parentCanvas == null || Camera.main == null) return;

            try
            {
                if (!isFixed)
                {
                    Debug.Log($"=== PREPARATION TOOLTIP POSITIONING ===");

                    // Получаем позицию иконки в мировых координатах
                    Vector3 iconWorldPosition = slotWorldPosition;

                    // Конвертируем в экранные координаты
                    Vector3 iconScreenPosition = Camera.main.WorldToScreenPoint(iconWorldPosition);

                    // Получаем размеры тултипа
                    Vector2 tooltipSize = tooltipRect.rect.size;

                    // Позиционируем тултип СНИЗУ от иконки
                    // Смещение по Y: отнимаем высоту тултипа плюс небольшой отступ
                    Vector3 screenPosition = new Vector3(
                        iconScreenPosition.x,
                        iconScreenPosition.y - tooltipSize.y + 40f, // 20f - отступ снизу
                        iconScreenPosition.z
                    );

                    // Если тултип не помещается снизу (выходит за нижнюю границу экрана),
                    // показываем его сверху
                    if (screenPosition.y - 100f < tooltipSize.y)
                    {
                        screenPosition.y = iconScreenPosition.y + 380f; // Показываем сверху
                    }

                    // Конвертируем экранные координаты в локальные координаты канваса
                    Vector2 localPoint;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        parentCanvas.GetComponent<RectTransform>(),
                        screenPosition,
                        parentCanvas.worldCamera,
                        out localPoint))
                    {
                        // Устанавливаем anchor в центр для удобного позиционирования
                        tooltipRect.anchorMin = new Vector2(0.5f, 0.5f);
                        tooltipRect.anchorMax = new Vector2(0.5f, 0.5f);
                        tooltipRect.pivot = new Vector2(0.5f, 1f); // Pivot сверху, чтобы тултип "вырастал" вниз

                        tooltipRect.anchoredPosition = localPoint;

                        // Принудительно обновляем Layout
                        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

                        Debug.Log($"Tooltip positioned below icon. Icon screen: {iconScreenPosition}, Tooltip screen: {screenPosition}");
                    }

                    lastTooltipPosition = tooltipRect.anchoredPosition;
                    Debug.Log($"=== END PREPARATION TOOLTIP ===");
                }
                else
                {
                    tooltipRect.anchoredPosition = lastTooltipPosition;

                    KeepTooltipOnScreen();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating preparation monster tooltip position: {e.Message}");
            }
        }

        private void CreateMonsterPreview(MonsterData monsterData)
        {
            // Очищаем предыдущее превью
            ClearMonsterPreview();

            if (monsterPreviewContainer == null) return;

            // Создаем контейнер для превью
            GameObject previewContainer = new GameObject($"MonsterPreview_{monsterData.m_MonsterName}");
            previewContainer.transform.SetParent(monsterPreviewContainer);
            previewContainer.transform.localPosition = Vector3.zero;
            previewContainer.transform.localScale = Vector3.one * monsterPreviewScale;

            // Создаем монстра через MonsterBuilder (как в PreparationMonsterPreviewController)
            try
            {
                var builder = new MonsterBuilder(ECS_Main_Lab.m_labWorld)
                    .AttachHead(monsterData.Head_id)
                    .AttachBody(monsterData.Body_id)
                    .AttachFarArm(monsterData.FarArm_id)
                    .AttachNearArm(monsterData.NearArm_id)
                    .AttachNearLeg(monsterData.NearLeg_id)
                    .AttachFarLeg(monsterData.FarLeg_id);

                var monsterEntity = builder.Build();
                var transformStash = ECS_Main_Lab.m_labWorld.GetStash<TransformRefComponent>();

                if (transformStash.Has(monsterEntity))
                {
                    ref var monsterTransform = ref transformStash.Get(monsterEntity).Value;
                    monsterTransform.position = monsterPreviewContainer.position;
                    monsterTransform.parent = previewContainer.transform;

                    // Настраиваем слой для корректного отображения
                    var spriteRenderers = previewContainer.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var renderer in spriteRenderers)
                    {
                        renderer.sortingLayerName = "UI";
                        renderer.sortingOrder = 1;
                    }
                }

                currentMonsterPreview = previewContainer;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error creating monster preview: {e.Message}");
                Destroy(previewContainer);
            }
        }

        private void ClearMonsterPreview()
        {
            if (currentMonsterPreview != null)
            {
                Destroy(currentMonsterPreview);
                currentMonsterPreview = null;
            }

            // Также очищаем контейнер
            if (monsterPreviewContainer != null)
            {
                foreach (Transform child in monsterPreviewContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void CheckClickOutsideTooltip()
        {
            if (!isFixed) return;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);

            // Проверяем, был ли клик на тултипе или его дочерних элементах
            bool clickedOnTooltip = false;
            foreach (var result in results)
            {
                if (result.gameObject == monsterTooltipPanel ||
                    result.gameObject.transform.IsChildOf(monsterTooltipPanel.transform))
                {
                    clickedOnTooltip = true;
                    break;
                }
            }

            // Если клик был вне тултипа - скрываем его
            if (!clickedOnTooltip)
            {
                isFixed = false;
                HideMonsterTooltip();
            }
        }

        // Обработчик клика по тултипу (для фиксации)
        public void OnPointerClick(PointerEventData eventData)
        {
            eventData.Use();

            if (eventData.button == PointerEventData.InputButton.Left && isShowing)
            {
                // Если включена фиксация по клику, то фиксируем/разфиксируем
                if (isFixedByClick)
                {
                    isFixed = !isFixed;
                    Debug.Log($"Tooltip {(isFixed ? "fixed at position" : "unfixed")}: {tooltipRect.anchoredPosition}");

                    // Сохраняем позицию при фиксации
                    if (isFixed)
                    {
                        lastTooltipPosition = tooltipRect.anchoredPosition;
                    }
                }
            }
        }


        private bool ShouldShowTooltip()
        {
            return true;
        }

        public void HideMonsterTooltip()
        {
            if (!isShowing) return;

            isShowing = false;
            isFixed = false;
            monsterTooltipPanel.SetActive(false);

            // Очищаем превью
            ClearMonsterPreview();

            currentMonsterData = null;
        }

        // Для принудительного скрытия (например, при клике вне тултипа)
        public void ForceHideTooltip()
        {
            HideMonsterTooltip();
        }

        private void UpdateMonsterInfo(MonsterData data)
        {
            monsterNameText.text = GetMonsterName(data);

            int totalHP = 0;
            int totalSpeed = 0;

            BodyPartData headData = GetBodyPartData(data.Head_id);
            BodyPartData torsoData = GetBodyPartData(data.Body_id);
            BodyPartData leftArmData = GetBodyPartData(data.NearArm_id);
            BodyPartData rightArmData = GetBodyPartData(data.FarArm_id);
            BodyPartData leftLegData = GetBodyPartData(data.NearLeg_id);
            BodyPartData rightLegData = GetBodyPartData(data.FarLeg_id);

            if (headData != null)
            {
                totalHP += headData.hp_amount;
                totalSpeed += headData.speed_amount;
            }

            if (torsoData != null)
            {
                totalHP += torsoData.hp_amount;
                totalSpeed += torsoData.speed_amount;
            }

            if (leftArmData != null)
            {
                totalHP += leftArmData.hp_amount;
                totalSpeed += leftArmData.speed_amount;
            }

            if (rightArmData != null)
            {
                totalHP += rightArmData.hp_amount;
                totalSpeed += rightArmData.speed_amount;
            }

            if (leftLegData != null)
            {
                totalHP += leftLegData.hp_amount;
                totalSpeed += leftLegData.speed_amount;
            }

            if (rightLegData != null)
            {
                totalHP += rightLegData.hp_amount;
                totalSpeed += rightLegData.speed_amount;
            }

            hpText.text = $"{totalHP}";
            speedText.text = $"{totalSpeed}";
        }

        private void UpdateResistanceIcons(MonsterData data)
        {
            List<IResistanceModiffier.Stage> bleedStages = new List<IResistanceModiffier.Stage>();
            List<IResistanceModiffier.Stage> poisonStages = new List<IResistanceModiffier.Stage>();
            List<IResistanceModiffier.Stage> fireStages = new List<IResistanceModiffier.Stage>();

            AddResistanceIfExists(data.Head_id, bleedStages, poisonStages, fireStages);
            AddResistanceIfExists(data.Body_id, bleedStages, poisonStages, fireStages);
            AddResistanceIfExists(data.NearArm_id, bleedStages, poisonStages, fireStages);
            AddResistanceIfExists(data.FarArm_id, bleedStages, poisonStages, fireStages);
            AddResistanceIfExists(data.NearLeg_id, bleedStages, poisonStages, fireStages);
            AddResistanceIfExists(data.FarLeg_id, bleedStages, poisonStages, fireStages);

            IResistanceModiffier.Stage finalBleed = GetHighestResistance(bleedStages);
            IResistanceModiffier.Stage finalPoison = GetHighestResistance(poisonStages);
            IResistanceModiffier.Stage finalFire = GetHighestResistance(fireStages);

            SetResistanceSprite(bleedResistanceIcon, GetBleedSprite(finalBleed));
            SetResistanceSprite(poisonResistanceIcon, GetPoisonSprite(finalPoison));
            SetResistanceSprite(fireResistanceIcon, GetFireSprite(finalFire));
        }

        private void AddResistanceIfExists(string partId, List<IResistanceModiffier.Stage> bleedList,
                                          List<IResistanceModiffier.Stage> poisonList, List<IResistanceModiffier.Stage> fireList)
        {
            BodyPartData partData = GetBodyPartData(partId);
            if (partData != null)
            {
                bleedList.Add(partData.res_bleed);
                poisonList.Add(partData.res_poison);
                fireList.Add(partData.res_fire);
            }
        }

        private IResistanceModiffier.Stage GetHighestResistance(List<IResistanceModiffier.Stage> stages)
        {
            if (stages.Count == 0) return IResistanceModiffier.Stage.NONE;

            IResistanceModiffier.Stage highest = IResistanceModiffier.Stage.NONE;
            foreach (var stage in stages)
            {
                if (stage == IResistanceModiffier.Stage.IMMUNE) return IResistanceModiffier.Stage.IMMUNE;
                if (stage == IResistanceModiffier.Stage.RESISTANT) highest = IResistanceModiffier.Stage.RESISTANT;
            }
            return highest;
        }

        private void UpdateAbilityIcons(MonsterData data)
        {
            BodyPartData headData = GetBodyPartData(data.Head_id);
            if (headData != null && headData.ability_icon != null && headAbilityIcon != null)
            {
                headAbilityIcon.sprite = headData.ability_icon;
                headAbilityIcon.enabled = true;

                var trigger = headAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                if (trigger != null)
                {
                    trigger.UpdateRuntimeBodyPartData(headData);
                }
            }
            else if (headAbilityIcon != null)
            {
                headAbilityIcon.enabled = false;
            }

            BodyPartData leftArmData = GetBodyPartData(data.NearArm_id);
            if (leftArmData != null && leftArmData.ability_icon != null && leftArmAbilityIcon != null)
            {
                leftArmAbilityIcon.sprite = leftArmData.ability_icon;
                leftArmAbilityIcon.enabled = true;

                var trigger = leftArmAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                if (trigger != null)
                {
                    trigger.UpdateRuntimeBodyPartData(leftArmData);
                }
            }
            else if (leftArmAbilityIcon != null)
            {
                leftArmAbilityIcon.enabled = false;
            }

            BodyPartData rightArmData = GetBodyPartData(data.FarArm_id);
            if (rightArmData != null && rightArmData.ability_icon != null && rightArmAbilityIcon != null)
            {
                rightArmAbilityIcon.sprite = rightArmData.ability_icon;
                rightArmAbilityIcon.enabled = true;

                var trigger = rightArmAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                if (trigger != null)
                {
                    trigger.UpdateRuntimeBodyPartData(rightArmData);
                }
            }
            else if (rightArmAbilityIcon != null)
            {
                rightArmAbilityIcon.enabled = false;
            }

            BodyPartData leftLegData = GetBodyPartData(data.NearLeg_id);
            BodyPartData rightLegData = GetBodyPartData(data.FarLeg_id);

            if (legAbilityIcon != null)
            {
                if (leftLegData != null && rightLegData != null)
                {
                    // Обе ноги присутствуют
                    if (leftLegData.ability_icon != null)
                    {
                        legAbilityIcon.sprite = leftLegData.ability_icon;
                    }
                    else if (rightLegData.ability_icon != null)
                    {
                        legAbilityIcon.sprite = rightLegData.ability_icon;
                    }
                    legAbilityIcon.enabled = true;

                    // Настраиваем триггер для комбинированных ног
                    var trigger = legAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                    if (trigger == null)
                    {
                        trigger = legAbilityIcon.gameObject.AddComponent<AbilityTooltipTrigger>();
                    }
                    trigger.SetCombinedLegsData(leftLegData, rightLegData);
                }
                else if (leftLegData != null)
                {
                    // Только левая нога
                    if (leftLegData.ability_icon != null)
                    {
                        legAbilityIcon.sprite = leftLegData.ability_icon;
                        legAbilityIcon.enabled = true;

                        var trigger = legAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                        if (trigger == null) trigger = legAbilityIcon.gameObject.AddComponent<AbilityTooltipTrigger>();
                        trigger.UpdateRuntimeBodyPartData(leftLegData);
                    }
                }
                else if (rightLegData != null)
                {
                    // Только правая нога
                    if (rightLegData.ability_icon != null)
                    {
                        legAbilityIcon.sprite = rightLegData.ability_icon;
                        legAbilityIcon.enabled = true;

                        var trigger = legAbilityIcon.GetComponent<AbilityTooltipTrigger>();
                        if (trigger == null) trigger = legAbilityIcon.gameObject.AddComponent<AbilityTooltipTrigger>();
                        trigger.UpdateRuntimeBodyPartData(rightLegData);
                    }
                }
                else
                {
                    legAbilityIcon.enabled = false;
                }
            }
        }

        private BodyPartData GetBodyPartData(string partId)
        {
            if (string.IsNullOrEmpty(partId)) return null;

            if (cachedPartsData.TryGetValue(partId, out BodyPartData cachedData))
            {
                return cachedData;
            }

            BodyPartData data = null;

            if (Persistence.DB.DataBase.TryFindRecordByID(partId, out var e_record))
            {
                data = new BodyPartData();
                data.db_id = partId;

                if (Persistence.DB.DataBase.TryGetRecord<TagHead>(e_record, out var e_tmh))
                {
                    data.type = BODYPART_TYPE.HEAD;
                }
                if (Persistence.DB.DataBase.TryGetRecord<TagTorso>(e_record, out var e_tmt))
                {
                    data.type = BODYPART_TYPE.TORSO;
                }
                if (Persistence.DB.DataBase.TryGetRecord<TagArm>(e_record, out var e_tma))
                {
                    data.type = BODYPART_TYPE.ARM;
                }
                if (Persistence.DB.DataBase.TryGetRecord<TagLeg>(e_record, out var e_tml))
                {
                    data.type = BODYPART_TYPE.LEG;
                }

                if (Persistence.DB.DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                {
                    data.icon = e_icon.m_Value;
                }

                if (Persistence.DB.DataBase.TryGetRecord<Name>(e_record, out var e_name))
                {
                    data.partName = e_name.m_Value;
                }

                if (Persistence.DB.DataBase.TryGetRecord<EffectsProvider>(e_record, out var e_effects_record))
                {
                    if (Persistence.DB.DataBase.TryFindRecordByID(e_effects_record.m_Effects[0], out var e_effect_pointer))
                    {
                        if (Persistence.DB.DataBase.TryGetRecord<MaxHealthModifier>(e_effect_pointer, out var e_effect_hp))
                        {
                            data.hp_amount = e_effect_hp.m_Flat;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<SpeedModifier>(e_effect_pointer, out var e_effect_spd))
                        {
                            data.speed_amount = e_effect_spd.m_Flat;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<BurningResistanceModiffier>(e_effect_pointer, out var e_effect_res_f))
                        {
                            data.res_fire = e_effect_res_f.m_Stage;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<PoisonResistanceModiffier>(e_effect_pointer, out var e_effect_res_p))
                        {
                            data.res_poison = e_effect_res_p.m_Stage;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<BleedResistanceModiffier>(e_effect_pointer, out var e_effect_res_b))
                        {
                            data.res_bleed = e_effect_res_b.m_Stage;
                        }
                    }
                }

                if (Persistence.DB.DataBase.TryGetRecord<AbilityProvider>(e_record, out var e_abt_id))
                {
                    string abt_id = e_abt_id.m_AbilityTemplateID;

                    if (Persistence.DB.DataBase.TryFindRecordByID(abt_id, out var e_abt_record))
                    {
                        if (Persistence.DB.DataBase.TryGetRecord<Name>(e_abt_record, out var e_abt_name))
                        {
                            data.ability_name = e_abt_name.m_Value;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<IconUI>(e_abt_record, out var e_abt_icon))
                        {
                            data.ability_icon = e_abt_icon.m_Value;
                        }
                        if (Persistence.DB.DataBase.TryGetRecord<AbilityDefenition>(e_abt_record, out var e_abt_definitions))
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

        private string GetMonsterName(MonsterData data)
        {
            return !string.IsNullOrEmpty(data.m_MonsterName) ? data.m_MonsterName : "Unnamed Monster";
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

        private void UpdateTooltipPosition()
        {
            if (tooltipRect == null || parentCanvas == null || Camera.main == null) return;

            try
            {
                if (!isFixed)
                {
                    Vector2 panelSize = tooltipRect.sizeDelta;
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(slotWorldPosition);

                    float leftBoundary = panelSize.x * 0.5f + 20f;
                    if (screenPosition.x < leftBoundary)
                    {
                        screenPosition.x = leftBoundary + 70f;
                    }
                    else
                    {
                        screenPosition.x += offset.x;
                    }

                    screenPosition.x -= 500f;

                    float halfWidth = panelSize.x * 0.5f;
                    float halfHeight = panelSize.y * 0.5f;

                    screenPosition.x = Mathf.Clamp(screenPosition.x, halfWidth + 10f, Screen.width - halfWidth - 10f);
                    screenPosition.y = Mathf.Clamp(screenPosition.y, 0f, Screen.height - panelSize.y - 10f);

                    UpdateRectTransformPosition(screenPosition);

                    lastTooltipPosition = tooltipRect.anchoredPosition;
                }
                else
                {
                    tooltipRect.anchoredPosition = lastTooltipPosition;
                    KeepTooltipOnScreen();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error updating monster tooltip position: {e.Message}");
            }
        }

        public void ClearCache()
        {
            cachedPartsData.Clear();
            ClearMonsterPreview();
        }

        void OnDestroy()
        {
            ClearMonsterPreview();
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

        private void KeepTooltipOnScreen()
        {
            // Просто проверяем, что тултип не выходит за экран
            Vector2 anchoredPos = tooltipRect.anchoredPosition;
            Vector2 sizeDelta = tooltipRect.sizeDelta;

            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            float canvasWidth = canvasRect.rect.width;
            float canvasHeight = canvasRect.rect.height;

            // Проверяем границы
            float minX = sizeDelta.x * 0.5f;
            float maxX = canvasWidth - sizeDelta.x * 0.5f;
            float minY = sizeDelta.y * 0.5f;
            float maxY = canvasHeight - sizeDelta.y * 0.5f;

            anchoredPos.x = Mathf.Clamp(anchoredPos.x, minX, maxX);
            anchoredPos.y = Mathf.Clamp(anchoredPos.y, minY, maxY);

            tooltipRect.anchoredPosition = anchoredPos;
        }

    }


    // Вспомогательный класс для обработки кликов на тултипе
    public class PointerClickHandler : MonoBehaviour, IPointerClickHandler
    {
        private MonsterTooltipController parentController;

        void Start()
        {
            parentController = GetComponentInParent<MonsterTooltipController>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (parentController != null)
            {
                parentController.OnPointerClick(eventData);
            }
        }
    }


}