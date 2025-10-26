using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

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
    public struct ActorActionStatesChanged : IEventData
    {
        public Entity m_Actor;
        public List<ActorActionStates> m_Values;
    }
}

