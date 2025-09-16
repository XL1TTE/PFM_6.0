using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DealDamageRequest : IRequestData
    {
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public short MinBaseDamage;
        public short MaxBaseDamage;
        public DamageType DamageType;
    }
}

