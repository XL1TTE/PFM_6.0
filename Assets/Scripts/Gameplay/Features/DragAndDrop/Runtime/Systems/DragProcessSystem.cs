
using Core.Components;
using Gameplay.Features.DragAndDrop.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Features.DragAndDrop.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DragProcessSystem : ISystem
    {
        public World World { get; set; }

        private Filter _draggingEntities;
        private Filter _dropTargetEntities;

        private Entity _closestDropTargetEntity;

        private Stash<DragStateComponent> stash_dragState;
        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<DropTargetComponent> stash_dropTarget;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;

        public void OnAwake()
        {
            _draggingEntities = World.Filter
                .With<DragStateComponent>()
                .Build();

            _dropTargetEntities = World.Filter
                .With<DropTargetComponent>()
                .With<TransformRefComponent>()
                .Build();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_dropTarget = World.GetStash<DropTargetComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_draggingEntities.IsEmpty()) { return; }

            var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.forward, Vector3.zero);
            plane.Raycast(mouseRay, out var distance);
            Vector2 mouseWorldPos = mouseRay.GetPoint(distance);

            foreach (var entity in _draggingEntities)
            {
                ref var state = ref stash_dragState.Get(entity);
                ref var transform = ref stash_transformRef.Get(entity).TransformRef;

                transform.position = mouseWorldPos;
                // adds offset for fix z-fighting
                transform.position += new Vector3(0, 0, -5);

                UpdateDropTarget(entity, mouseWorldPos);
            }
        }

        public void Dispose()
        {

        }

        private void UpdateDropTarget(Entity draggedEntity, Vector3 mouseWorldPos)
        {
            //if(!stash_currentDragTarget.Has(draggedEntity)){return;}

            ref var currentTarget = ref stash_currentDragTarget.Get(draggedEntity);

            float closestDist = float.MaxValue;

            bool isDropTargetFound = false;

            foreach (var entity in _dropTargetEntities)
            {
                ref var transform = ref stash_transformRef.Get(entity).TransformRef;
                float distance = Vector3.Distance(mouseWorldPos, transform.position);
                float dropRadius = stash_dropTarget.Get(entity).DropRadius;

                if (distance > dropRadius) continue;

                if (distance < closestDist)
                {
                    closestDist = distance;
                    _closestDropTargetEntity = entity;

                    isDropTargetFound = true;
                }
            }

            if (isDropTargetFound)
            {
                currentTarget.TargetEntity = _closestDropTargetEntity;
                currentTarget.ValidDropPosition = stash_transformRef.Get(currentTarget.TargetEntity)
                    .TransformRef.position;

                currentTarget.IsValid = true;
            }
            else
            {
                currentTarget.ValidDropPosition = Vector3.zero;
                currentTarget.IsValid = false;
            }
        }
    }

}

