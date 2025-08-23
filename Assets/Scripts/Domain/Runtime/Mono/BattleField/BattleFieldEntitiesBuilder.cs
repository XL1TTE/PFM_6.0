using Domain.BattleField.Components;
using Scellecs.Morpeh;
using UnityEngine;

namespace Domain.BattleField.Mono
{
    public static class BattleFieldEntitiesBuilder
    {
        static BattleFieldEntitiesBuilder(){
            _ecsWorld = World.Default;
        }
        private static World _ecsWorld;
        public static Entity SetupCellEntity(Entity entity, Vector3 global_position, Vector2Int grid_position)
        {
            Stash<CellPositionComponent> CellPositions = _ecsWorld.GetStash<CellPositionComponent>();

            if(!CellPositions.Has(entity)){return entity;}

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

