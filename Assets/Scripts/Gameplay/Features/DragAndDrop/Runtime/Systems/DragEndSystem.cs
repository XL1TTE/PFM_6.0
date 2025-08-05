using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.DragAndDrop.Events;
using Gameplay.Features.DragAndDrop.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Experimental.AI;

namespace Gameplay.Features.DragAndDrop.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragEndSystem : ISystem
    {
        public World World { get; set; }

        private Filter _draggingEntities;

        private Request<EndDragRequest> req_endDrag;

        private Event<DragEndedEvent> evt_dragEnded;

        private Stash<DragStateComponent> stash_dragState;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;
        private Stash<DropStateComponent> stash_dropState;

        public void OnAwake()
        {
            _draggingEntities = World.Filter
                .With<DragStateComponent>()
                .With<DraggableComponent>()
                .Build();

            req_endDrag = World.GetRequest<EndDragRequest>();

            evt_dragEnded = World.GetEvent<DragEndedEvent>();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
            stash_dropState = World.GetStash<DropStateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_endDrag.Consume())
            {

                foreach (var entity in _draggingEntities)
                {
                    ref var dragState = ref stash_dragState.Get(entity);
                    ref var dragTarget = ref stash_currentDragTarget.Get(entity);

                    var endEvent = new DragEndedEvent
                    {
                        DraggedEntity = entity,
                        DropTargetEntity = dragTarget.TargetEntity,
                        WasSuccessful = false
                    };

                    //Validation
                    if (dragTarget.IsValid)
                    {
                        endEvent.WasSuccessful = true;
                    }

                    stash_dropState.Add(entity, new DropStateComponent{WasHandled = false});

                    evt_dragEnded.NextFrame(endEvent);
                }
            }
        }

        public void Dispose()
        {

        }
    }
}


