using Extantions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

// System will find all entities with draggable component which also under cursor 
// and will mark them with UnderCursorComponent  

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class CursorDetectionSystem : ISystem 
{
    public World World { get; set;}

    private Filter _draggables;
    private Entity _lastUnderCursor;
    private Entity _closestEntity;
        
    private Stash<UnderCursorComponent> stash_underCursor;
    private Stash<DraggableComponent> stash_draggable;
    private Stash<TransformRefComponent> stash_transformRef;

    public void OnAwake() 
    {
        _draggables = World.Filter
            .With<DraggableComponent>()
            .With<TransformRefComponent>()
            .Build();

        stash_underCursor = World.GetStash<UnderCursorComponent>();
        stash_draggable = World.GetStash<DraggableComponent>();
        stash_transformRef = World.GetStash<TransformRefComponent>();
    }

    public void OnUpdate(float deltaTime) 
    {
        var mousePos = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mousePos);

        float closestDist = float.MaxValue;
        
        bool isAnyEntityInPickupRadius = false;

        // Find the nearest one if draggables pick radiuses is overlaped.
        foreach (var entity in _draggables)
        {
            ref var transform = ref stash_transformRef.Get(entity).TransformRef;
            var distance = Vector3.Cross(ray.direction, transform.position - ray.origin).magnitude;

            if (distance <= stash_draggable.Get(entity).PickRadius && distance < closestDist)
            {
                closestDist = distance;
                _closestEntity = entity;

                isAnyEntityInPickupRadius = true;
            }
        }

        if(_lastUnderCursor.IsExist())
        {
            stash_underCursor.Remove(_lastUnderCursor);
        }

        if (_closestEntity.IsExist() && isAnyEntityInPickupRadius)
        {
            stash_underCursor.Set(_closestEntity, new UnderCursorComponent{
                HitPoint = ray.origin + ray.direction * closestDist
            });
            _lastUnderCursor = _closestEntity;
        }
    }

    public void Dispose()
    {

    }
}
