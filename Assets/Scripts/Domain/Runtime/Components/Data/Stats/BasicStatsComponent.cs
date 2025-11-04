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


        public int m_MovementActions;
        public int m_MaxMovementActions;
        public int m_InteractionActions;
        public int m_MaxInteractionActions;
    }
}


