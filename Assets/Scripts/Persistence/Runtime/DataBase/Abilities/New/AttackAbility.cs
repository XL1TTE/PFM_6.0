


using System.Collections.Generic;
using Domain.Ability;
using Gameplay.Abilities;
using UnityEngine;

namespace Persistence.DB
{

    public sealed class AttackAbility : IDbRecord
    {
        public AttackAbility()
        {
            ID("AttackAbility");

            With<AbilityData>(new AbilityData
            {
                m_Shifts = new Vector2Int[2] { new Vector2Int(1, 0), new Vector2Int(2, 0) },
                m_Ability = new Ability(new List<IAbilityEffect>
                {
                    new DealDamage(5, DamageType.PHYSICAL_DAMAGE)
                }),
            });
        }
    }
}
