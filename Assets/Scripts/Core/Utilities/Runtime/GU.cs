using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.TargetSelection.Events;
using GluonGui.Dialog;
using Scellecs.Morpeh;
using UnityEngine;

namespace Core.Utilities
{
    public static class GU
    {
        public static List<Entity> FindMoveOptionsCellsFor(Entity entity, World world)
        {
            var gridPos = world.GetStash<GridPosition>();
            var cellPos = world.GetStash<CellPositionComponent>();
            var moveAbility = world.GetStash<MovementAbility>();
            var lookDir = world.GetStash<LookDirection>();

            var f_cellsNotOccupied = world.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Without<TagOccupiedCell>()
                .Build();

            if (moveAbility.Has(entity) == false) { return new(); }
            if (gridPos.Has(entity) == false) { return new(); }

            List<Entity> result = new();
            List<Vector2Int> shiftedMoves = new();


            var entityMoves = moveAbility.Get(entity).Movements;
            var entityGridPos = gridPos.Get(entity);

            if (lookDir.Has(entity))
            {
                switch (lookDir.Get(entity).m_Value)
                {
                    case Directions.LEFT:
                        entityMoves = entityMoves.Select((m) => m *= new Vector2Int(-1, 1)).ToList();
                        break;
                    case Directions.RIGHT:
                        // Movements setuped for right direction by default;
                        break;
                }
            }

            foreach (var move in entityMoves)
            {
                shiftedMoves.Add(move + entityGridPos.Value);
            }

            foreach (var cell in f_cellsNotOccupied)
            {
                var c_cellPos = cellPos.Get(cell);
                if (shiftedMoves.Contains(new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y)))
                {
                    result.Add(cell);
                }
            }

            return result;
        }

        public static List<Entity> FindAttackOptionsCellsFor(Entity attacker, World world)
        {
            var gridPos = world.GetStash<GridPosition>();
            var cellPos = world.GetStash<CellPositionComponent>();
            var attackAbility = world.GetStash<AttackAbility>();
            var lookDir = world.GetStash<LookDirection>();

            var f_cells = world.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Build();

            if (gridPos.Has(attacker) == false) { return new(); }
            if (attackAbility.Has(attacker) == false) { return new(); }

            List<Entity> result = new();
            List<Vector2Int> shiftedAttacks = new();

            var attackerPos = gridPos.Get(attacker);
            var entityAttacks = attackAbility.Get(attacker).Attacks;

            if (lookDir.Has(attacker))
            {
                switch (lookDir.Get(attacker).m_Value)
                {
                    case Directions.LEFT:
                        entityAttacks = entityAttacks.Select((m) => m *= new Vector2Int(-1, 1)).ToList();
                        break;
                    case Directions.RIGHT:
                        // Movements setuped for right direction by default;
                        break;
                }
            }


            foreach (var attack in entityAttacks)
            {
                shiftedAttacks.Add(attack + attackerPos.Value);
            }

            foreach (var cell in f_cells)
            {
                var c_cellPos = cellPos.Get(cell);
                var cellPosGrid = new Vector2Int(c_cellPos.grid_x, c_cellPos.grid_y);

                if (shiftedAttacks.Contains(cellPosGrid))
                {
                    result.Add(cell);
                }
            }
            return result;
        }

        public static Vector2Int GetEntityPositionOnCell(Entity entity, World world)
        {
            var stash_gridPos = world.GetStash<GridPosition>();
            if (stash_gridPos.Has(entity) == false) { return Vector2Int.zero; }

            return stash_gridPos.Get(entity).Value;
        }
        public static IEnumerable<Entity> GetCellsFromShifts(Vector2Int a_basis, IEnumerable<Vector2Int> a_shifts, World a_world)
        {
            var f_cells = a_world.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .Build();
            var stash_pos = a_world.GetStash<CellPositionComponent>();

            IEnumerable<Vector2Int> t_calcPos = a_shifts.Select(x => x + a_basis);

            List<Entity> t_result = new(f_cells.GetLengthSlow());
            foreach (var cell in f_cells)
            {
                var cellPos = stash_pos.Get(cell);
                var cellPosVector = new Vector2Int(cellPos.grid_x, cellPos.grid_y);
                if (t_calcPos.Contains(cellPosVector))
                {
                    t_result.Add(cell);
                }
            }
            return t_result;
        }

        public static ref TargetSelectionResult GetTargetSelectionResult(Entity owner, World world)
        {
            var results = world.GetStash<TargetSelectionResult>();
            if (results.Has(owner))
            {
                return ref results.Get(owner);
            }
            throw new System.Exception($"Target selection result was not found on Entity: {owner.Id}.");
        }

        public static Entity GetCellOccupier(Entity cell, World world)
        {
            var occupiedCell = world.GetStash<TagOccupiedCell>();
            if (occupiedCell.Has(cell))
            {
                return occupiedCell.Get(cell).Occupier;
            }
            else
            {
                throw new System.Exception("You trying to get cell occupier from not occupied cell.");
            }
        }

        public static IEnumerable<Entity> GetCellOccupiers(IEnumerable<Entity> cells, World world)
        {
            var occupiedCell = world.GetStash<TagOccupiedCell>();
            var result = new List<Entity>();

            foreach (var cell in cells)
            {
                if (occupiedCell.Has(cell))
                {
                    result.Add(occupiedCell.Get(cell).Occupier);
                }
                else
                {
                    throw new System.Exception("You trying to get cell occupier from not occupied cell.");
                }
            }
            return result;
        }

    }

}


