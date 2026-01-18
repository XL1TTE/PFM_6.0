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
    public sealed class RoosterArmAbility : IDbRecord
    {
        public RoosterArmAbility()
        {
            ID("abt_rooster-arm");

            With<Name>(new Name("RoosterArmAbility_name"));
            With<Description>(new Description("RoosterArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_ROOSTER_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3] {
                    new Vector2Int(1, 0),
                    new Vector2Int(2, -1),
                    new Vector2Int(2, 1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(7, DamageType.PHYSICAL_DAMAGE),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
