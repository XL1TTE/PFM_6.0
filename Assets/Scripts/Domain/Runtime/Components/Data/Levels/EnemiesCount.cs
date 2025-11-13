using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Levels.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct EnemiesCount : IComponent 
    {
        public int amount;
    }
}