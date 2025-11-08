
using System;
using System.Linq;
using Domain.Extentions;
using Domain.Stats.Components;
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
    public sealed class TurnProcessorSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_turnQueue;

        private Request<EndTurnRequest> req_processTurn;
        private Event<NextTurnStartedEvent> evt_nextTurnStart;
        private Event<TurnSystemInitializedEvent> evt_turnSystemInitialized;
        private Stash<CurrentTurnTakerTag> stash_turnTakerTag;
        private Stash<TurnQueueComponent> stash_turnQueue;


        public void OnAwake()
        {
            filter_turnQueue = World.Filter
                .With<TurnQueueComponent>()
                .Build();

            req_processTurn = World.GetRequest<EndTurnRequest>();

            evt_nextTurnStart = World.GetEvent<NextTurnStartedEvent>();
            evt_turnSystemInitialized = World.GetEvent<TurnSystemInitializedEvent>();

            stash_turnTakerTag = World.GetStash<CurrentTurnTakerTag>();
            stash_turnQueue = World.GetStash<TurnQueueComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_processTurn.Consume())
            {
                ProcessTurn();
            }

            foreach (var evt in evt_turnSystemInitialized.publishedChanges)
            {
                ProcessVeryFirstTurn();
            }
        }

        public void Dispose()
        {

        }

        private void SendNotifications(Entity previousTurnTaker, Entity currentTurnTaker)
        {
            evt_nextTurnStart.NextFrame(new NextTurnStartedEvent
            {
                m_CurrentTurnTaker = currentTurnTaker
            });
        }

        private void ProcessVeryFirstTurn()
        {
            if (filter_turnQueue.IsEmpty()) { return; }
            ref var queue = ref stash_turnQueue.Get(filter_turnQueue.First()).Value;
            var currentTurnTaker = queue.First();
            AddTurnTakerTag(currentTurnTaker);

            SendNotifications(currentTurnTaker, currentTurnTaker);
        }

        private void ProcessTurn()
        {
            if (filter_turnQueue.IsEmpty()) { return; }
            ref var queue = ref stash_turnQueue.Get(filter_turnQueue.First()).Value;

            Entity previousTurnTaker = default;
            Entity currentTurnTaker = default;

            if (queue.Count < 1) { return; }

            if (queue.Count == 1)
            {
                previousTurnTaker = queue.Dequeue();
                currentTurnTaker = previousTurnTaker;
                queue.Enqueue(currentTurnTaker);
            }

            else
            {
                previousTurnTaker = queue.Dequeue();
                currentTurnTaker = queue.First();
                queue.Enqueue(previousTurnTaker);
            }
            if (previousTurnTaker.IsExist())
            {
                RemoveTurnTakerTag(previousTurnTaker);
            }
            AddTurnTakerTag(currentTurnTaker);

            SendNotifications(previousTurnTaker, currentTurnTaker);
        }


        private void RemoveTurnTakerTag(Entity entity)
        {
            if (stash_turnTakerTag.Has(entity))
            {
                stash_turnTakerTag.Remove(entity);
            }
        }
        private void AddTurnTakerTag(Entity entity)
        {
            stash_turnTakerTag.Set(entity, new CurrentTurnTakerTag { });
        }

        private void Cleanup()
        {

        }

    }
}


