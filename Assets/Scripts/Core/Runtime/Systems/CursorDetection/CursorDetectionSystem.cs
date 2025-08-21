using Core.Components;
using Core.Utilities.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Core.Systems{
    // System will find all entities with draggable component which also under cursor 
    // and will mark them with UnderCursorComponent  

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorDetectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _detectors;
        private Entity _lastUnderCursor;
        private Entity _closestEntity;

        private Stash<UnderCursorComponent> stash_underCursor;
        private Stash<TagCursorDetector> stash_detectors;
        private Stash<TransformRefComponent> stash_transformRef;

        public void OnAwake()
        {
            _detectors = World.Filter
                .With<TagCursorDetector>()
                .With<TransformRefComponent>()
                .Build();

            stash_underCursor = World.GetStash<UnderCursorComponent>();
            stash_detectors = World.GetStash<TagCursorDetector>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            var mousePos = Input.mousePosition;
            var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            worldMousePos.z = 0;

            float closestDist = float.MaxValue;
            int highestPriority = int.MinValue;

            bool isAnyEntityInPickupRadius = false;

            // Find the nearest one if draggables pick radiuses is overlaped.
            foreach (var entity in _detectors)
            {
                ref var transform = ref stash_transformRef.Get(entity).TransformRef;
                var entityPos = transform.position;

                var distance = Vector2.Distance(new Vector2(worldMousePos.x, worldMousePos.y)
                    ,new Vector2(entityPos.x, entityPos.y));
                    
                var detectionData = stash_detectors.Get(entity);

                if (distance <= detectionData.DetectionRadius){
                    if (detectionData.DetectionPriority > highestPriority ||
                       (detectionData.DetectionPriority == highestPriority && distance < closestDist))
                    {
                        closestDist = distance;
                        highestPriority = detectionData.DetectionPriority;
                        _closestEntity = entity;
                        isAnyEntityInPickupRadius = true;
                    }
                }
            }

            if (_lastUnderCursor.IsExist() && !World.IsDisposed(_lastUnderCursor))
            {
                stash_underCursor.Remove(_lastUnderCursor);
            }

            if (_closestEntity.IsExist() && isAnyEntityInPickupRadius)
            {
                stash_underCursor.Set(_closestEntity, new UnderCursorComponent
                {
                    HitPoint = worldMousePos
                });
                _lastUnderCursor = _closestEntity;
            }
        }

        public void Dispose()
        {

        }
    }

}

