using System;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using static Domain.Stats.Components.IResistanceModiffier;

namespace Domain.Stats.Components
{

    public interface IResistanceModiffier : IComponent
    {
        [Serializable]
        public enum Stage { WEAKNESS = -1, NONE = 0, RESISTANT = 1, IMMUNE = 2 }

        Stage m_Stage { get; set; }
    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BleedResistanceModiffier : IResistanceModiffier
    {
        [field: SerializeField]
        public Stage m_Stage { get; set; }
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct PoisonResistanceModiffier : IResistanceModiffier
    {
        [field: SerializeField]
        public Stage m_Stage { get; set; }
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BurningResistanceModiffier : IResistanceModiffier
    {
        [field: SerializeField]
        public Stage m_Stage { get; set; }
    }
}


