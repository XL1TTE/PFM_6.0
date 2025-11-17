
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

            With<Name>(new Name("Din's power."));
            With<Description>(new Description("Apply poison. And poison weakness."));

            With<IconUI>(new IconUI(GR.SPR_ATTACK_ABILITY_ICON));

            With(new AbilityDefenition
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
                    new DealDamage(1, DamageType.PHYSICAL_DAMAGE),
                    new ApplyPoison(2, 5),
                    new ApplyEffect(2, "effect_poison_weak"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
    public sealed class Din2ArmAbility : IDbRecord
    {
        public Din2ArmAbility()
        {
            ID("abt_din_arm2");

            With<Description>(new Description("Heal in area."));

            With<IconUI>(new IconUI(GR.SPR_ATTACK_ABILITY_ICON));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.ANY_CELL,
                m_Shifts = new Vector2Int[2] { new Vector2Int(3, 0), new Vector2Int(1, 0) },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    // new PlayTweenAnimation(TweenAnimations.ATTACK),
                    // new WaitForTweenActionFrame(),
                    // new DealDamage(1, DamageType.PHYSICAL_DAMAGE),
                    // new ApplyBleeding(1, 2),
                    // new WaitForLastAnimationEnd()
                    new HealInArea(5, 1)
                }),
            });
        }
    }
    public sealed class Din3ArmAbility : IDbRecord
    {
        public Din3ArmAbility()
        {
            ID("abt_din_arm3");

            With<IconUI>(new IconUI(GR.SPR_ATTACK_ABILITY_ICON));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] { new Vector2Int(3, 0), new Vector2Int(1, 0) },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(1, DamageType.PHYSICAL_DAMAGE),
                    new ApplyBurning(3, 5),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
