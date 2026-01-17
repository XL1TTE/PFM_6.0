using Project;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EnemyLootComponent : IComponent
    {
        public EnemyLootWrapper[] loot;
    }
}


