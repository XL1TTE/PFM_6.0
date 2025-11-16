using System;
using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Persistence.DB;
using Scellecs.Morpeh;
using static Domain.Stats.Components.IResistanceModiffier;

namespace Interactions
{
    public abstract class ResistanceInteractionBase<T> : BaseInteraction, IOnGameEffectApply, IOnGameEffectRemove
    where T : struct, IResistanceModiffier
    {
        public async UniTask OnEffectApply(string a_EffectID, Entity a_Target, World a_world)
        {
            if (DataBase.TryFindRecordByID(a_EffectID, out var effectRecord) == false) { return; }

            if (ValidateEffect(effectRecord, a_world) == false) { return; }
            await UniTask.Yield();
            ApplyEffect(effectRecord, a_Target, a_world);
        }

        public async UniTask OnEffectRemove(string a_EffectID, Entity a_Target, World a_world)
        {
            if (DataBase.TryFindRecordByID(a_EffectID, out var effectRecord) == false) { return; }

            if (ValidateEffect(effectRecord, a_world) == false) { return; }
            RemoveEffect(effectRecord, a_Target, a_world);
            await UniTask.Yield();
        }

        protected virtual void ApplyEffect(Entity a_Effect, Entity a_Target, World a_world)
        {
            var stash_modifier = a_world.GetStash<T>();
            if (stash_modifier.Has(a_Target) == false)
            {
                stash_modifier.Set(a_Target);
            }

            ref var target_mod = ref stash_modifier.Get(a_Target);
            DataBase.TryGetRecord<T>(a_Effect, out var effect_mod);

            target_mod.m_Stage = (Stage)Math.Clamp((Int32)target_mod.m_Stage + (Int32)effect_mod.m_Stage, 0, 2);
        }

        protected virtual void RemoveEffect(Entity a_Effect, Entity a_Target, World a_world)
        {
            var stash_modifier = a_world.GetStash<T>();
            if (stash_modifier.Has(a_Target) == false)
            {
                stash_modifier.Set(a_Target);
            }

            ref var target_mod = ref stash_modifier.Get(a_Target);
            DataBase.TryGetRecord<T>(a_Effect, out var effect_mod);

            target_mod.m_Stage = (Stage)Math.Clamp((Int32)target_mod.m_Stage - (Int32)effect_mod.m_Stage, 0, 2);
        }

        protected virtual bool ValidateEffect(Entity a_Effect, World a_world)
        {
            if (DataBase.TryGetRecord<T>(a_Effect, out var modifier))
            {
                return true;
            }
            return false;
        }
    }

    public sealed class IBleedResistanceInteraction : ResistanceInteractionBase<BleedResistanceModiffier> { }
    public sealed class IPoisonResistanceInteraction : ResistanceInteractionBase<PoisonResistanceModiffier> { }
    public sealed class IBurningResistanceInteraction : ResistanceInteractionBase<BurningResistanceModiffier> { }
}

