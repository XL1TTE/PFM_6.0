
using ECS.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Project{

    public static partial class Domain
    {
        public static class BattleField
        {
            public static Entity SetupCellEntity(Entity entity, Vector3 global_position, Vector2Int grid_position)
            {
                Stash<CellPositionComponent> CellPositions = _ecsWorld.GetStash<CellPositionComponent>();

                // Act
                ref var CellPosition = ref CellPositions.Get(entity);
                CellPosition.global_x = global_position.x;
                CellPosition.global_y = global_position.y;
                CellPosition.grid_x = grid_position.x;
                CellPosition.grid_y = grid_position.y;

                return entity;
            }
        }
    }

}
