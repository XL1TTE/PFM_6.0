
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
    public sealed class SheepLegAbility : IDbRecord
    {
        public SheepLegAbility()
        {
            ID("abt_sheep-leg");

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_MOVE));
            With(new AbilityDefenition
            {
                m_AbilityType = AbilityType.MOVEMENT,
                m_TargetType = TargetSelectionTypes.CELL_EMPTY,
                m_Shifts = new Vector2Int[3] {
                    new Vector2Int(-2, 0),
                    new Vector2Int(-1, 2),
                    new Vector2Int(-1, -2),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new MoveToCell()
                }),
            });
        }
    }

}
