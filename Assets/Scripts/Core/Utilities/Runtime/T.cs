using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Persistence.Components;
using Persistence.DB;
using UI.ToolTip;
using UnityEngine;

namespace Core.Utilities
{
    public static partial class T
    {
        private const int TEXT_SIZE_H1 = 32;
        private const int TEXT_SIZE_H2 = 28;
        private const int TEXT_SIZE_H3 = 24;
        private const int TEXT_SIZE_H4 = 20;
        private const int TEXT_SIZE_H5 = 18;
        private const int TEXT_SIZE_B1 = 20;
        private const int TEXT_SIZE_B2 = 18;
        private const int TEXT_SIZE_B3 = 16;
        private const int TEXT_SIZE_B4 = 14;

        private static ToolTipLine CreateToolTipLine()
        {
            var line = UnityEngine.Object.Instantiate(GR.p_ToolTipLine);
            line.gameObject.SetActive(false);
            return line;
        }

        public static ToolTipLines GetAbilityShortTooltip(AbilityData a_abilityData)
        {
            var a_ability = a_abilityData.m_Value;

            var t_lines = new List<ToolTipLine>();

            if (DataBase.TryFindRecordByID(a_abilityData.m_AbilityTemplateID, out var abilityRecord))
            {
                if (DataBase.TryGetRecord<Name>(abilityRecord, out var abilityName))
                {
                    ToolTipLine t_nameLine = CreateToolTipLine().AlignStart();
                    var name = t_nameLine
                        .WarmupText(abilityName.m_Value, Color.white)
                        .Bold()
                        .FontSize(TEXT_SIZE_H3);

                    t_nameLine.Insert(name);
                    t_lines.Add(t_nameLine);
                }
                if (DataBase.TryGetRecord<Description>(abilityRecord, out var abilityDesc))
                {
                    ToolTipLine t_descLine = CreateToolTipLine().AlignStart();
                    var desc = t_descLine
                        .WarmupText(abilityDesc.m_Value, Color.white)
                        .FontSize(TEXT_SIZE_B2);

                    t_descLine.Insert(desc);
                    t_lines.Add(t_descLine);
                }
            }

            var phys_dmg = GetDamageForAbiltiy(a_ability, DamageType.PHYSICAL_DAMAGE);
            if (phys_dmg > 0)
            {
                ToolTipLine t_physDmg = CreateToolTipLine().AlignStart();
                var t_physDmgText = t_physDmg
                    .WarmupText($"{phys_dmg} DMG.", "#8d781e".ToColor())
                    .FontSize(TEXT_SIZE_B2);

                var icon = t_physDmg.WarmupIcon(GR.SPR_UI_PHYSICAL_DMG);
                t_physDmg.Insert(icon);

                t_physDmg.Insert(t_physDmgText);
                t_lines.Add(t_physDmg);
            }

            foreach (var status in a_ability.GetEffects<ApplyBleeding>())
            {
                ToolTipLine t_line = CreateToolTipLine().AlignStart();
                var text = t_line
                    .WarmupText(GetTextForStatusEffects(status), Color.red)
                    .FontSize(TEXT_SIZE_B2);

                var icon = t_line.WarmupIcon(GR.SPR_UI_EFFECT_BLOOD);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }
            foreach (var status in a_ability.GetEffects<ApplyBurning>())
            {
                ToolTipLine t_line = CreateToolTipLine().AlignStart();
                var text = t_line
                    .WarmupText(GetTextForStatusEffects(status), Color.yellow)
                    .FontSize(TEXT_SIZE_B2);

                var icon = t_line.WarmupIcon(GR.SPR_UI_EFFECT_FIRE);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);

            }
            foreach (var status in a_ability.GetEffects<ApplyPoison>())
            {
                ToolTipLine t_line = CreateToolTipLine().AlignStart();
                var text = t_line
                    .WarmupText(GetTextForStatusEffects(status), Color.green)
                    .FontSize(TEXT_SIZE_B2);

                var icon = t_line.WarmupIcon(GR.SPR_UI_EFFECT_POISON);
                t_line.Insert(icon);
                t_line.Insert(text);
                t_lines.Add(t_line);
            }

            return new ToolTipLines { m_Lines = t_lines };
        }

        private static string GetTextForStatusEffects<T>(T a_status)
            where T : struct, IApplyStatusEffect
        {
            return $"{a_status.m_DamagePerTick} DMG FOR {a_status.m_Duration} TURNS.";
        }

        private static int GetDamageForAbiltiy(Ability a_ability, DamageType a_type)
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


