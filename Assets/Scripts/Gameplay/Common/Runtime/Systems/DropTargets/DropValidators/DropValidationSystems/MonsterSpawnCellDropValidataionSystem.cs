using Core.Components;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Events;
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
    public sealed class MonsterSpawnCellDropValidataionSystem : ISystem
    {
        public World World { get; set; }

        private Request<StartDragRequest> req_startDrag;

        private Event<DragEndedEvent> evt_dragEnded;
        private Event<CellOccupiedEvent> evt_cellOccupied;

        private Stash<DragStateComponent> stash_dragState;
        private Stash<CurrentDragTargetComponent> stash_currentDragTarget;
        private Stash<TransformRefComponent> stash_transformRef;
        private Stash<TagMonsterSpawnCell> stash_spawnCell;
        private Stash<TagOccupiedCell> stash_occupiedCell;
        private Stash<DropStateComponent> stash_dropState;


        public void OnAwake()
        {
            req_startDrag = World.GetRequest<StartDragRequest>();

            evt_dragEnded = World.GetEvent<DragEndedEvent>();
            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            stash_dragState = World.GetStash<DragStateComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
            stash_currentDragTarget = World.GetStash<CurrentDragTargetComponent>();
            stash_spawnCell = World.GetStash<TagMonsterSpawnCell>();
            stash_occupiedCell = World.GetStash<TagOccupiedCell>();
            stash_dropState = World.GetStash<DropStateComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_dragEnded.publishedChanges)
            {
                if(evt.WasSuccessful == false){return;}

                if (IsDropTargetMonsterSpawnCell(evt.DropTargetEntity) == false){return;}

                OccupyCell(evt.DraggedEntity, evt.DropTargetEntity);

                if (IsCellOccupied(evt.DropTargetEntity) == true){
                    Entity occupier = stash_occupiedCell.Get(evt.DropTargetEntity).Occupier;
                    if(occupier.Id == evt.DraggedEntity.Id) {return;}
                    
                    req_startDrag.Publish(new StartDragRequest{
                        ClickWorldPos = Input.mousePosition,
                        DraggedEntity = occupier,
                        StartPosition = stash_dragState.Get(evt.DraggedEntity).StartWorldPos
                    }, true);
                }
            }
        }

        public void Dispose()
        {

        }
        
        private void MarkDropAsHandled(Entity draggedEntity)
        {
            ref var dropState = ref stash_dropState.Get(draggedEntity);
            dropState.WasHandled = true;
        }
        private void AlignInCellCenter(Entity draggedEntity)
        {
            ref var transform = ref stash_transformRef.Get(draggedEntity).TransformRef;
            ref var dropPosition = ref stash_currentDragTarget.Get(draggedEntity).ValidDropPosition;
            transform.position = dropPosition;
        }

        private void OccupyCell(Entity occupier, Entity cell)
        {

            AlignInCellCenter(occupier);
            evt_cellOccupied.NextFrame(new CellOccupiedEvent
            {
                CellEntity = cell,
                OccupiedBy = occupier
            });
            MarkDropAsHandled(occupier);

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
    }

}

