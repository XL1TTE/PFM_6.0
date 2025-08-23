
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.DragAndDrop.Components;
using Domain.DragAndDrop.Events;
using Domain.DragAndDrop.Requests;
using Domain.Extentions;
using Domain.Monster.Tags;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.DragAndDrop.Validators
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterSpawnCellDropValidataionSystem : ISystem
    {
        public World World { get; set; }
        
        private Filter _occupiedMonsterSpawnCells;

        private Request<StartDragRequest> req_startDrag;

        private Event<DragEndedEvent> evt_dragEnded;
        private Event<CellOccupiedEvent> evt_cellOccupied;

        private Stash<DragStateComponent> stash_dragState;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;
        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<TagMonsterSpawnCell> stash_spawnCell;
        private Stash<TagOccupiedCell> stash_occupiedCell;
        private Stash<DropStateComponent> stash_dropState;
        private Stash<TagMonster> stash_monsterTag;


        public void OnAwake()
        {
            _occupiedMonsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .With<TagOccupiedCell>()
                .Build();

            req_startDrag = World.GetRequest<StartDragRequest>();

            evt_dragEnded = World.GetEvent<DragEndedEvent>();
            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
            stash_spawnCell = World.GetStash<TagMonsterSpawnCell>();
            stash_occupiedCell = World.GetStash<TagOccupiedCell>();
            stash_dropState = World.GetStash<DropStateComponent>();
            stash_monsterTag = World.GetStash<TagMonster>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_dragEnded.publishedChanges)
            {
                if(Validate(evt) == false){return;} // if monster droped on monster spawn cell
                
                var cellToDrop = evt.DropTargetEntity;
                var draggedMonster = evt.DraggedEntity;

                if (IsCellOccupied(cellToDrop) == true){
                    Entity occupierOfCellToDrop = stash_occupiedCell.Get(cellToDrop).Occupier;
                    
                    if(occupierOfCellToDrop.Id == draggedMonster.Id) {return;} // if try to drop on the same cell
                    
                    req_startDrag.Publish(new StartDragRequest{ // starts dragging for cell occupier
                        ClickWorldPos = Input.mousePosition,
                        DraggedEntity = occupierOfCellToDrop,
                        StartPosition = stash_dragState.Get(draggedMonster).StartWorldPos
                    }, true);

                    var draggedMonsterCell = FindOccupiedCell(draggedMonster);
                    
                    if(draggedMonsterCell.IsExist()){
                        OccupyCell(occupierOfCellToDrop, draggedMonsterCell);
                    }

                }

                OccupyCell(draggedMonster, cellToDrop);
                MarkDropAsHandled(draggedMonster);
            }
        }

        public void Dispose()
        {

        }
        
        private Entity FindOccupiedCell(Entity occupier){
            foreach(var e in _occupiedMonsterSpawnCells){
                if(stash_occupiedCell.Get(e).Occupier.Id == occupier.Id){return e;}
            }
            return default;
        }
        
        private void MarkDropAsHandled(Entity draggedEntity)
        {
            ref var dropState = ref stash_dropState.Get(draggedEntity);
            dropState.WasHandled = true;
        }
        private void AlignInCellCenter(Entity draggedEntity, Entity cell)
        {
            ref var transform = ref stash_transformRef.Get(draggedEntity).Value;
            var dropPosition =  stash_transformRef.Get(cell).Value.position;
            transform.position = dropPosition;
            transform.position += new Vector3(0, 0, dropPosition.y * 0.01f);
        }

        private void OccupyCell(Entity occupier, Entity cell)
        {
            AlignInCellCenter(occupier, cell);
            evt_cellOccupied.NextFrame(new CellOccupiedEvent
            {
                CellEntity = cell,
                OccupiedBy = occupier
            });
        }

        private bool IsDropTargetMonsterSpawnCell(Entity dropTarget)
        {
            if (stash_spawnCell.Has(dropTarget)) { return true; }
            return false;
        }
        private bool IsCellOccupied(Entity dropTarget)
        {
            if (stash_occupiedCell.Has(dropTarget)) { return true; }

            return false;
        }
    
    
        private bool Validate(DragEndedEvent evt){
            if(stash_monsterTag.Has(evt.DraggedEntity) == false ){return false;}
            if (evt.WasSuccessful == false) { return false; }
            if (IsDropTargetMonsterSpawnCell(evt.DropTargetEntity) == false) { return false; }
            
            return true;
        }
    }

}

