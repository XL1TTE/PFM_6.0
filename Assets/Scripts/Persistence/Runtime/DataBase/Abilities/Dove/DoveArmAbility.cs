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
    public sealed class DoveArmAbility : IDbRecord
    {
        public DoveArmAbility()
        {
            ID("abt_dove-arm");

            With<Name>(new Name("DoveArmAbility_name"));
            With<Description>(new Description("DoveArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_DOVE_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.HEAL
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ALLY,
                m_Shifts = new Vector2Int[4] {
                    new Vector2Int(0, -1),
                    new Vector2Int(0, 1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new Heal(7),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
