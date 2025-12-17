using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Abilities.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct InteractionsComponent : IComponent
    {
        public int m_MaxInteractions;
        public int m_InteractionsLeft;

        public int m_MaxMoveInteractions;
        public int m_MoveInteractionsLeft;
    }
}


