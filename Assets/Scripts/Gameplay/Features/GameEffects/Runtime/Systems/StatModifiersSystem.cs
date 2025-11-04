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

    public void OnAwake()
    {
        f_toProcess = World.Filter
            .With<Modifier>()
            .Build();

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

            ProcessSingleEntity(entity);
        }
    }

    [BurstCompile]
    private void ProcessSingleEntity(Entity entity)
    {
        ResetAffectedStats(entity);

        ApplyModifierToStat(entity);
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


    public void Dispose() { }
}
