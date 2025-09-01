using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.StateMachine.Mono{
    
    
    public static class StateMachineWorld{
        
        static StateMachineWorld(){
            Value = World.Create();

            evt_onStateEnter = Value.GetEvent<OnStateEnterEvent>();
            evt_onStateExit = Value.GetEvent<OnStateExitEvent>();
        }

        public static World Value;
        
        private static Event<OnStateEnterEvent> evt_onStateEnter;
        private static Event<OnStateExitEvent> evt_onStateExit;
                
        private static void CommitChanges(){
            Value.Commit();
        }
        
        public static void Update(){
            Value.Update(Time.deltaTime);
            Value.CleanupUpdate(Time.deltaTime);
        }
        
        public static bool TryGetState<T>(out T state) where T : struct, IState{
            var states = Value.Filter.With<T>().Build();
            if(states.IsEmpty()){state = default; return false;}
            
            state = Value.GetStash<T>().Get(states.First());
            return true;
        }
        
        public static bool IsStateActiveOptimized<T>(Filter stateFilter, Stash<T> stash, out T state) where T: struct, IState{
            if (stateFilter.IsEmpty()) { state = default; return false; }
            if(stash.Has(stateFilter.First()) == false){
                state = default;
                return false;
            }
            state = stash.Get(stateFilter.First());
            return true;
        }

        public static Entity EnterState<T>() where T: struct, IState
        {
            var state = Value.CreateEntity();
            var stash = Value.GetStash<T>();
            stash.Add(state);

            evt_onStateEnter.NextFrame(new OnStateEnterEvent{StateEntity = state});

            CommitChanges();
            return state;
        }
        
        public static void ExitState<T>() where T: struct, IState{
            var states = Value.Filter.With<T>().Build();
            if(states.IsEmpty()){return;}
                        
            var state = states.First();
            
            evt_onStateExit.NextFrame(new OnStateExitEvent { StateEntity = state });

            CommitChanges();
        }
        
        public static void ExitState(Entity stateEntity){
            if(Value.Has(stateEntity)){
                Value.RemoveEntity(stateEntity);
            }
            CommitChanges();
        }
    } 
    
}
