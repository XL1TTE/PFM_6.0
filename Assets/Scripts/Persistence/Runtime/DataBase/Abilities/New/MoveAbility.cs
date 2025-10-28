
using System.Collections.Generic;
using Domain.Abilities;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class MoveAbility : IDbRecord
    {
        public MoveAbility()
        {
            ID("MoveAbility");

            With<AbilityDefenition>(new AbilityDefenition
            {
                m_TargetType = TargetSelectionTypes.CELL_EMPTY,
                m_Shifts = new Vector2Int[5]
                {
                    new Vector2Int(1, 0),
                    new Vector2Int(1, 1),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0),
                },
                m_Ability = new Ability(new List<IAbilityEffect>
                {
                    new MoveToCell(),
                }),
            });
        }
    }
}
