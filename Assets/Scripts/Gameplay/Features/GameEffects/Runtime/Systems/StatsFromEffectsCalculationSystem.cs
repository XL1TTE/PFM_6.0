using System;
using Domain.GameEffects;
using Domain.Stats.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.GameEffects
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class StatsFromEffectsCalculationSystem : ISystem
    {
        public World World { get; set; }

        private Filter f_entitiesToProcess;
        private Stash<EffectsPoolComponent> stash_EffectsPool;
        private Stash<BaseStatsComponent> stash_BaseStats;
        private Stash<CurrentStatsComponent> stash_CurrentStats;

        public void OnAwake()
        {
            f_entitiesToProcess = World.Filter
                .With<EffectsPoolComponent>()
                .With<BaseStatsComponent>()
                .With<CurrentStatsComponent>()
                .Build();

            stash_EffectsPool = World.GetStash<EffectsPoolComponent>();

            stash_BaseStats = World.GetStash<BaseStatsComponent>();
            stash_CurrentStats = World.GetStash<CurrentStatsComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in f_entitiesToProcess)
            {

                ResetToBaseStats(entity);

                ref var effects_pool = ref stash_EffectsPool.Get(entity);
                foreach (var permanent_effect in effects_pool.m_PermanentEffects)
                {
                    CalculateEffectContribution(entity, permanent_effect.m_EffectId);
                }
                foreach (var status_effect in effects_pool.m_StatusEffects)
                {
                    CalculateEffectContribution(entity, status_effect.m_EffectId);
                }
            }
        }

        public void Dispose()
        {

        }


        private void CalculateEffectContribution(Entity subject, string effect_id)
        {

            if (DataBase.TryFindRecordByID(effect_id, out var effect_record) == false) { return; }

            var base_subject_stats = stash_BaseStats.Get(subject);
            ref var cur_subject_stats = ref stash_CurrentStats.Get(subject);

            if (DataBase.TryGetRecord<MaxHealthModifier>(effect_record, out var max_health_modifier))
            {
                cur_subject_stats.m_MaxHealth += max_health_modifier.m_Flat;
                cur_subject_stats.m_MaxHealth =
                    (int)Math.Floor(cur_subject_stats.m_MaxHealth * 1 + max_health_modifier.m_Multiplier);
            }
            if (DataBase.TryGetRecord<SpeedModifier>(effect_record, out var max_speed_modifier))
            {
                cur_subject_stats.m_MaxSpeed += max_speed_modifier.m_Flat;
                cur_subject_stats.m_MaxSpeed =
                    (int)Math.Floor(cur_subject_stats.m_MaxSpeed * 1 + max_speed_modifier.m_Multiplier);
            }

        }

        private void ResetToBaseStats(Entity subject)
        {
            var base_stats = stash_BaseStats.Get(subject);
            ref var cur_stats = ref stash_CurrentStats.Get(subject);

            cur_stats.m_MaxHealth = base_stats.MaxHealth;

            cur_stats.m_MaxSpeed = base_stats.MaxSpeed;
        }
    }
}
