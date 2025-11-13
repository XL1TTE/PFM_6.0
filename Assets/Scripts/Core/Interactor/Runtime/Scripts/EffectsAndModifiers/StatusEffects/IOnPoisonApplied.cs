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
        UniTask OnPoisonApplied(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world);
    }


    public sealed class WeakenBurning : BaseInteraction, IOnPoisonApplied
    {
        public UniTask OnPoisonApplied(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
        {
            if (V.IsBuring(a_target, a_world) == false) { return UniTask.CompletedTask; }

            ref var t_burnStatus = ref a_world.GetStash<BurningStatusComponent>().Get(a_target);
            ref var t_burns = ref t_burnStatus.m_Stacks;

            List<BurningStatusComponent.Stack> t_toRemove = new();
            for (int i = 0; i < t_burns.Count; ++i)
            {
                t_burns[i].m_DamagePerTurn -= a_damagePerTick;
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

