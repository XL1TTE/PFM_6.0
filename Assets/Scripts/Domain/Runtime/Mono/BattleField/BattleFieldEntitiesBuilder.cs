using Domain.BattleField.Components;
using Game;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Mono
{
    public static class BattleFieldEntitiesBuilder
    {

        public static Entity SetupCellEntity(Entity entity, Vector3 global_position, Vector2Int grid_position, World a_world)
        {
            Stash<PositionComponent> stash_Position = a_world.GetStash<PositionComponent>();

            if (!stash_Position.Has(entity)) { return entity; }

            // Act
            ref var CellPosition = ref stash_Position.Get(entity);
            CellPosition.m_GlobalPosition = new Vector2(global_position.x, global_position.y);
            CellPosition.m_GridPosition = new Vector2Int(grid_position.x, grid_position.y);

            return entity;
        }
    }
}

