using System;
using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Persistence.DB;
using Scellecs.Morpeh;

namespace Interactions
{
    public sealed class MaxHealthModifierInteraction : ModifierInteractionBase<MaxHealthModifier>
    {
        public override async UniTask OnEffectApply(string a_EffectID, Entity a_Target, World a_world)
        {
            var stash_maxHealth = a_world.GetStash<MaxHealth>();
            int oldMaxHealth = stash_maxHealth.Has(a_Target) ? stash_maxHealth.Get(a_Target).m_Value : 0;

            await base.OnEffectApply(a_EffectID, a_Target, a_world);
            await UniTask.Yield();
            //ScaleHealthWithMaxHealth(a_Target, oldMaxHealth, a_world);
        }

        public override async UniTask OnEffectRemove(string a_EffectID, Entity a_Target, World a_world)
        {
            var stash_maxHealth = a_world.GetStash<MaxHealth>();
            int oldMaxHealth = stash_maxHealth.Has(a_Target) ? stash_maxHealth.Get(a_Target).m_Value : 0;

            await base.OnEffectRemove(a_EffectID, a_Target, a_world);
            await UniTask.Yield();
            //ScaleHealthWithMaxHealth(a_Target, oldMaxHealth, a_world);
        }

        private void ScaleHealthWithMaxHealth(Entity a_target, int oldMaxHealth, World a_world)
        {
            var stash_health = a_world.GetStash<Health>();
            var stash_maxHealth = a_world.GetStash<MaxHealth>();

            if (stash_health.Has(a_target) && stash_maxHealth.Has(a_target))
            {
                ref var health = ref stash_health.Get(a_target);
                ref var maxHealth = ref stash_maxHealth.Get(a_target);

                if (health.GetHealth() <= 0)
                {
                    return;
                }

                int maxHealthDelta = maxHealth.m_Value - oldMaxHealth;

                if (maxHealthDelta != 0)
                {
                    int newHealth = health.GetHealth() + maxHealthDelta;
                    health.SetHealth(Math.Max(1, newHealth));
                }
            }
        }

    }
}

