using System;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Persistence.Components;
using Persistence.DB;
using Persistence.Utilities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;

namespace Game
{
    public static partial class Battle
    {
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
                    }
                }
            }
        }

        private static void PlaceOnCell(Entity a_enemyEntity, Entity a_cell, World a_world)
        {
            var t_transform = a_world.GetStash<TransformRefComponent>();
            var cellPos = t_transform.Get(a_cell).Value.position;

            ref var enemyTransform = ref t_transform.Get(a_enemyEntity).Value;
            enemyTransform.position = cellPos;
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
                t_legsAbtData.m_Shifts = movements;
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
