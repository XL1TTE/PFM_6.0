
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
    public sealed class CockroachArmAbility : IDbRecord
    {
        public CockroachArmAbility()
        {
            ID("abt_cockroach-arm");

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_ATTACK));

            With(new AbilityDefenition
            {
                m_Tags = new List<AbilityTags>{
                    AbilityTags.DAMAGE
                },
                m_AbilityType = AbilityType.INTERACTION,
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[4] {
                    new Vector2Int(1, 0),
                    new Vector2Int(-1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1)},

                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new ApplyToAllEnemiesInArea(new List<IAbilityNode>{
                        new DealDamage(2, DamageType.POISON_DAMAGE),
                        new ApplyPoison(2, 5),
                    }, 1),

                    new WaitForLastAnimationEnd()
                }),
            });
        }
    }
}
