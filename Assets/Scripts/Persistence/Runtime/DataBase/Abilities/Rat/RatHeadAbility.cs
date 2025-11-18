
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class RatHeadAbility : IDbRecord
    {
        public RatHeadAbility()
        {
            ID("abt_rat-head");

            With<IconUI>(new IconUI(GR.SPR_ATTACK_ABILITY_ICON));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3]
                {
                     new Vector2Int(1, 0),
                     new Vector2Int(1, 1),
                     new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new ApplyEffect(-1, "effect_rat-debuff"),
                }),
            });
        }
    }
}
