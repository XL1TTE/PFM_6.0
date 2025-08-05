using Gameplay.Common.Components;
using Gameplay.Common.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StateMachineSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _currentState;
        
        private Request<StateChangeRequest> req_stateChange;
        private Request<StateTransitionRequest> req_stateTransition;
        
        private Stash<CurrentStateComponent> stash_currentState;

        public void OnAwake()
        {
            _currentState = World.Filter
                .With<CurrentStateComponent>()
                .Build();

            req_stateChange = World.GetRequest<StateChangeRequest>();
            req_stateTransition = World.GetRequest<StateTransitionRequest>();

            stash_currentState = World.GetStash<CurrentStateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if(_currentState.IsEmpty()){return;}
            
            foreach(var req in req_stateChange.Consume()){
                ref Entity currentState = ref stash_currentState.Get(_currentState.First()).Value;

                req_stateTransition.Publish(new StateTransitionRequest{
                   OldState = currentState,
                   NewState = req.NextState 
                });
                
                currentState = req.NextState;
            }
        }

        public void Dispose()
        {

        }
    }
}


