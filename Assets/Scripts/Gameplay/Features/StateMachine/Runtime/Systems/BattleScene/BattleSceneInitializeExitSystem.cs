using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleSceneInitializeExitSystem : ISystem
    {
        public World World { get; set; }
        
        private Event<OnStateExitEvent> evt_onStateExit;
        
        private Stash<BattleSceneInitializeState> stash_state;

        public void OnAwake()
        {
            evt_onStateExit = StateMachineWorld.Value.GetEvent<OnStateExitEvent>();

            stash_state = StateMachineWorld.Value.GetStash<BattleSceneInitializeState>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateExit.publishedChanges)
            {
            }
        }



        public void Dispose()
        {
        }

        private bool IsValid(Entity entityState)
        {
            if (stash_state.Has(entityState))
            {
                return true;
            }
            return false;
        }
    }

}
