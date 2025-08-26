using System;
using System.Collections.Generic;
using System.Linq;
using Domain.BattleField.Components;
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Monster.Mono;
using Domain.Monster.Requests;
using Persistence.Buiders;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.Monster.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class MonsterSpawnSystem : ISystem
    {
        public World World { get; set; }

        private Filter _freeMonsterSpawnCells;
        

        private Stash<CellPositionComponent> stash_cellPosition;
        private Stash<TransformRefComponent> stash_monsterTransform;
        private Stash<GridPosition> stash_gridPosition;

        private Request<SpawnMonstersRequest> _spawnRequests;
        private Event<CellOccupiedEvent> _cellOccupiedEvent;
        
        
        public void OnAwake()
        {
            _freeMonsterSpawnCells = World.Filter
                .With<TagMonsterSpawnCell>()
                .Without<TagOccupiedCell>()
                .With<CellPositionComponent>()
                .Build();

            stash_cellPosition = World.GetStash<CellPositionComponent>();
            stash_monsterTransform = World.GetStash<TransformRefComponent>();
            stash_gridPosition = World.GetStash<GridPosition>();

            _spawnRequests = World.GetRequest<SpawnMonstersRequest>();
            _cellOccupiedEvent = World.GetEvent<CellOccupiedEvent>();

        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var req in _spawnRequests.Consume())
            {
                SpawnNewMonster(req);
            }
        }

        public void Dispose()
        {
        }

        private List<Entity> PickRandomFreeCells(int MonsterCount)
        {
            int lenght = _freeMonsterSpawnCells.GetLengthSlow();

            List<Entity> cells = new();
            List<Entity> result = new();

            foreach (var cell in _freeMonsterSpawnCells)
            {
                cells.Add(cell);
            }

            var cycles = Math.Min(MonsterCount, lenght);

            for (int i = 0; i < cycles; i++)
            {
                var pick = UnityEngine.Random.Range(0, cells.Count);
                result.Add(cells[pick]);
                cells.RemoveAt(pick);
            }
            return result;
        }


        private MonsterBuilder MonsterDataToBuilder(MosnterData mosnterData){
            var builder = new MonsterBuilder(World)
                .AttachHead(mosnterData.Head_id)
                .AttachBody(mosnterData.Body_id)
                .AttachFarArm(mosnterData.FarArm_id)
                .AttachNearArm(mosnterData.NearArm_id)
                .AttachNearLeg(mosnterData.NearLeg_id)
                .AttachFarLeg(mosnterData.FarLeg_id);
            return builder;
        }

        private void SpawnNewMonster(SpawnMonstersRequest req)
        {
            var free_cells = PickRandomFreeCells(req.Monsters.Count);
            
            for(int i = 0; i < free_cells.Count; i++){
                var cell = free_cells[i];
                var monsterData = req.Monsters[i];

                var monster = MonsterDataToBuilder(monsterData);

                SetupMonster(monster.Build(), cell);
            }
        }
        
        private void SetupMonster(Entity monster, Entity SpawnCell){
            
            var cellPos = stash_cellPosition.Get(SpawnCell);
            
            if (!stash_monsterTransform.Has(monster))
            {
                throw new Exception("Monster entity does not have a transform component. Can't spawn monster without it!");
            }
            ref var monsterTransform = ref stash_monsterTransform.Get(monster);
            monsterTransform.Value.position =
                new UnityEngine.Vector3(cellPos.global_x, cellPos.global_y, cellPos.global_y * 0.01f);

            stash_gridPosition.Add(monster, new GridPosition
            {
                grid_x = cellPos.grid_x,
                grid_y = cellPos.grid_y
            });

            _cellOccupiedEvent.NextFrame(new CellOccupiedEvent
            {
                CellEntity = SpawnCell,
                OccupiedBy = monster
            });
        }
    }
}


