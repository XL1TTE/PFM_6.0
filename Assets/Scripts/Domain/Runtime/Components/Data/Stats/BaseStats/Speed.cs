using System.Collections.Generic;
using Scellecs.Morpeh;
using TriInspector;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Speed : IStatComponent
    {
        [ShowInInspector] public int m_Value { get; set; }
    }
}


