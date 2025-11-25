using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnPoisonApplied
    {
        UniTask OnPoisonApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }
    public interface IOnPoisonRemoved
    {
        UniTask OnPoisonRemoved(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world);
    }

    public sealed class WeakenBurning : BaseInteraction, IOnPoisonApplied
    {
        public UniTask OnPoisonApplied(Entity a_source, Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
        {
            if (V.IsBuring(a_target, a_world) == false) { return UniTask.CompletedTask; }

            ref var t_burnStatus = ref a_world.GetStash<BurningStatusComponent>().Get(a_target);
            var t_burns = t_burnStatus.m_Stacks;

            List<IStatusEffectComponent.Stack> t_toRemove = new();
            for (int i = 0; i < t_burns.Count; ++i)
            {
                t_burns[i].m_DamagePerTurn -= a_stack.m_DamagePerTurn;
                if (t_burns[i].m_DamagePerTurn <= 0)
                {
                    t_toRemove.Add(t_burns[i]);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemoveBurningStack(a_target, stack, a_world);
            }
            return UniTask.CompletedTask;
        }
    }

}

