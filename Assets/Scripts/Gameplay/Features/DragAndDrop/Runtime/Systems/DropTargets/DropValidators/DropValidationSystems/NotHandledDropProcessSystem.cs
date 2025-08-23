
using Domain.Components;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.DragAndDrop.Validators
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NotHandledDropProcessSystem : ISystem
    {
        public World World { get; set; }

        private Event<DragEndedEvent> evt_dragEnded;

        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<DragStateComponent> stash_dragState;

        private Stash<DropStateComponent> stash_dropState;


        public void OnAwake()
        {
            evt_dragEnded = World.GetEvent<DragEndedEvent>();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();

            stash_dropState = World.GetStash<DropStateComponent>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var evt in evt_dragEnded.publishedChanges){
                if(stash_dropState.Get(evt.DraggedEntity).WasHandled == false)
                {
                    ReturnToStartPosition(evt.DraggedEntity);
                }
            }
        }

        public void Dispose()
        {

        }

        private void ReturnToStartPosition(Entity draggedEntity)
        {
            Vector3 originPos = stash_dragState.Get(draggedEntity).StartWorldPos;

            ref var transform = ref stash_transformRef.Get(draggedEntity).Value;

            transform.position = originPos;
        }
    }

}

