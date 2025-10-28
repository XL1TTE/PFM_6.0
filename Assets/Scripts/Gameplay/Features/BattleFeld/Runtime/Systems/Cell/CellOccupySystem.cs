
using Domain.BattleField.Components;
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.BattleField.Systems
{
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
        private Stash<PositionComponent> stash_Position;

        public void OnAwake()
        {
            _occupiedCells = World.Filter
                .With<TagOccupiedCell>()
                .Build();

            _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();
            evt_cellPostitionChanged = World.GetEvent<EntityCellPositionChangedEvent>();

            stash_occupiedCell = World.GetStash<TagOccupiedCell>();
            stash_Position = World.GetStash<PositionComponent>();
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
                m_Subject = evt.OccupiedBy,
                m_pCell = previousCell,
                m_nCell = evt.CellEntity
            });
        }

        private Entity FindPreviousCell(Entity occupier)
        {
            foreach (var cell in _occupiedCells)
            {
                if (stash_occupiedCell.Has(cell))
                {
                    if (stash_occupiedCell.Get(cell).m_Occupier.Id == occupier.Id)
                    {
                        return cell;
                    }
                }
            }
            return default;
        }

        private void UnoccupyCell(CellOccupiedEvent evt)
        {
            var previousCell = FindPreviousCell(evt.OccupiedBy);
            if (previousCell.IsExist())
            {
                stash_occupiedCell.Remove(previousCell);
            }
        }

        private void OccupyCell(CellOccupiedEvent evt)
        {
            var cell = evt.CellEntity;

            var occupier = evt.OccupiedBy;

            stash_occupiedCell.Set(cell, new TagOccupiedCell
            {
                m_Occupier = occupier
            });

            var c_cellPos = stash_Position.Get(cell);

            stash_Position.Set(occupier, new PositionComponent
            {
                m_GridPosition = c_cellPos.m_GridPosition,
                m_GlobalPosition = c_cellPos.m_GlobalPosition
            });
        }

    }
}


