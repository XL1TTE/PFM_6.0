using Gameplay.Features.DragAndDrop.Components;
using Gameplay.Features.DragAndDrop.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Features.DragAndDrop.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragAndDropCleanupSystem : ISystem
    {
        public World World { get; set; }

        private Event<DragEndedEvent> evt_dragEnded;

        private Stash<DragStateComponent> stash_dragState;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;

        public void OnAwake()
        {
            evt_dragEnded = World.GetEvent<DragEndedEvent>();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_dragEnded.publishedChanges)
            {
                stash_currentDragTarget.Remove(evt.DraggedEntity);
                stash_dragState.Remove(evt.DraggedEntity);
            }
        }

        public void Dispose()
        {

        }
    }

}

