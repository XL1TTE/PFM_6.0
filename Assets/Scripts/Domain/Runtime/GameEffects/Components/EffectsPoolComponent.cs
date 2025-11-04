using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.GameEffects
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EffectsPoolComponent : IComponent
    {
        public List<StatusEffect> m_StatusEffects;
        public List<PermanentEffect> m_PermanentEffects;
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InitialEffectsPoolComponent : IComponent
    {
        public List<StatusEffect> m_StatusEffects;
        public List<PermanentEffect> m_PermanentEffects;
    }
}
