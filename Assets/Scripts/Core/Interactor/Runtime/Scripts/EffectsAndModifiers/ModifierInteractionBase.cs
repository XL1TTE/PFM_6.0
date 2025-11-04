using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Persistence.DB;
using Scellecs.Morpeh;

namespace Interactions
{
    public abstract class ModifierInteractionBase<T>
        : BaseInteraction, IOnGameEffectApply, IOnGameEffectRemove where T : struct, IStatModifierComponent
    {
        public virtual async UniTask OnEffectApply(string a_EffectID, Entity a_Target, World a_world)
        {
            if (DataBase.TryFindRecordByID(a_EffectID, out var effectRecord) == false) { return; }

            if (ValidateEffect(effectRecord, a_world) == false) { return; }
            await UniTask.Yield();
            ApplyEffect(effectRecord, a_Target, a_world);
        }
        public virtual async UniTask OnEffectRemove(string a_EffectID, Entity a_Target, World a_world)
        {
            if (DataBase.TryFindRecordByID(a_EffectID, out var effectRecord) == false) { return; }

            if (ValidateEffect(effectRecord, a_world) == false) { return; }
            RemoveEffect(effectRecord, a_Target, a_world);
            await UniTask.Yield();
        }

        protected virtual bool ValidateEffect(Entity a_Effect, World a_world)
        {
            if (DataBase.TryGetRecord<T>(a_Effect, out var modifier))
            {
                return true;
            }
            return false;
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


            target_mod.m_Flat += effect_mod.m_Flat;
            target_mod.m_Multiplier += effect_mod.m_Multiplier;
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

            target_mod.m_Flat -= effect_mod.m_Flat;
            target_mod.m_Multiplier -= effect_mod.m_Multiplier;

        }
    }
}

