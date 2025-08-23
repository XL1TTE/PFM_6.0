using Domain.Monster.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Monster.Components{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct MonsterDammyRefComponent : IComponent
    {
        public MonsterDammy MonsterDammy;
    }
}


