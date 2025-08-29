using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.BattleField.Events;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Events;
using Domain.Extentions;
using Domain.Monster.Requests;
using Domain.Providers;
using Domain.Requests;
using Persistence.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Enemies{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class EnemiesSpawnSystem : ISystem
    {
        public World World { get; set; }
                
        private Filter f_enemySpawnCell;
        
        private Transform EnemiesContainer;
        
        private Request<EnemySpawnRequest> req_EnemiesSpawn;
        private Request<EntityPrefabInstantiateRequest> req_entityPfbInstansiate;
        private Event<CellOccupiedEvent> evt_cellOccupied;
        private Event<EntityPrefabInstantiatedEvent> evt_entityPfbInstantiated;
        
        private Stash<TransformRefComponent> stash_transformRef;
        
        private List<Entity> _emptyCells = new();
        private HashSet<string> _instantiateRequestGuids = new();

        public void OnAwake()
        {
            f_enemySpawnCell = World.Filter
                .With<TagEnemySpawnCell>()
                .With<TransformRefComponent>()
                .Without<TagOccupiedCell>()
                .Build();

            req_EnemiesSpawn = World.GetRequest<EnemySpawnRequest>();
            evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

            req_entityPfbInstansiate =
                World.GetRequest<EntityPrefabInstantiateRequest>();

            evt_entityPfbInstantiated =
                World.GetEvent<EntityPrefabInstantiatedEvent>();

            stash_transformRef = World.GetStash<TransformRefComponent>();

            EnemiesContainer = new GameObject("ENEMIES").transform;
            UnityEngine.Object.Instantiate(EnemiesContainer);
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var req in req_EnemiesSpawn.Consume()){
                SpawnEnemies(req);
            }
            foreach(var evt in evt_entityPfbInstantiated.publishedChanges){
                if(_instantiateRequestGuids.Contains(evt.GUID)){
                    PlaceEnemyOnEmptyCell(evt.EntityProvider.Entity);
                    _instantiateRequestGuids.Remove(evt.GUID);
                }
            }
        }

        public void Dispose()
        {

        }

        private void PlaceEnemyOnEmptyCell(Entity entity)
        {
            var cell = _emptyCells.First();
            var cellPos = stash_transformRef.Get(cell).Value.position;

            ref var enemyTransform = ref stash_transformRef.Get(entity).Value;
            enemyTransform.position = cellPos;
            
            evt_cellOccupied.NextFrame(new CellOccupiedEvent{
               OccupiedBy = entity,
               CellEntity = cell,
            });
        }


        private List<Entity> PickRandomSpawnCells(int count){
            List<Entity> result = new();
            List<Entity> _temp = new();
            foreach(var e in f_enemySpawnCell){
                _temp.Add(e);
            }
            _temp.Shuffle();
            for(int i = 0; i < System.Math.Min(count, _temp.Count); i++){
                result.Add(_temp[i]);
            }
            return result;
        }

        private void SpawnEnemies(EnemySpawnRequest req)
        {
            var e_ids = req.EnemiesIDs;
            
            var total_count = Math.Min(e_ids.Count, f_enemySpawnCell.GetLengthSlow());
            
            _emptyCells = PickRandomSpawnCells(total_count);

            for (int i = 0; i < total_count; i++)
            {
                if (DataBase.TryFindRecordByID(e_ids[i], out var e_record))
                {
                    if (DataBase.TryGetRecord<PrefabComponent>(e_record, out var e_pfb))
                    {
                        string guid = Guid.NewGuid().ToString();
                        _instantiateRequestGuids.Add(guid);

                        req_entityPfbInstansiate.Publish(new EntityPrefabInstantiateRequest{
                            GUID = guid,
                            EntityPrefab = e_pfb.Value,
                            Parent = EnemiesContainer
                        });
                    }
                }
            }
        }
    }
}
