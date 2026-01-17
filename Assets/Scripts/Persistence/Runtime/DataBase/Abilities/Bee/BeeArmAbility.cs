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
    public sealed class BeeArmAbility : IDbRecord
    {
        public BeeArmAbility()
        {
            ID("abt_bee-arm");

            With<Name>(new Name("BeeArmAbility_name"));
            With<Description>(new Description("BeeArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_BEE_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3] {
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1),
                    new Vector2Int(-1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new ApplyToAllAlliesInArea(new List<IAbilityNode>{
                        new ApplyEffect(2, "effect_bee-arm-skill"),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
