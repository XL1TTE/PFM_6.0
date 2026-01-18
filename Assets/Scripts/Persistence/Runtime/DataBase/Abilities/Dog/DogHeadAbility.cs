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
    public sealed class DogHeadAbility : IDbRecord
    {
        public DogHeadAbility()
        {
            ID("abt_dog-head");

            With<Name>(new Name("DogHeadAbility_name"));
            With<Description>(new Description("DogHeadAbility_desc"));

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_DOG_HEAD));

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
                    new ApplyToAllEnemiesInArea(new List<IAbilityNode>{
                         new DealDamage(1, DamageType.PHYSICAL_DAMAGE),
                         new ApplyBleeding(3, 2),
                    }, 1),
                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
