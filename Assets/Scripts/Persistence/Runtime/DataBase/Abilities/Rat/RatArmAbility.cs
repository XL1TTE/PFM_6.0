
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
    public sealed class RatArmAbility : IDbRecord
    {
        public RatArmAbility()
        {
            ID("abt_rat-arm");

            With<AbilityShiftsSprite>(new AbilityShiftsSprite() { m_Value = Resources.Load<Sprite>("Assets/Resources/Art/Abilities/Spr_Bodypart_Head_Test_1") });

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_ATTACK));
            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] { new Vector2Int(5, 0), new Vector2Int(6, 0) },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(3, DamageType.PHYSICAL_DAMAGE),
                    new ApplyPoison(3, 12),
                    new ApplyEffect(2, "effect_poison_weak"),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
