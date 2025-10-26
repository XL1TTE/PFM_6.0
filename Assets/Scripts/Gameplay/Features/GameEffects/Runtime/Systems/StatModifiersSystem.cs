using System;
using System.Collections.Generic;
using Domain.GameEffects;
using Domain.Stats.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Native;
using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public class StatModifiersSystem<Modifier, AffectedStat> : ISystem
    where Modifier : struct, IStatModifierComponent
    where AffectedStat : struct, IStatComponent
{
    public World World { get; set; }

    private Stash<Modifier> stash_Modifier;
    private Stash<AffectedStat> stash_AffectedStat;
    private Filter f_toProcess;
    private Stash<EffectsPoolComponent> stash_Effects;

    public void OnAwake()
    {
        f_toProcess = World.Filter
            .With<EffectsPoolComponent>()
            .Build();

        stash_Effects = World.GetStash<EffectsPoolComponent>();
        stash_Modifier = World.GetStash<Modifier>();
        stash_AffectedStat = World.GetStash<AffectedStat>();
    }

    public void OnUpdate(float deltaTime)
    {
        ProcessEffectModifiers();
    }

    private void ProcessEffectModifiers()
    {
        var nativeFilter = this.f_toProcess.AsNative();

        for (int i = 0; i < nativeFilter.length; i++)
        {
            var entity = nativeFilter[i];
            if (!stash_Effects.Has(entity)) continue;

            ProcessSingleEntity(entity);
        }
    }

    [BurstCompile]
    private void ProcessSingleEntity(Entity entity)
    {
        ResetModifiers(entity);
        ResetAffectedStats(entity);

        ref var effects_pool = ref stash_Effects.Get(entity);

        ProcessPermanentEffectsList(entity, effects_pool.PermanentEffects);
        ProcessStatusEffectsList(entity, effects_pool.StatusEffects);

        ApplyModifierToStat(entity);
    }

    [BurstCompile]
    private void ProcessStatusEffectsList(Entity entity, List<StatusEffect> effects)
    {
        int count = effects.Count;
        for (int i = 0; i < count; i++)
        {
            CalculateEffectContribution(entity, effects[i].EffectId);
        }
    }

    [BurstCompile]
    private void ProcessPermanentEffectsList(Entity entity, List<PermanentEffect> effects)
    {
        int count = effects.Count;
        for (int i = 0; i < count; i++)
        {
            CalculateEffectContribution(entity, effects[i].EffectId);
        }
    }

    [BurstCompile]
    private void ResetAffectedStats(Entity entity)
    {
        if (!stash_AffectedStat.Has(entity))
        {
            stash_AffectedStat.Set(entity);
        }

        ref var stats = ref stash_AffectedStat.Get(entity);
        stats.m_Value = 0;
    }

    [BurstCompile]
    private void ResetModifiers(Entity entity)
    {
        stash_Modifier.Set(entity);
    }

    [BurstCompile]
    private void ApplyModifierToStat(Entity entity)
    {
        var modifier = stash_Modifier.Get(entity);
        ref var stats = ref stash_AffectedStat.Get(entity);

        stats.m_Value = CalculateFinalValue(stats.m_Value, modifier.m_Flat, modifier.m_Multiplier);
    }

    [BurstCompile]
    private static int CalculateFinalValue(int baseValue, float flat, float multiplier)
    {
        float result = (baseValue + flat) * (1f + multiplier);
        return (int)Math.Round(result);
    }

    [BurstCompile]
    private void CalculateEffectContribution(Entity entity, string effectId)
    {
        if (DataBase.TryFindRecordByID(effectId, out var effect_record))
        {
            if (DataBase.TryGetRecord<Modifier>(effect_record, out var modifier))
            {
                ref var modifier_value = ref stash_Modifier.Get(entity);
                modifier_value.m_Flat += modifier.m_Flat;
                modifier_value.m_Multiplier += modifier.m_Multiplier;
            }
        }
    }

    public void Dispose() { }
}
