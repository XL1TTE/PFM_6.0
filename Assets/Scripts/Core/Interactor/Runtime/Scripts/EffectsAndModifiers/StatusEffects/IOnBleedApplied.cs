using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnBleedApplied
    {
        UniTask OnBleedApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }
    public interface IOnBleedRemoved
    {
        UniTask OnOnBleedRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }

    public sealed class WeakenPoisonsDamage : BaseInteraction, IOnBleedApplied
    {
        public UniTask OnBleedApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            if (V.IsPoisoned(a_target, a_world) == false) { return UniTask.CompletedTask; }

            var t_poisons = a_world.GetStash<PoisonStatusComponent>().Get(a_target).m_Stacks;

            List<IStatusEffectComponent.Stack> t_toRemove = new();
            for (int i = 0; i < t_poisons.Count; ++i)
            {
                t_poisons[i].m_DamagePerTurn -= a_stack.m_DamagePerTurn;
                if (t_poisons[i].m_DamagePerTurn <= 0)
                {
                    t_toRemove.Add(t_poisons[i]);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemovePoisonStack(a_target, stack, a_world);
            }
            return UniTask.CompletedTask;
        }
    }
}

