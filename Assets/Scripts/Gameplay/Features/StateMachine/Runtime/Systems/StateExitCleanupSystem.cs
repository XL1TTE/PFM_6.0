
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StateExitCleanupSystem : ICleanupSystem
    {
        public World World { get; set; }

        private Event<OnStateExitEvent> evt_StateExit;

        public void OnAwake()
        {
            evt_StateExit = SM.m_World.GetEvent<OnStateExitEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var e in evt_StateExit.publishedChanges)
            {
                SM.RemoveState(e.StateEntity);
            }
        }
        public void Dispose()
        {

        }
    }
}


