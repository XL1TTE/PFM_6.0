using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnBurningApplied
    {
        UniTask OnBurningApplied(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world);
    }

    public sealed class WeakenBleedingDamage : BaseInteraction, IOnBurningApplied
    {
        public UniTask OnBurningApplied(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
        {
            if (V.IsBleeding(a_target, a_world) == false) { return UniTask.CompletedTask; }

            ref var t_poisons = ref a_world.GetStash<BleedingStatusComponent>().Get(a_target).m_Stacks;

            List<BleedingStatusComponent.Stack> t_toRemove = new();
            for (int i = 0; i < t_poisons.Count; ++i)
            {
                t_poisons[i].m_DamagePerTurn -= a_damagePerTick;
                if (t_poisons[i].m_DamagePerTurn <= 0)
                {
                    t_toRemove.Add(t_poisons[i]);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemoveBleedingStack(a_target, stack, a_world);
            }
            return UniTask.CompletedTask;
        }
    }
}

