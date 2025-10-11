using System.Collections.Generic;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace Core.Utilities
{
    public static class GameLogicUtility
    {
        public static List<Entity> FindMoveOptionsCellsFor(Entity entity, World world)
        {
            var gridPos = world.GetStash<GridPosition>();
            var cellPos = world.GetStash<CellPositionComponent>();
            var moveAbility = world.GetStash<MovementAbility>();

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

            var f_cellsOccupied = world.Filter
                .With<CellTag>()
                .With<CellPositionComponent>()
                .With<TagOccupiedCell>()
                .Build();

            if (gridPos.Has(attacker) == false) { return new(); }
            if (attackAbility.Has(attacker) == false) { return new(); }

            List<Entity> result = new();
            List<Vector2Int> shiftedAttacks = new();

            var attackerPos = gridPos.Get(attacker);
            var entityAttacks = attackAbility.Get(attacker);

            foreach (var attack in entityAttacks.Attacks)
            {
                shiftedAttacks.Add(attack + attackerPos.Value);
            }

            foreach (var cell in f_cellsOccupied)
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
    }
}
