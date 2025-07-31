using ECS.Components;
using ECS.Components.Monsters;
using ECS.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class MonsterSpawnCellDropValidataionSystem : ISystem 
{
    public World World { get; set;}

    private Event<DragEndedEvent> evt_dragEnded;
    private Event<CellOccupiedEvent> evt_cellOccupied;
    
    private Stash<DragStateComponent> stash_dragState;
    private Stash<CurrentDragTargetComponent> stash_currentDragTarget;
    private Stash<TransformRefComponent> stash_transformRef;
    private Stash<TagMonsterSpawnCell> stash_spawnCell;
    private Stash<TagOccupiedCell> stash_occupiedCell;

    public void OnAwake() 
    {
        evt_dragEnded = World.GetEvent<DragEndedEvent>();
        evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

        stash_dragState = World.GetStash<DragStateComponent>();
        stash_transformRef = World.GetStash<TransformRefComponent>();
        stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
        stash_spawnCell = World.GetStash<TagMonsterSpawnCell>();
        stash_occupiedCell = World.GetStash<TagOccupiedCell>();
    }

    public void OnUpdate(float deltaTime) 
    {
        foreach(var evt in evt_dragEnded.publishedChanges){
            Entity draggedEntity = evt.DraggedEntity;
            bool wasSuccessful = evt.WasSuccessful;
            
            if(!wasSuccessful){
                ReturnToStartPosition(draggedEntity);
                return;
            }

            if(Validate(evt.DropTargetEntity)){
                AlignInCellCenter(draggedEntity);
                OccupyCell(draggedEntity, evt.DropTargetEntity);
            }
            else{
                ReturnToStartPosition(draggedEntity);
            }
        }
    }

    public void Dispose()
    {

    }
    
    private void ReturnToStartPosition(Entity draggedEntity){
        Vector3 originPos = stash_dragState.Get(draggedEntity).StartWorldPos;

        ref var transform = ref stash_transformRef.Get(draggedEntity).TransformRef;

        transform.position = originPos;
    }
    private void AlignInCellCenter(Entity draggedEntity){
        ref var transform = ref stash_transformRef.Get(draggedEntity).TransformRef;
        ref var dropPosition = ref stash_currentDragTarget.Get(draggedEntity).ValidDropPosition;
        transform.position = dropPosition;
    }
    
    private void OccupyCell(Entity occupier, Entity cell){
        evt_cellOccupied.NextFrame(new CellOccupiedEvent{
            CellEntity = cell,
            OccupiedBy = occupier
        });
    }
    
    private bool Validate(Entity dropTarget){
        if(!stash_spawnCell.Has(dropTarget)){return false;}
        if(stash_occupiedCell.Has(dropTarget)){return false;}
        
        return true;
    }
}
