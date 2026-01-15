using Domain.Map;
using Domain.Monster.Mono;
using Domain.Stats.Components;
using Persistence.Components;
using Persistence.DB;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class CraftingMonsterTooltipController : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject craftingTooltipPanel;
        public TMP_Text hpText;
        public TMP_Text speedText;
        public Image fireResistanceIcon;
        public Image bleedResistanceIcon;
        public Image poisonResistanceIcon;

        [Header("Ability Sections")]
        public Image headAbilityIcon;
        public Image leftArmAbilityIcon;
        public Image rightArmAbilityIcon;
        public Image legAbilityIcon;

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

        private LabMonsterCraftController craftController;
        private Dictionary<string, BodyPartData> cachedPartsData = new Dictionary<string, BodyPartData>();

        private void Start()
        {
            craftController = FindObjectOfType<LabMonsterCraftController>();

            if (craftController == null)
            {
                return;
            }

            if (craftingTooltipPanel != null)
            {
                craftingTooltipPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (craftController == null)
            {
                return;
            }

            bool hasAnyPart = craftController.HasAnyPartInCraftSlots();

            if (hasAnyPart)
            {
                if (!craftingTooltipPanel.activeSelf)
                {
                    craftingTooltipPanel.SetActive(true);
                }

                MonsterData monsterData = craftController.GetMonsterDataForCraftingTooltip();
                if (monsterData != null)
                {
                    UpdateMonsterInfo(monsterData);
                }
            }
            else
            {
                if (craftingTooltipPanel.activeSelf)
                {
                    craftingTooltipPanel.SetActive(false);
                }

                ResetAbilityIcons();
            }
        }

        private void UpdateMonsterInfo(MonsterData data)
        {
            ResetAbilityIcons();

            int totalHP = 0;
            int totalSpeed = 0;

            BodyPartData headData = GetBodyPartData(data.Head_id);
            BodyPartData torsoData = GetBodyPartData(data.Body_id);
            BodyPartData leftArmData = GetBodyPartData(data.NearArm_id);
            BodyPartData rightArmData = GetBodyPartData(data.FarArm_id);
            BodyPartData leftLegData = GetBodyPartData(data.NearLeg_id);
            BodyPartData rightLegData = GetBodyPartData(data.FarLeg_id);

            // Голова
            if (headData != null)
            {
                totalHP += headData.hp_amount;
                totalSpeed += headData.speed_amount;

                if (headAbilityIcon != null && headData.ability_icon != null)
                {
                    headAbilityIcon.sprite = headData.ability_icon;
                    headAbilityIcon.enabled = true;

                    UpdateAbilityTrigger(headAbilityIcon.gameObject, headData);
                }
            }

            // Торс
            if (torsoData != null)
            {
                totalHP += torsoData.hp_amount;
                totalSpeed += torsoData.speed_amount;
            }

            // Левая рука
            if (leftArmData != null)
            {
                totalHP += leftArmData.hp_amount;
                totalSpeed += leftArmData.speed_amount;

                if (leftArmAbilityIcon != null && leftArmData.ability_icon != null)
                {
                    leftArmAbilityIcon.sprite = leftArmData.ability_icon;
                    leftArmAbilityIcon.enabled = true;

                    UpdateAbilityTrigger(leftArmAbilityIcon.gameObject, leftArmData);
                }
            }

            // Правая рука
            if (rightArmData != null)
            {
                totalHP += rightArmData.hp_amount;
                totalSpeed += rightArmData.speed_amount;

                if (rightArmAbilityIcon != null && rightArmData.ability_icon != null)
                {
                    rightArmAbilityIcon.sprite = rightArmData.ability_icon;
                    rightArmAbilityIcon.enabled = true;

                    UpdateAbilityTrigger(rightArmAbilityIcon.gameObject, rightArmData);
                }
            }

            // Ноги
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

            // Обновляем тексты
            if (hpText != null) hpText.text = $"{totalHP}";
            if (speedText != null) speedText.text = $"{totalSpeed}";

            // Обновляем сопротивления
            UpdateResistanceIcons(data);
        }

        private void UpdateAbilityTrigger(GameObject iconObject, BodyPartData partData)
        {
            if (iconObject == null || partData == null) return;

            var trigger = iconObject.GetComponent<AbilityTooltipTrigger>();
            if (trigger != null)
            {
                trigger.UpdateRuntimeBodyPartData(partData);
            }
        }

        private void ResetAbilityIcons()
        {
            if (headAbilityIcon != null)
            {
                headAbilityIcon.enabled = false;
                headAbilityIcon.sprite = null;
            }

            if (leftArmAbilityIcon != null)
            {
                leftArmAbilityIcon.enabled = false;
                leftArmAbilityIcon.sprite = null;
            }

            if (rightArmAbilityIcon != null)
            {
                rightArmAbilityIcon.enabled = false;
                rightArmAbilityIcon.sprite = null;
            }

            if (legAbilityIcon != null)
            {
                legAbilityIcon.enabled = false;
                legAbilityIcon.sprite = null;
            }
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
                        if (Persistence.DB.DataBase.TryGetRecord<Description>(e_abt_record, out var e_abt_desc))
                        {
                            data.ability_desc = e_abt_desc.m_Value;
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

        public void ForceUpdate()
        {
            if (craftController != null && craftController.HasAnyPartInCraftSlots())
            {
                MonsterData monsterData = craftController.GetMonsterDataForCraftingTooltip();
                if (monsterData != null)
                {
                    UpdateMonsterInfo(monsterData);
                }
            }
        }

        public void HideTooltip()
        {
            if (craftingTooltipPanel != null)
                craftingTooltipPanel.SetActive(false);
        }

        public void ClearCache()
        {
            cachedPartsData.Clear();
        }

        void OnDestroy()
        {
            ClearCache();
        }
    }
}