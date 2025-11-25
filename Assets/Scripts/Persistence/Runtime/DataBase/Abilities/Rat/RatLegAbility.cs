
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
    public sealed class RatLegAbility : IDbRecord
    {
        public RatLegAbility()
        {
            ID("abt_rat-leg");

            With<AbilityShiftsSprite>(new AbilityShiftsSprite() { m_Value = Resources.Load<Sprite>("Assets/Resources/Art/Abilities/Spr_Bodypart_Head_Test_1") });

            With<IconUI>(new IconUI(GR.SPR_UI_ABT_MOVE));
            With(new AbilityDefenition
            {
                m_AbilityType = AbilityType.MOVEMENT,
                m_TargetType = TargetSelectionTypes.CELL_EMPTY,
                m_Shifts = new Vector2Int[4]
                {
                    new Vector2Int(1, 0),
                    new Vector2Int(0, 1),
                    new Vector2Int(0, -1),
                    new Vector2Int(-1, 0),
                },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new MoveToCell(),
                }),
            });
        }
    }
}
