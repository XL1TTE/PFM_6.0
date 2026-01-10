
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class PigArmAbility : IDbRecord
    {
        public PigArmAbility()
        {
            ID("abt_pig-arm");

            With<Name>(new Name("PigArmAbility_name"));
            With<Description>(new Description("PigArmAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_PIG_ARM));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                     AbilityTags.DAMAGE,
                     AbilityTags.EFFECT
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[5] {
                    new Vector2Int(2, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                    new Vector2Int(1, 1),
                    new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(Domain.Services.TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(3, DamageType.PHYSICAL_DAMAGE),
                    new ApplyEffect(3, "effect_pig_skill1"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
