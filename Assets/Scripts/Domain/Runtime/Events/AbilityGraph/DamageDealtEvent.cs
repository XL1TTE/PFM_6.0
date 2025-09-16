using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.AbilityGraph
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct DamageDealtEvent: IEventData{
        public Entity Source;
        public Entity Target;
        public Entity SourceAbility;
        public short BaseDamage;
        public short FinalDamage;
        public DamageType DamageType;
    }
}

