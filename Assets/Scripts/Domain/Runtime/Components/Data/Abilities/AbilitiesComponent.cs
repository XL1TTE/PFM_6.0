using System.Collections;
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
        public AbilityData m_LegAbility;
    }
}


