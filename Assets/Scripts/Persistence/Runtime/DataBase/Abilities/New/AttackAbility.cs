
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Services;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using UnityEngine;

namespace Persistence.DB
{

    public sealed class AttackAbility : IDbRecord
    {
        public AttackAbility()
        {
            ID("AttackAbility");

            With<AbilityDefenition>(new AbilityDefenition
            {
                m_TargetType = TargetSelectionTypes.CELL_WITH_ENEMY,
                m_Shifts = new Vector2Int[2] { new Vector2Int(5, 0), new Vector2Int(2, 0) },
                m_Ability = new Ability(new List<IAbilityEffect>
                {
                    new PlayTweenAnimation(TweenAnimations.ATTACK),
                    new WaitForTweenActionFrame(),
                    new DealDamage(5, DamageType.PHYSICAL_DAMAGE),
                }),
            });
        }
    }
}
