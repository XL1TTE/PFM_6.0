using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Extentions;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using System.Collections.Generic;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class HorseHeadAbility : IDbRecord
    {
        public HorseHeadAbility()
        {
            ID("abt_horse-head");

            With<Name>(new Name("HorseHeadAbility_name"));
            With<Description>(new Description("HorseHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_HORSE_HEAD));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[3]
                {
                     new Vector2Int(1, 0),
                     new Vector2Int(1, 1),
                     new Vector2Int(1, -1),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(4, DamageType.PHYSICAL_DAMAGE),
                    new ApplyBleeding(3, 2),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
