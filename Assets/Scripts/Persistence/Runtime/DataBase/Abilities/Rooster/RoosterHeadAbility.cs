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
    public sealed class RoosterHeadAbility : IDbRecord
    {
        public RoosterHeadAbility()
        {
            ID("abt_rooster-head");

            With<Name>(new Name("RoosterHeadAbility_name"));
            With<Description>(new Description("RoosterHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_ROOSTER_HEAD));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[6]
                {
                     new Vector2Int(2, 0),
                     new Vector2Int(2, 1),
                     new Vector2Int(2, -1),
                     new Vector2Int(-2, 0),
                     new Vector2Int(-2, 1),
                     new Vector2Int(-2, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                   new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllAlliesInArea(new List<IAbilityNode>{
                         new ApplyEffect(2, "effect_rooster-head-skill"),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
