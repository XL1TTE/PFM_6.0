using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Monster.Components;
using Domain.Monster.Tags;
using Domain.TurnSystem.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Requests;
using Domain.TurnSystem.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;


namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnSystemInitializer : ISystem
    {
        public World World { get; set; }
        
        private Filter filter_monsters;
        
        private Request<InitializeTurnSystemRequest> req_initSystem;

        private Request<ProcessTurnRequest> req_processTurn;
        private Event<TurnSystemInitializedEvent> evt_turnSystemInitialized;

        private Stash<Speed> stash_Speed;
        private Stash<TurnQueueComponent> stash_turnQueue;
        
        public void OnAwake()
        {
            filter_monsters = World.Filter
                .With<TagMonster>()
                .Build();

            req_initSystem = World.GetRequest<InitializeTurnSystemRequest>();
            req_processTurn = World.GetRequest<ProcessTurnRequest>();
            evt_turnSystemInitialized = World.GetEvent<TurnSystemInitializedEvent>();

            stash_Speed = World.GetStash<Speed>();
            stash_turnQueue = World.GetStash<TurnQueueComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_initSystem.Consume()){
                InitializeSystem(req);
            }
        }

        public void Dispose()
        {
        }
        
        private void InitializeSystem(InitializeTurnSystemRequest req)
        {
            CreateTurnQueue();
            SendNotification();
        }
        
        private Entity CreateTurnQueue(){
            Queue<Entity> TurnQueue = new();
            Entity queueEntity = World.CreateEntity();
            
            foreach (var monster in filter_monsters)
            {
                TurnQueue.Enqueue(monster);
            }
            TurnQueue.OrderBy(e =>
            {
                var speed = 0.0f;
                if (stash_Speed.Has(e))
                {
                    speed = stash_Speed.Get(e).Value;
                }
                return speed;
            });

            stash_turnQueue.Set(queueEntity, new TurnQueueComponent{
                Value = TurnQueue
            });

            return queueEntity;
        }

        private void SendNotification(){
            evt_turnSystemInitialized.NextFrame(new TurnSystemInitializedEvent{});
        }

    }
}


