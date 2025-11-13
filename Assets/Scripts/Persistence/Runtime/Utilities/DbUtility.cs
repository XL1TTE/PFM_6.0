
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Abilities;
using Domain.Abilities.Components;
using Gameplay.Abilities;
using Persistence.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.VisualScripting;
using UnityEngine;

namespace Persistence.Utilities
{


    public static class DbUtility
    {
        /// <summary>
        /// Gets ability data from ability record by record id.
        /// </summary>
        /// <param name="a_RecordID"></param>
        /// <returns></returns>
        public static AbilityData GetAbilityDataFromAbilityRecord(string a_RecordID)
        {
            if (DataBase.TryFindRecordByID(a_RecordID, out var record) == false) { return null; }
            DataBase.TryGetRecord<IconUI>(record, out var icon);
            if (DataBase.TryGetRecord<AbilityDefenition>(record, out var defenition))
            {
                return new AbilityData
                {
                    m_Value = defenition.m_Ability.Clone(),
                    m_AbilityTemplateID = a_RecordID,
                    m_Shifts = defenition.m_Shifts == null ? new List<Vector2Int>() : defenition.m_Shifts.ToList(),
                    m_TargetType = defenition.m_TargetType,
                    m_Icon = icon.m_Value,
                    m_Tags = defenition.m_Tags,
                    m_AbilityType = defenition.m_AbilityType
                };
            }
            return null;
        }



        public static void DoubleAbilityStats(ref Ability ability)
        {
            if (ability == null) { return; }

            var dmg_effects = ability.GetEffects<DealDamage>();
            for (int i = 0; i < dmg_effects.Count(); ++i)
            {
                dmg_effects[i].m_BaseDamage *= 2;
            }
        }

        public static IEnumerable<Vector2Int> CombineShifts(IEnumerable<Vector2Int> first, IEnumerable<Vector2Int> second)
        {
            HashSet<Vector2Int> result = new HashSet<Vector2Int>(first.Concat(second));
            return result;
        }
    }

}
