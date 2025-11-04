using Domain.BattleField.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Mono
{
    public static class BattleFieldEntitiesBuilder
    {
        static BattleFieldEntitiesBuilder()
        {
            _ecsWorld = World.Default;
        }
        private static World _ecsWorld;
        public static Entity SetupCellEntity(Entity entity, Vector3 global_position, Vector2Int grid_position)
        {
            Stash<PositionComponent> stash_Position = _ecsWorld.GetStash<PositionComponent>();

            if (!stash_Position.Has(entity)) { return entity; }

            // Act
            ref var CellPosition = ref stash_Position.Get(entity);
            CellPosition.m_GlobalPosition = new Vector2(global_position.x, global_position.y);
            CellPosition.m_GridPosition = new Vector2Int(grid_position.x, grid_position.y);

            return entity;
        }
    }
}

