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
    public sealed class RaccoonArmAbility : IDbRecord
    {
        public RaccoonArmAbility()
        {
            ID("abt_raccoon-arm");

            With<Name>(new Name("RaccoonArmAbility_name"));
            With<Description>(new Description("RaccoonArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_RACCOON_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3] {
                    new Vector2Int(1, 2),
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                   new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(2, DamageType.PHYSICAL_DAMAGE),
                    new ApplyPoison(2, 8),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
