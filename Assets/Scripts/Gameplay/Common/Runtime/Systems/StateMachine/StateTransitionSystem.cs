using Core.Utilities.Extantions;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StateTransitionSystem : ISystem
    {
        public World World { get; set; }
        
        private Request<StateTransitionRequest> req_stateTransition;
        
        private Event<OnStateEnterEvent> evt_onStateEnter;
        private Event<OnStateExitEvent> evt_onStateExit;
        
        private Stash<TagStateComponent> stash_tagState;

        public void OnAwake()
        {
            req_stateTransition = World.GetRequest<StateTransitionRequest>();

            evt_onStateEnter = World.GetEvent<OnStateEnterEvent>();
            evt_onStateExit = World.GetEvent<OnStateExitEvent>();

            stash_tagState = World.GetStash<TagStateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_stateTransition.Consume()){
                
                if(req.OldState.IsExist()){
                    DisableState(req.OldState);
                }
                if(req.NewState.IsExist()){
                    EnableState(req.NewState);
                }
            }
        }

        public void Dispose()
        {

        }
        
        
        private void EnableState(Entity stateEntity){
            if(!stash_tagState.Has(stateEntity)){
                stash_tagState.Add(stateEntity);
                evt_onStateEnter.NextFrame(new OnStateEnterEvent{
                   StateEntity = stateEntity 
                });
            }
        }
        private void DisableState(Entity stateEntity){
            if (stash_tagState.Has(stateEntity))
            {
                stash_tagState.Remove(stateEntity);
                evt_onStateExit.NextFrame(new OnStateExitEvent
                {
                    StateEntity = stateEntity
                });
            }
        }
    }
}

