using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Persistence.Components;
using Persistence.DB;
using Persistence.Utilities;
using UI.Elements;
using UI.ToolTip;
using UnityEngine;

namespace Core.Utilities
{
    public static partial class TT
    {
        public const int TEXT_SIZE_H1 = 32;
        public const int TEXT_SIZE_H2 = 28;
        public const int TEXT_SIZE_H3 = 24;
        public const int TEXT_SIZE_H4 = 20;
        public const int TEXT_SIZE_H5 = 18;
        public const int TEXT_SIZE_B1 = 20;
        public const int TEXT_SIZE_B2 = 18;
        public const int TEXT_SIZE_B3 = 16;
        public const int TEXT_SIZE_B4 = 14;

        private static HorizontalLayoutElement CreateHorizontalLine()
        {
            var line = UnityEngine.Object.Instantiate(GR.p_ToolTipLine);
            line.gameObject.SetActive(false);
            return line;
        }

        public static ToolTipLines GetAbilityShortTooltip(Domain.Abilities.Components.AbilityData a_abilityData)
        {
            var a_ability = a_abilityData.m_Value;

            var t_lines = new List<HorizontalLayoutElement>();

            if (DataBase.TryFindRecordByID(a_abilityData.m_AbilityTemplateID, out var abilityRecord))
            {
                if (DataBase.TryGetRecord<Name>(abilityRecord, out var abilityName))
                {
                    HorizontalLayoutElement t_nameLine = CreateHorizontalLine().AlignStart();
                    var name =
                        TextPool.I().WarmupElement()
                        .SetText(abilityName.m_Value)
                        .SetColor(Color.white)
                        .Bold()
                        .FontSize(TEXT_SIZE_H3);

                    t_nameLine.Insert(name);
                    t_lines.Add(t_nameLine);
                }
                if (DataBase.TryGetRecord<Description>(abilityRecord, out var abilityDesc))
                {
                    HorizontalLayoutElement t_descLine = CreateHorizontalLine().AlignStart();
                    var text =
                        TextPool.I().WarmupElement()
                        .SetText(abilityDesc.m_Value)
                        .SetColor(Color.white)
                        .FontSize(TEXT_SIZE_B2);

                    t_descLine.Insert(text);
                    t_lines.Add(t_descLine);
                }
            }

            var phys_dmg = GetDamageForAbility(a_ability, DamageType.PHYSICAL_DAMAGE);
            if (phys_dmg > 0)
            {
                HorizontalLayoutElement t_physDmg = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText($"{phys_dmg} DMG.")
                    .SetColor("#8d781e".ToColor())
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement()
                    .SetIcon(GR.SPR_UI_PHYSICAL_DMG)
                    .MinSize(16);

                t_physDmg.Insert(icon);
                t_physDmg.Insert(text);
                t_lines.Add(t_physDmg);
            }

            foreach (var status in a_ability.GetEffects<ApplyBleeding>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText(GetTextForStatusEffects(status))
                    .SetColor(Color.red)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_EFFECT_BLOOD).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }
            foreach (var status in a_ability.GetEffects<ApplyBurning>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText(GetTextForStatusEffects(status))
                    .SetColor(Color.yellow)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_EFFECT_FIRE).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);

            }
            foreach (var status in a_ability.GetEffects<ApplyPoison>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText(GetTextForStatusEffects(status))
                    .SetColor(Color.green)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_EFFECT_POISON).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }

            foreach (var status in a_ability.GetEffects<ApplyStun>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText($"STUNS FOR {status.m_Duration} TURNS.")
                    .SetColor(Color.white)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_PHYSICAL_DMG).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }
            foreach (var status in a_ability.GetEffects<Heal>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText($"HEALS TARGET FOR {status.m_Amount} HP.")
                    .SetColor(C.COLOR_HEALING)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_PHYSICAL_DMG).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }
            foreach (var status in a_ability.GetEffects<HealInArea>())
            {
                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText($"HEALS ALL TARGET IN RADIUS OF {status.m_Area} FOR {status.m_Amount} HP.")
                    .SetColor(C.COLOR_HEALING)
                    .FontSize(TEXT_SIZE_B2);

                var icon = IconPool.I().WarmupElement().SetIcon(GR.SPR_UI_PHYSICAL_DMG).MinSize(16);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }

            foreach (var effect in a_ability.GetEffects<ApplyToAllAlliesInArea>())
            {
                var t_text = $"For every ally in area of {effect.m_Area}:\n";

                var applyEffects = effect.m_Nodes.GetAll<ApplyEffect>();
                foreach (var e in applyEffects)
                {
                    var name = DbUtility.GetNameFromRecordWithID(e.m_EffectID);
                    t_text += $" Apply effect - {name}";
                }

                HorizontalLayoutElement t_line = CreateHorizontalLine().AlignStart();
                var text =
                    TextPool.I().WarmupElement()
                    .SetText(t_text)
                    .SetColor(C.COLOR_DEFAULT)
                    .FontSize(TEXT_SIZE_B2);

                t_line.Insert(text);
                t_lines.Add(t_line);
            }

            return new ToolTipLines { m_Lines = t_lines };
        }

        private static string GetTextForStatusEffects<T>(T a_status)
            where T : struct, IApplyDamageStatusEffect
        {
            return $"{a_status.m_DamagePerTick} DMG FOR {a_status.m_Duration} TURNS.";
        }

        private static int GetDamageForAbility(Ability a_ability, DamageType a_type)
        {
            int t_total = 0;
            foreach (var dmg in a_ability.GetEffects<DealDamage>())
            {
                if (dmg.m_DamageType == a_type)
                {
                    t_total += dmg.m_BaseDamage;
                }
            }
            return t_total;
        }
    }

}


