using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{

    public enum AbilityTargetType : byte
    {
        ENEMY,
        MONSTER,
        OBSTACLE,
        SELF
    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct AbilityTargetsComponent : IComponent
    {
        public int m_TargetCount;
        public Entity[] m_Targets;

        /// <summary>
        /// Types of target to which ability can be aplied.
        /// </summary>
        public List<AbilityTargetType> m_TargetTypes;
    }
}

