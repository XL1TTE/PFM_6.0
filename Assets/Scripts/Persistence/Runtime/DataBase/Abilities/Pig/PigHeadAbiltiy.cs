
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
    public sealed class PigHeadAbiltiy : IDbRecord
    {
        public PigHeadAbiltiy()
        {
            ID("abt_pig-head");

            With<Name>(new Name("PigHeadAbiltiy_name"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_PIG_HEAD));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[1]
                {
                    new Vector2Int(0, 0),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new ApplyToAllAlliesInArea(new List<IAbilityNode>{
                        new ApplyEffect(2, "effect_pig-head-skill"),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
