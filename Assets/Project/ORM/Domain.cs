
using ECS.Components;
using Scellecs.Morpeh;

using UnityEngine;

namespace Project.Domain{
    
    public static class Domain{
        public static readonly World _ecsWorld = World.Default;
        
        public static class BattleField{
            public static Entity CreateCellEntity(Vector2 global_position, Vector2Int grid_position){
                // Arrange
                Entity entity = _ecsWorld.CreateEntity();

                Stash<TagBattleFieldCell> CellTags = _ecsWorld.GetStash<TagBattleFieldCell>();
                Stash<BattleFieldCellPosition> CellPositions = _ecsWorld.GetStash<BattleFieldCellPosition>();

                Vector3 SpawnPosition = new Vector3(global_position.x, global_position.y, 0.0f);

                // Act
                CellTags.Add(entity);
                
                ref var CellPosition = ref CellPositions.Add(entity);
                CellPosition.global_x = global_position.x;
                CellPosition.global_y = global_position.y;
                CellPosition.grid_x = grid_position.x;
                CellPosition.grid_y = grid_position.y;
                
                return entity;
            }
        }

        public static void DeleteEntity(Entity entity)
        {
            _ecsWorld.RemoveEntity(entity);
        }

    } 
}
