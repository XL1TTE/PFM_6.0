using System.Collections.Generic;
using Domain.Abilities;
using Domain.Abilities.Components;
using Gameplay.Abilities;

namespace Core.Utilities
{
    public sealed class L
    {
        public const string TURN_AROUND_ABILITY_ID = "abt_turn_around";


        public static AbilityData DO_NOTHING_ABILITY = new AbilityData
        {
            m_AbilityType = AbilityType.ALL,
            m_Value = new Ability(new List<IAbilityNode> { new DoNothing() })
        };


    }

}


