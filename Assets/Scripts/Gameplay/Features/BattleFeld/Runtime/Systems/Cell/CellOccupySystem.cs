
using Core.Components;
using Core.Utilities.Extentions;
using Gameplay.Features.BattleField.Components;
using Gameplay.Features.BattleField.Events;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Features.BattleField.Systems{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class CellOccupySystem : ISystem
    {
        public World World { get; set; }

        private Filter _occupiedCells;

        private Event<CellOccupiedEvent> _cellOccupiedEvent;
        private Event<EntityCellPositionChangedEvent> evt_cellPostitionChanged;
        private Stash<TagOccupiedCell> stash_occupiedCell;
        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<GridPosition> stash_gridPosition;
        private Stash<TransformRefComponent> stash_transformRef;

        public void OnAwake()
        {
            _occupiedCells = World.Filter
                .With<TagOccupiedCell>()
                .Build();

            _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();
            evt_cellPostitionChanged = World.GetEvent<EntityCellPositionChangedEvent>();

            stash_occupiedCell = World.GetStash<TagOccupiedCell>();
            stash_gridPosition = World.GetStash<GridPosition>();
            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in _cellOccupiedEvent.publishedChanges)
            {
                SendNotificationCellPositionChanged(evt); // 1

                UnoccupyCell(evt); // 2

                OccupyCell(evt); // 3
            }

        }

        public void Dispose()
        {

        }
        
        
        private void SendNotificationCellPositionChanged(CellOccupiedEvent evt)
        {
            var previousCell = FindPreviousCell(evt.OccupiedBy);
            evt_cellPostitionChanged.NextFrame(new EntityCellPositionChangedEvent
            {
                entity = evt.OccupiedBy,
                oldCell = previousCell,
                newCell = evt.CellEntity
            });
        }
        
        private Entity FindPreviousCell(Entity occupier){
            foreach (var cell in _occupiedCells)
            {
                if (stash_occupiedCell.Has(cell))
                {
                    if (stash_occupiedCell.Get(cell).Occupier.Id == occupier.Id)
                    {
                        return cell;
                    }
                }
            }
            return default;
        }
        
        private void UnoccupyCell(CellOccupiedEvent evt){
            var previousCell = FindPreviousCell(evt.OccupiedBy);
            if(previousCell.IsExist()){
                stash_occupiedCell.Remove(previousCell);
            }
        }
    
        private void OccupyCell(CellOccupiedEvent evt)
        {
            var cell = evt.CellEntity;
            
            var occupier = evt.OccupiedBy;
            
            stash_occupiedCell.Set(cell, new TagOccupiedCell
            {
                Occupier = occupier
            });
            
            var c_cellPos = stash_cellPosition.Get(cell);
            if(stash_gridPosition.Has(occupier)){
                ref var c_gridPos = ref stash_gridPosition.Get(occupier);
                c_gridPos.grid_x = c_cellPos.grid_x;
                c_gridPos.grid_y = c_cellPos.grid_y;
            }
        }
    
    }
}


