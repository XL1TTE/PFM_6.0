
using System.Collections.Generic;
using Domain.Abilities;
using Domain.Extentions;
using Gameplay.Abilities;
using Gameplay.TargetSelection;
using Persistence.Components;
using UnityEngine;

namespace Persistence.DB
{
    public sealed class DinLegAbility : IDbRecord
    {
        public DinLegAbility()
        {
            ID("abt_din_leg");

            With<IconUI>(new IconUI(GR.SPR_MOVE_ABILITY_ICON));
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
