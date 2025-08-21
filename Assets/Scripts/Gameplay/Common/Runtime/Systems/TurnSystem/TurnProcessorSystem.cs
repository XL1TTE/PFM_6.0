using System.Collections.Generic;
using System.Linq;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Features.Monster.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEditor.Search;


namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnProcessorSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter filter_turnQueue;
        
        private Request<ProcessTurnRequest> req_processTurn;
        private Event<NextTurnStartedEvent> evt_nextTurnStart;
        
        private Stash<CurrentTurnTakerTag> stash_turnTakerTag;
        private Stash<TurnQueueComponent> stash_turnQueue;
        

        public void OnAwake()
        {
            filter_turnQueue = World.Filter
                .With<TurnQueueComponent>()
                .Build();

            req_processTurn = World.GetRequest<ProcessTurnRequest>();
            evt_nextTurnStart = World.GetEvent<NextTurnStartedEvent>();
            
            stash_turnTakerTag = World.GetStash<CurrentTurnTakerTag>();
            stash_turnQueue = World.GetStash<TurnQueueComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_processTurn.Consume()){
                ProcessTurn(req);
                SendNotifications();
            }
        }

        public void Dispose()
        {

        }
        
        private void SendNotifications(){
            evt_nextTurnStart.NextFrame(new NextTurnStartedEvent{});
        }
        
        private void ProcessTurn(ProcessTurnRequest req)
        {
            if(filter_turnQueue.IsEmpty()){return;}
            ref var queue = ref stash_turnQueue.Get(filter_turnQueue.First()).Value;
            
            Entity previousTurnTaker;
            Entity currentTurnTaker;
            
            if(queue.Count < 1){return;}

            previousTurnTaker = queue.Dequeue();
            currentTurnTaker = queue.Dequeue();
            queue.Enqueue(currentTurnTaker);
            queue.Enqueue(previousTurnTaker);

            RemoveTurnTakerTag(previousTurnTaker);
            AddTurnTakerTag(currentTurnTaker);
        }
        
        private void RemoveTurnTakerTag(Entity entity){
            if(stash_turnTakerTag.Has(entity)){
                stash_turnTakerTag.Remove(entity);
            }
        }
        private void AddTurnTakerTag(Entity entity)
        {
            stash_turnTakerTag.Set(entity, new CurrentTurnTakerTag{});
        }
        
        private void Cleanup(){

        }

    }
}


