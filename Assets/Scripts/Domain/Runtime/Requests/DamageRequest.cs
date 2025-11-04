using Domain.Abilities;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Requests
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DealDamageRequest : IRequestData
    {
        public Entity m_Source;
        public Entity m_Target;
        public Entity m_SourceAbility;
        public int m_Damage;
        public DamageType m_DamageType;
    }
}

