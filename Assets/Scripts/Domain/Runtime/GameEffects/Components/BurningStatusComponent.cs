using System;
using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.GameEffects
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BurningStatusComponent : IComponent
    {
        [Serializable]
        public class Stack
        {
            public int m_Duration;
            public int m_TurnsLeft;
            public int m_DamagePerTurn;
        }

        public List<Stack> m_Stacks;
    }
}
