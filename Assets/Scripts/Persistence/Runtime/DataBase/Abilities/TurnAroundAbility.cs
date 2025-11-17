
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
    public sealed class TurnAroundAbility : IDbRecord
    {
        public TurnAroundAbility()
        {
            ID("abt_turn_around");

            With<IconUI>(new IconUI(GR.SPR_TURN_AROUND_ABILITY_ICON));
            With(new AbilityDefenition
            {
                m_AbilityType = AbilityType.ROTATE,
                m_TargetType = TargetSelectionTypes.NONE,
                m_Shifts = new Vector2Int[0] { },
                m_Ability = new Ability(new List<IAbilityNode>
                {
                    new TurnAround()
                }),
            });
        }
    }
}
