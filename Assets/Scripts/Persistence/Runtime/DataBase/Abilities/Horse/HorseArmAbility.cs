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
    public sealed class HorseArmAbility : IDbRecord
    {
        public HorseArmAbility()
        {
            ID("abt_horse-arm");

            With<Name>(new Name("HorseArmAbility_name"));
            With<Description>(new Description("HorseArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_HORSE_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE,
                    AbilityTags.DEBUFF
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] {
                    new Vector2Int(-1, 1),
                    new Vector2Int(-1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(5, DamageType.PHYSICAL_DAMAGE),
                    new ApplyEffect(3, "effect_horse-arm-skill"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
