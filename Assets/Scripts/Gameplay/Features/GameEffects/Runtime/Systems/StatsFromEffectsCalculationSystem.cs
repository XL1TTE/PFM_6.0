using System;
using Domain.GameEffects;
using Domain.GameEffects.Modifiers;
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
                foreach (var permanent_effect in effects_pool.PermanentEffects)
                {
                    CalculateEffectContribution(entity, permanent_effect.EffectId);
                }
                foreach (var status_effect in effects_pool.StatusEffects)
                {
                    CalculateEffectContribution(entity, status_effect.EffectId);
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
                cur_subject_stats.MaxHealth += max_health_modifier.AdditiveValue;
                cur_subject_stats.MaxHealth =
                    (int)Math.Floor(cur_subject_stats.MaxHealth * 1 + max_health_modifier.MultiplierValue);
            }
            if (DataBase.TryGetRecord<MaxSpeedModifier>(effect_record, out var max_speed_modifier))
            {
                cur_subject_stats.MaxSpeed += max_speed_modifier.AdditiveValue;
                cur_subject_stats.MaxSpeed =
                    (int)Math.Floor(cur_subject_stats.MaxSpeed * 1 + max_speed_modifier.MultiplierValue);
            }

        }

        private void ResetToBaseStats(Entity subject)
        {
            var base_stats = stash_BaseStats.Get(subject);
            ref var cur_stats = ref stash_CurrentStats.Get(subject);

            cur_stats.MaxHealth = base_stats.MaxHealth;

            cur_stats.MaxSpeed = base_stats.MaxSpeed;
        }
    }
}
