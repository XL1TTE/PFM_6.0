using TriInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MaxHealthModifier : IStatModifierComponent
    {
        [ShowInInspector] public int m_Flat { get; set; }

        [ShowInInspector] public float m_Multiplier { get; set; }
    }
}


