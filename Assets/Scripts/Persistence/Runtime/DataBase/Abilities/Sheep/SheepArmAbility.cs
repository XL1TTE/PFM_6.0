
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
    public sealed class SheepArmAbility : IDbRecord
    {
        public SheepArmAbility()
        {
            ID("abt_sheep-arm");

            With<Name>(new Name("SheepArmAbility_name"));
            With<Description>(new Description("SheepArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_SHEEP_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.HEAL
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[3] {
                    new Vector2Int(2, 0),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllAlliesInArea(new List<IAbilityNode>{
                         new Heal(3),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }

}
