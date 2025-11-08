
using System.Collections.Generic;
using Domain.TurnSystem.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Domain.UI.Mono;
using Domain.UI.Widgets;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.TurnSystem.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TurnQueueRenderSystem : ISystem
    {
        public World World { get; set; }

        private Filter filter_currentTurnTaker;

        private Filter filter_turnQueue;

        private Event<NextTurnStartedEvent> evt_turnStarted;
        private Event<TurnSystemInitializedEvent> evt_turnSystemInitialized;

        private Stash<TurnQueueComponent> stash_turnQueue;
        private Stash<TurnQueueAvatar> stash_turnQueueAvatar;

        private Entity _previousTurnTaker;

        private Dictionary<int, TurnQueueElementView> QueueRenderMap = new();

        public void OnAwake()
        {
            filter_currentTurnTaker = World.Filter.With<CurrentTurnTakerTag>().Build();

            filter_turnQueue = World.Filter.With<TurnQueueComponent>().Build();

            evt_turnStarted = World.GetEvent<NextTurnStartedEvent>();
            evt_turnSystemInitialized = World.GetEvent<TurnSystemInitializedEvent>();

            stash_turnQueue = World.GetStash<TurnQueueComponent>();
            stash_turnQueueAvatar = World.GetStash<TurnQueueAvatar>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_turnStarted.publishedChanges)
            {
                RenderTurn();
            }

            foreach (var evt in evt_turnSystemInitialized.publishedChanges)
            {
                PrepareQueueView();
            }
        }

        public void Dispose()
        {
            QueueRenderMap = null;
        }

        private void RenderTurn()
        {
            if (filter_currentTurnTaker.IsEmpty()) { return; }

            var turnTaker = filter_currentTurnTaker.First();

            if (QueueRenderMap.TryGetValue(_previousTurnTaker.Id, out var pt_render))
            {
                pt_render.DisableHighlighting();
            }
            if (QueueRenderMap.TryGetValue(turnTaker.Id, out var ct_render))
            {
                ct_render.EnableHighlighting();
            }
            _previousTurnTaker = turnTaker;
        }

        private void PrepareQueueView()
        {
            if (filter_turnQueue.IsEmpty()) { return; }

            QueueRenderMap.Clear(); // reset

            var queue = stash_turnQueue.Get(filter_turnQueue.First());
            foreach (var elt in queue.Value)
            {
                var elt_view = BattleFieldUIRefs.Instance.TurnQueueWidget.AddNewInQueue();
                QueueRenderMap.Add(elt.Id, elt_view);

                if (stash_turnQueueAvatar.Has(elt))
                {
                    elt_view.SetAvatar(stash_turnQueueAvatar.Get(elt).m_Value);
                }
            }
        }
    }
}


