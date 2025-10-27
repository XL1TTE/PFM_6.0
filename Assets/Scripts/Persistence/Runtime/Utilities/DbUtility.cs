
using System.Collections.Generic;
using System.Linq;
using Domain.Abilities;
using Domain.Abilities.Components;
using Gameplay.Abilities;
using Persistence.DB;
using Scellecs.Morpeh;

namespace Persistence.Utilities
{


    public static class DbUtility
    {
        public static AbilityData GetAbilityDataFromAbilityRecord(string a_RecordID)
        {
            if (DataBase.TryFindRecordByID(a_RecordID, out var record) == false) { return null; }
            if (DataBase.TryGetRecord<AbilityDefenition>(record, out var defenition))
            {
                return new AbilityData
                {
                    m_Value = defenition.m_Ability.Clone(),
                    m_AbilityTemplateID = a_RecordID
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

    }

}
