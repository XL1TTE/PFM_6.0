using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DoveHeadAbility : IDbRecord
    {
        public DoveHeadAbility()
        {
            ID("abt_dove-head");

            With<Name>(new Name("DoveHeadAbility_name"));
            With<Description>(new Description("DoveHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_DOVE_HEAD));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.HEAL,
                    AbilityTags.EFFECT,
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[4]
                {
                     new Vector2Int(-1, 0),
                     new Vector2Int(-2, 0),
                     new Vector2Int(-2, -1),
                     new Vector2Int(-2, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllAlliesInArea(new List<IAbilityNode>{
                         new Heal(3),
                         new ApplyEffect(2, "effect_dove-head-skill"),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
