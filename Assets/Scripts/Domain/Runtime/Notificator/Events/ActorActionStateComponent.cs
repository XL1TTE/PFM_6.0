using System.Collections.Generic;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Domain.Notificator
{
    public enum ActorActionStates : byte
    {
        Animating,
        ExecutingAbility,
        Moving,
        Idle,
    }

    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct ActorActionStatesComponent : IComponent
    {
        public List<ActorActionStates> m_Values;
    }
}

