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
                   AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[4] {
                    new Vector2Int(-1, 0),
                    new Vector2Int(-1, 1),
                    new Vector2Int(-1, -1),
                    new Vector2Int(-2, 0),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(2, DamageType.PHYSICAL_DAMAGE),
                    new ApplyBurning(5, 5),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
