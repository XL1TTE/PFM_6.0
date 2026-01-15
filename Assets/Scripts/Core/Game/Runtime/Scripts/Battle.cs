using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Domain.Monster.Mono;
using Interactions;
using Persistence.Buiders;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Persistence.Utilities;
using Project;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Game
{
    public static partial class Battle
    {

        private static List<MonsterData> MONSTERS_TO_SPAWN = new List<MonsterData>{
                    new MonsterData(
                        "bp_pig-head",
                        "bp_rat-arm",
                        "bp_cow-arm",
                        "bp_pig-torso",
                        "bp_pig-leg",
                        "bp_rat-leg",
                        4,
                        "Nikita"),
                };
        public static void SpawnMonstersOnLoad(World a_world)
        {
            var t_filter = a_world.Filter
                .With<TagMonsterSpawnCell>()
                .Without<TagOccupiedCell>()
                .With<PositionComponent>()
                .Build();


            var storageMonsters = DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>().crusade_monsters;

            if (storageMonsters != null)
            {
                if (storageMonsters.Count > 0)
                {
                    MONSTERS_TO_SPAWN = storageMonsters;
                }
            }

            var t_spawnCells = F.FilterEmptyCells(t_filter.AsEnumerable(), a_world).ToArray();
            for (int i = 0; i < Math.Min(t_spawnCells.Length, MONSTERS_TO_SPAWN.Count); ++i)
            {
                var t_cell = t_spawnCells[i];
                var monsterData = MONSTERS_TO_SPAWN[i];

                SpawnMonsterAsync(monsterData, t_cell, a_world).Forget();
            }
        }

        private static async UniTask SpawnMonsterAsync(MonsterData a_monsterData, Entity a_cell, World a_world)
        {
            var t_monsterEntity = GetMonsterBuilder(a_monsterData, a_world).Build();

            await UniTask.Yield();

            PlaceOnCell(t_monsterEntity, a_cell, a_world);
            G.OccupyCell(t_monsterEntity, a_cell, a_world);

            Interactor.CallAll<IOnEntityCellPositionChanged>(async handler =>
            {
                await handler.OnPositionChanged(a_cell, a_cell, t_monsterEntity, a_world);
            }).Forget();
        }

        private static MonsterBuilder GetMonsterBuilder(MonsterData mosnterData, World a_world)
        {
            var builder = new MonsterBuilder(a_world)
                .GiveName(mosnterData.m_MonsterName)
                .AttachHead(mosnterData.Head_id)
                .AttachBody(mosnterData.Body_id)
                .AttachFarArm(mosnterData.FarArm_id)
                .AttachNearArm(mosnterData.NearArm_id)
                .AttachNearLeg(mosnterData.NearLeg_id)
                .AttachFarLeg(mosnterData.FarLeg_id)
                .SetHealth(mosnterData.current_hp);
            return builder;
        }


        public static void SpawnEnemiesOnLoad(World a_world)
        {
            var t_filter = a_world.Filter
                .With<TagEnemySpawnCell>()
                .With<EnemySpawnPool>()
                .With<TransformRefComponent>()
                .Without<TagOccupiedCell>()
                .Build();

            var t_requests = a_world.GetStash<EnemySpawnPool>();

            foreach (var e in t_filter)
            {
                if (t_requests.Has(e) == false) { continue; }
                var enemyID = t_requests.Get(e).m_SpawnRule.GetEnemy();

                SpawnEnemy(enemyID, e, a_world).Forget();

                t_requests.Remove(e);
            }
        }

        private static async UniTask SpawnEnemy(string a_enemyID, Entity a_cell, World a_world)
        {
            if (DataBase.TryFindRecordByID(a_enemyID, out var e_record))
            {
                if (DataBase.TryGetRecord<PrefabComponent>(e_record, out var e_pfb))
                {
                    var enemy = UnityEngine.Object.Instantiate(e_pfb.Value);

                    await UniTask.Yield();

                    if (enemy.TryFindComponent<EntityProvider>(out var unknown))
                    {
                        var t_enemyEntity = unknown.Entity;

                        PlaceOnCell(t_enemyEntity, a_cell, a_world);
                        G.OccupyCell(t_enemyEntity, a_cell, a_world);
                        SetupAbilities(t_enemyEntity, a_world);

                        BattleReward.AddRewardsToPool(GetLootRewards(t_enemyEntity, a_world));

                        Interactor.CallAll<IOnEntityCellPositionChanged>(async handler =>
                        {
                            await handler.OnPositionChanged(a_cell, a_cell, t_enemyEntity, a_world);
                        }).Forget();
                    }
                }
            }
        }

        private static void PlaceOnCell(Entity a_entity, Entity a_cell, World a_world)
        {
            var t_transform = a_world.GetStash<TransformRefComponent>();
            var cellPos = t_transform.Get(a_cell).Value.position;

            ref var enemyTransform = ref t_transform.Get(a_entity).Value;
            enemyTransform.position = cellPos;
        }

        private static EnemyLootWrapper[] GetLootRewards(Entity a_enemyEntity, World a_world)
        {
            var stash_enemyLoot = a_world.GetStash<EnemyLootComponent>();
            var t_loot = stash_enemyLoot.Get(a_enemyEntity);

            return t_loot.loot;
        }

        private static void SetupAbilities(Entity a_enemyEntity, World a_world)
        {
            var stash_enemyParts = a_world.GetStash<EnemyPartsComponent>();
            var t_parts = stash_enemyParts.Get(a_enemyEntity);

            var stash_abilities = a_world.GetStash<AbilitiesComponent>();
            var t_abilities = new AbilitiesComponent();


            AbilityData t_headAbility = null;
            AbilityData t_lArmAbility = null;
            AbilityData t_rArmAbility = null;
            AbilityData t_lLegAbility = null;
            AbilityData t_rLegAbility = null;
            AbilityData t_legsAbtData = null;
            if (DataBase.TryFindRecordByID(t_parts.m_HeadID, out var headRecord))
            {
                if (DataBase.TryGetRecord<AbilityProvider>(headRecord, out var ability))
                {
                    t_headAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
                }
            }
            if (DataBase.TryFindRecordByID(t_parts.m_LeftHandID, out var lArmRecord))
            {
                if (DataBase.TryGetRecord<AbilityProvider>(lArmRecord, out var ability))
                {
                    t_lArmAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
                }
            }
            if (DataBase.TryFindRecordByID(t_parts.m_RightHandID, out var rArmRecord))
            {
                if (DataBase.TryGetRecord<AbilityProvider>(rArmRecord, out var ability))
                {
                    t_rArmAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
                }
            }
            if (DataBase.TryFindRecordByID(t_parts.m_LeftLeg, out var lLegRecord))
            {
                if (DataBase.TryGetRecord<AbilityProvider>(lLegRecord, out var ability))
                {
                    t_lLegAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
                }
            }
            if (DataBase.TryFindRecordByID(t_parts.m_RightLeg, out var rLegRecord))
            {
                if (DataBase.TryGetRecord<AbilityProvider>(rLegRecord, out var ability))
                {
                    t_rLegAbility = DbUtility.GetAbilityDataFromAbilityRecord(ability.m_AbilityTemplateID);
                }
            }

            if (t_rArmAbility != null && t_lArmAbility != null
            && t_rArmAbility.m_AbilityTemplateID == t_lArmAbility.m_AbilityTemplateID)
            {
                DbUtility.DoubleAbilityStats(ref t_rArmAbility.m_Value);
                DbUtility.DoubleAbilityStats(ref t_lArmAbility.m_Value);
            }

            if (t_lLegAbility != null && t_rLegAbility != null
            && t_lLegAbility.m_AbilityTemplateID == t_rLegAbility.m_AbilityTemplateID)
            {
                var movements = DbUtility.CombineShifts(t_lLegAbility.m_Shifts, t_rLegAbility.m_Shifts);
                t_legsAbtData = t_lLegAbility;
                t_legsAbtData.m_Shifts = movements.ToList();
            }

            t_abilities.m_HeadAbility = t_headAbility;
            t_abilities.m_LeftHandAbility = t_lArmAbility;
            t_abilities.m_RightHandAbility = t_rArmAbility;
            t_abilities.m_LegsAbility = t_legsAbtData;


            t_abilities.m_TurnAroundAbility =
                DbUtility.GetAbilityDataFromAbilityRecord(L.TURN_AROUND_ABILITY_ID);

            stash_abilities.Set(a_enemyEntity, t_abilities);
        }
    }
}
