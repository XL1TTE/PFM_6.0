using System.Collections;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Components
{

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilitiesComponent : IComponent
    {
        public AbilityData m_LeftHandAbility;
        public AbilityData m_RightHandAbility;
        public AbilityData m_HeadAbility;
        public AbilityData m_LegsAbility;

        public AbilityData m_TurnAroundAbility;


        public IEnumerable<AbilityData> GetAllAbilities()
        {
            return new List<AbilityData>{
                m_HeadAbility,
                m_RightHandAbility,
                m_LeftHandAbility,
                m_LegsAbility,
                m_TurnAroundAbility
            };
        }
    }
}


