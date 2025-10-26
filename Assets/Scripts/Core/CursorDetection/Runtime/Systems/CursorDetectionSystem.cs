using Domain.Components;
using Domain.CursorDetection.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace CursorDetection.Systems
{

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CursorDetectionSystem : ISystem
    {
        public World World { get; set; }

        private Filter _detectors;
        private Entity _lastUnderCursor;

        private Stash<UnderCursorComponent> stash_underCursor;
        private Stash<TagCursorDetector> stash_detectors;
        private Stash<HitBoxComponent> stash_hitBox;
        private Stash<TransformRefComponent> stash_transformRef;
        private Event<OnCursorEnterEvent> evt_OnPointerEnter;
        private Event<OnCursorExitEvent> evt_OnPointerExit;

        public void OnAwake()
        {
            _detectors = World.Filter
                .With<TagCursorDetector>()
                .With<TransformRefComponent>()
                .Build();

            stash_underCursor = World.GetStash<UnderCursorComponent>();
            stash_detectors = World.GetStash<TagCursorDetector>();
            stash_hitBox = World.GetStash<HitBoxComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();

            evt_OnPointerEnter = World.GetEvent<OnCursorEnterEvent>();
            evt_OnPointerExit = World.GetEvent<OnCursorExitEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            var mousePos = Input.mousePosition;
            var worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            worldMousePos.z = 0;


            Entity _closestEntity = default;
            float closestDist = float.MaxValue;
            int highestPriority = int.MinValue;

            foreach (var entity in _detectors)
            {
                ref var transform = ref stash_transformRef.Get(entity).Value;
                if (transform == null)
                {
                    throw new System.Exception($"Entity {entity.Id} have transform component but value is not assigned!");
                }
                Vector2 entityPos = transform.position;

                if (stash_hitBox.Has(entity) == false)
                {
                    throw new System.Exception($"Entity {entity.Id} does not have hit box component, which is needed for cursor detection!");
                }
                var c_hitBox = stash_hitBox.Get(entity);

                bool isInsideBox = IsPointInsideBox(
                    worldMousePos,
                    entityPos + c_hitBox.Offset,
                    c_hitBox.Size
                );

                var detectionData = stash_detectors.Get(entity);
                if (isInsideBox)
                {
                    var distance = Vector2.Distance(
                        new Vector2(worldMousePos.x, worldMousePos.y),
                        new Vector2(entityPos.x, entityPos.y)
                    );

                    if (detectionData.DetectionPriority > highestPriority ||
                       (detectionData.DetectionPriority == highestPriority && distance < closestDist))
                    {
                        closestDist = distance;
                        highestPriority = detectionData.DetectionPriority;
                        _closestEntity = entity;
                    }
                }
            }

            if (_lastUnderCursor.IsExist() && !World.IsDisposed(_lastUnderCursor))
            {
                if (!_closestEntity.IsExist() || _closestEntity != _lastUnderCursor)
                {

                    evt_OnPointerExit.NextFrame(new OnCursorExitEvent
                    {
                        m_Entity = _lastUnderCursor
                    });

                    stash_underCursor.Remove(_lastUnderCursor);
                }
            }

            if (_closestEntity.IsExist() && !World.IsDisposed(_closestEntity))
            {
                if (!_lastUnderCursor.IsExist() || _closestEntity != _lastUnderCursor)
                {
                    evt_OnPointerEnter.NextFrame(new OnCursorEnterEvent
                    {
                        m_Entity = _closestEntity,
                    });
                }

                stash_underCursor.Set(_closestEntity, new UnderCursorComponent
                {
                    HitPoint = worldMousePos
                });
                _lastUnderCursor = _closestEntity;
            }
            else if (_lastUnderCursor.IsExist())
            {
                _lastUnderCursor = default;
            }
        }

        public void Dispose()
        {

        }

        private bool IsPointInsideBox(Vector3 point, Vector2 boxCenter, Vector2 boxSize)
        {
            float halfWidth = boxSize.x * 0.5f;
            float halfHeight = boxSize.y * 0.5f;

            return point.x >= boxCenter.x - halfWidth &&
                   point.x <= boxCenter.x + halfWidth &&
                   point.y >= boxCenter.y - halfHeight &&
                   point.y <= boxCenter.y + halfHeight;
        }
    }

}

