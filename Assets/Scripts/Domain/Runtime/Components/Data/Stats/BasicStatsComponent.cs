using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BaseStatsComponent : IComponent
    {
        public int Health;
        public int MaxHealth;
        
        public int Speed;
        public int MaxSpeed;
 
    }
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CurrentStatsComponent : IComponent
    {
        public int CurrentHealth;
        public int MaxHealth;
        
        public int CurrentSpeed;
        public int MaxSpeed;
    }
}


