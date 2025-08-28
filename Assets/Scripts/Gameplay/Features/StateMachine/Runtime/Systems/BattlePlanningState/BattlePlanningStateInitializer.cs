
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.StateMachine.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattlePlanningStateInitializer : IInitializer
    {
        public World World { get; set; }

        public void OnAwake()
        {
            StateMachineWorld.EnterState<BattlePlanningInitializeState>();
        }

        public void Dispose()
        {

        }
    }
}
