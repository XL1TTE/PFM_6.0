// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Core.Utilities;
// using Domain.Abilities.Components;
// using Domain.BattleField.Events;
// using Domain.BattleField.Tags;
// using Domain.Components;
// using Domain.Events;
// using Domain.Extentions;
// using Domain.Monster.Requests;
// using Domain.Requests;
// using Persistence.Components;
// using Persistence.DB;
// using Persistence.Utilities;
// using Scellecs.Morpeh;
// using Unity.IL2CPP.CompilerServices;
// using UnityEngine;

// namespace Gameplay.Enemies
// {
//     [Il2CppSetOption(Option.NullChecks, false)]
//     [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
//     [Il2CppSetOption(Option.DivideByZeroChecks, false)]
//     public sealed class EnemiesSpawnSystem : ISystem
//     {
//         public World World { get; set; }

//         private Filter f_enemySpawnCell;

//         private Transform EnemiesContainer;

//         //private Request<EnemySpawnRequest> req_EnemiesSpawn;
//         private Request<EntityPrefabInstantiateRequest> req_entityPfbInstansiate;
//         private Event<CellOccupiedEvent> evt_cellOccupied;
//         private Event<EntityPrefabInstantiatedEvent> evt_entityPfbInstantiated;

//         private Stash<TransformRefComponent> stash_transformRef;
//         private Stash<EnemySpawnPool> stash_EnemySpawnPool;
//         private HashSet<string> _instantiateRequestGuids = new();

//         public void OnAwake()
//         {
//             f_enemySpawnCell = World.Filter
//                 .With<TagEnemySpawnCell>()
//                 .With<EnemySpawnPool>()
//                 .With<TransformRefComponent>()
//                 .Without<TagOccupiedCell>()
//                 .Build();

//             //req_EnemiesSpawn = World.GetRequest<EnemySpawnRequest>();
//             evt_cellOccupied = World.GetEvent<CellOccupiedEvent>();

//             req_entityPfbInstansiate =
//                 World.GetRequest<EntityPrefabInstantiateRequest>();

//             evt_entityPfbInstantiated =
//                 World.GetEvent<EntityPrefabInstantiatedEvent>();

//             stash_transformRef = World.GetStash<TransformRefComponent>();
//             stash_EnemySpawnPool = World.GetStash<EnemySpawnPool>();

//             EnemiesContainer = new GameObject("ENEMIES").transform;
//         }

//         public void OnUpdate(float deltaTime)
//         {
//             foreach (var e in f_enemySpawnCell)
//             {
//                 if (stash_EnemySpawnPool.Has(e) == false) { continue; }
//                 var enemyID = stash_EnemySpawnPool.Get(e).m_SpawnRule.GetEnemy();

//                 SpawnEnemy(enemyID);

//                 stash_EnemySpawnPool.Remove(e);
//             }

//             // foreach (var req in req_EnemiesSpawn.Consume())
//             // {
//             //     SpawnEnemies(req);
//             // }
//             foreach (var evt in evt_entityPfbInstantiated.publishedChanges)
//             {
//                 if (_instantiateRequestGuids.Contains(evt.GUID))
//                 {
//                     SetupAbilities(evt.EntityProvider.Entity);
//                     PlaceEnemyOnEmptyCell(evt.EntityProvider.Entity);
//                     _instantiateRequestGuids.Remove(evt.GUID);
//                 }
//             }
//         }

//         private void SetupAbilities(Entity a_enemyEntity)
//         {
//             var stash_enemyParts = World.GetStash<EnemyPartsComponent>();
//             var t_parts = stash_enemyParts.Get(a_enemyEntity);

//             var stash_abilities = World.GetStash<AbilitiesComponent>();
//             var t_abilities = new AbilitiesComponent();


//             AbilityData t_headAbility = null;
//             AbilityData t_lArmAbility = null;
//             AbilityData t_rArmAbility = null;
//             AbilityData t_lLegAbility = null;
//             AbilityData t_rLegAbility = null;
//             AbilityData t_legsAbtData = null;
//             if (DataBase.TryFindRecordByID(t_parts.m_HeadID, out var headRecord))
//             {
//                 if (DataBase.TryGetRecord<AbilityProvider>(headRecord, out var ability))
//                 {
//                     t_headAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
//                 }
//             }
//             if (DataBase.TryFindRecordByID(t_parts.m_LeftHandID, out var lArmRecord))
//             {
//                 if (DataBase.TryGetRecord<AbilityProvider>(lArmRecord, out var ability))
//                 {
//                     t_lArmAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
//                 }
//             }
//             if (DataBase.TryFindRecordByID(t_parts.m_RightHandID, out var rArmRecord))
//             {
//                 if (DataBase.TryGetRecord<AbilityProvider>(rArmRecord, out var ability))
//                 {
//                     t_rArmAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
//                 }
//             }
//             if (DataBase.TryFindRecordByID(t_parts.m_LeftLeg, out var lLegRecord))
//             {
//                 if (DataBase.TryGetRecord<AbilityProvider>(lLegRecord, out var ability))
//                 {
//                     t_lLegAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
//                 }
//             }
//             if (DataBase.TryFindRecordByID(t_parts.m_RightLeg, out var rLegRecord))
//             {
//                 if (DataBase.TryGetRecord<AbilityProvider>(rLegRecord, out var ability))
//                 {
//                     t_rLegAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
//                 }
//             }

//             if (t_rArmAbility != null && t_lArmAbility != null
//             && t_rArmAbility.m_AbilityTemplateID == t_lArmAbility.m_AbilityTemplateID)
//             {
//                 DbUtility.DoubleAbilityStats(ref t_rArmAbility.m_Value);
//                 DbUtility.DoubleAbilityStats(ref t_lArmAbility.m_Value);
//             }

//             if (t_lLegAbility != null && t_rLegAbility != null
//             && t_lLegAbility.m_AbilityTemplateID == t_rLegAbility.m_AbilityTemplateID)
//             {
//                 var movements = DbUtility.CombineShifts(t_lLegAbility.m_Shifts, t_rLegAbility.m_Shifts);
//                 t_legsAbtData = t_lLegAbility;
//                 t_legsAbtData.m_Shifts = movements;
//             }

//             t_abilities.m_HeadAbility = t_headAbility;
//             t_abilities.m_LeftHandAbility = t_lArmAbility;
//             t_abilities.m_RightHandAbility = t_rArmAbility;
//             t_abilities.m_LegsAbility = t_legsAbtData;


//             t_abilities.m_TurnAroundAbility =
//                 DbUtility.GetAbilityDataFromAbilityRecord(L.TURN_AROUND_ABILITY_ID);

//             stash_abilities.Set(a_enemyEntity, t_abilities);
//         }

//         public void Dispose()
//         {

//         }

//         // private void PlaceEnemyOnEmptyCell(Entity entity)
//         // {
//         //     var cell = _emptyCells.First();
//         //     var cellPos = stash_transformRef.Get(cell).Value.position;

//         //     ref var enemyTransform = ref stash_transformRef.Get(entity).Value;
//         //     enemyTransform.position = cellPos;

//         //     evt_cellOccupied.NextFrame(new CellOccupiedEvent
//         //     {
//         //         OccupiedBy = entity,
//         //         CellEntity = cell,
//         //     });
//         // }


//         private List<Entity> PickRandomSpawnCells(int count)
//         {
//             List<Entity> result = new();
//             List<Entity> _temp = new();
//             foreach (var e in f_enemySpawnCell)
//             {
//                 _temp.Add(e);
//             }
//             _temp.Shuffle();
//             for (int i = 0; i < System.Math.Min(count, _temp.Count); i++)
//             {
//                 result.Add(_temp[i]);
//             }
//             return result;
//         }

//         private void SpawnEnemy(string a_enemyID)
//         {
//             if (DataBase.TryFindRecordByID(a_enemyID, out var e_record))
//             {
//                 if (DataBase.TryGetRecord<PrefabComponent>(e_record, out var e_pfb))
//                 {
//                     string guid = Guid.NewGuid().ToString();
//                     _instantiateRequestGuids.Add(guid);

//                     req_entityPfbInstansiate.Publish(new EntityPrefabInstantiateRequest
//                     {
//                         GUID = guid,
//                         EntityPrefab = e_pfb.Value,
//                         Parent = EnemiesContainer
//                     });
//                 }
//             }
//         }
//     }
// }
