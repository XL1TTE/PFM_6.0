using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enemies.Tags;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TurnSystem.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Requests;
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
        private Filter filter_enemies;
        
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
                
            filter_enemies = World.Filter
                .With<TagEnemy>()
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
            List<Entity> OrderedBySpeed = new();
            Entity queueEntity = World.CreateEntity();
            
            foreach (var monster in filter_monsters)
            {
                OrderedBySpeed.Add(monster);
            }
            foreach (var enemy in filter_enemies)
            {
                OrderedBySpeed.Add(enemy);
            }
            OrderedBySpeed = OrderedBySpeed.OrderByDescending(e =>
            {
                var speed = 0.0f;
                if (stash_Speed.Has(e))
                {
                    speed = stash_Speed.Get(e).Value;
                }
                return speed;
            }).ToList();

            Queue<Entity> TurnQueue = new Queue<Entity>(OrderedBySpeed);

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


