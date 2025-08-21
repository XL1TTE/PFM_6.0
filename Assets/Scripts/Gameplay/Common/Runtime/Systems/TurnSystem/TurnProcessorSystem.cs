using System.Collections.Generic;
using System.Linq;
using Gameplay.Common.Components;
using Gameplay.Common.Events;
using Gameplay.Common.Requests;
using Gameplay.Features.Monster.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.Common.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnProcessorSystem : ISystem
    {
        public World World { get; set; }
        
        
        private Request<ProcessTurnRequest> req_processTurn;
        private Event<NextTurnStartedEvent> evt_nextTurnStart;
        
        private Stash<CurrentTurnTakerTag> stash_turnTakerTag;
        

        public void OnAwake()
        {
            req_processTurn = World.GetRequest<ProcessTurnRequest>();
            evt_nextTurnStart = World.GetEvent<NextTurnStartedEvent>();
            
            stash_turnTakerTag = World.GetStash<CurrentTurnTakerTag>();
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
            Entity previusTurnTaker;
            Entity nextTurnTaker;
            if (req.CurrentTurnQueue.Count > 1){
                previusTurnTaker = req.CurrentTurnQueue[0];
                nextTurnTaker = req.CurrentTurnQueue[1];

            }
            else{
                previusTurnTaker = req.CurrentTurnQueue[0];
                nextTurnTaker = req.CurrentTurnQueue[0];
            }
            RemoveTurnTakerTag(previusTurnTaker);
            AddTurnTakerTag(nextTurnTaker);
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


