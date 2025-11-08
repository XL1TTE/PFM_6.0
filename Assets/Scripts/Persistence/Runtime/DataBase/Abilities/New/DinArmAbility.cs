
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

    public sealed class DinArmAbility : IDbRecord
    {
        public DinArmAbility()
        {
            ID("abt_din_arm");

            With<IconUI>(new IconUI(GR.SPR_ATTACK_ABILITY_ICON));
            With<AbilityDefenition>(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] { new Vector2Int(5, 0), new Vector2Int(1, 0) },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(3, DamageType.PHYSICAL_DAMAGE),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
