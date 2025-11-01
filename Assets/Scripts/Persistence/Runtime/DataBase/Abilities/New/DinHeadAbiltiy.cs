
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DinHeadAbiltiy : IDbRecord
    {
        public DinHeadAbiltiy()
        {
            ID("abt_din_head");

            With<IconUI>(new IconUI(GR.SPR_EFFECT_ABILITY_ICON));
            With<AbilityDefenition>(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2]
                {
                    new Vector2Int(1, 0),
                    new Vector2Int(1, 0),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyEffect(-1, "effect_max_health_debuff", false),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
