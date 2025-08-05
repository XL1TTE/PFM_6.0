using Core.Components;
using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.DragAndDrop.Events;
using Gameplay.Features.DragAndDrop.Requests;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.DragAndDrop.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragStartSystem : ISystem
    {
        public World World { get; set; }

        private Request<StartDragRequest> req_startDrag;
        private Event<DragStartedEvent> event_dragStarted;

        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<DragStateComponent> stash_dragState;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;
        private Filter _dragState;

        public void OnAwake()
        {
            req_startDrag = World.GetRequest<StartDragRequest>();
            event_dragStarted = World.GetEvent<DragStartedEvent>();

            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_dragState = World.GetStash<DragStateComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in req_startDrag.Consume())
            {
                Entity draggedEntity = req.DraggedEntity;

                if (stash_transformRef.Has(draggedEntity))
                {
                    ref var transform = ref stash_transformRef.Get(draggedEntity).TransformRef;
                    Vector3 offset = transform.position - req.ClickWorldPos;

                    stash_dragState.Add(draggedEntity, new DragStateComponent
                    {
                        StartWorldPos = transform.position,
                        Offset = offset,
                        StartParent = transform.parent
                    });

                    stash_currentDragTarget.Add(draggedEntity);

                    event_dragStarted.NextFrame(new DragStartedEvent
                    {
                        DraggedEntity = draggedEntity
                    });
                }

            }
        }

        public void Dispose()
        {

        }
    }
}


