using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct SpeedModifier : IStatModifierComponent
    {
        public int m_Flat { get; set; }
        public float m_Multiplier { get; set; }
    }
}


