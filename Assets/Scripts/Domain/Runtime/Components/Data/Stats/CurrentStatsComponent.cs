using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Stats.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct CurrentStatsComponent : IComponent
    {
        public int m_CurrentHealth;
        public int m_MaxHealth;

        public int m_CurrentSpeed;
        public int m_MaxSpeed;

        public int m_MovementActions;
        public int m_MaxMovementActions;
        public int m_InteractionActions;
        public int m_MaxInteractionActions;
    }
}


