

using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.BattleField.Components;
using Domain.Extentions;
using Domain.GameEffects;
using Domain.Stats.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Game;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnTurnStartInteraction
    {
        /// <summary>
        /// Invoke to start turn.
        /// </summary>
        /// <param name="a_turnTaker">Entity which turn is it.</param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        UniTask OnTurnStart(Entity a_turnTaker, World a_world);
    }

    public sealed class AddTurnTakerMarkInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            if (a_turnTaker.isNullOrDisposed(a_world)) { return UniTask.CompletedTask; }

            var stash_turnTaker = a_world.GetStash<CurrentTurnTakerTag>();
            stash_turnTaker.Set(a_turnTaker);
            return UniTask.CompletedTask;
        }
    }

    public sealed class EnableCellPointerView : BaseInteraction, IOnTurnStartInteraction
    {
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_cellView = a_world.GetStash<CellViewComponent>();
            var t_cell = GU.GetOccupiedCell(a_turnTaker, a_world);

            if (t_cell.isNullOrDisposed(a_world)) { return; }
            if (stash_cellView.Has(t_cell) == false) { return; }

            stash_cellView.Get(t_cell).m_Value.EnablePointerLayer();

            await UniTask.CompletedTask;
        }
    }

    public sealed class OnTurnStartECSNotifyInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            a_world.GetEvent<NextTurnStartedEvent>().NextFrame(new NextTurnStartedEvent
            {
                m_CurrentTurnTaker = a_turnTaker
            });
            return UniTask.CompletedTask;
        }
    }

    public sealed class ProcessTemporalEffectsInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_effectPool = a_world.GetStash<EffectsPoolComponent>();
            if (stash_effectPool.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<string> t_toRemove = new(4);

            ref var effects = ref stash_effectPool.Get(a_turnTaker);

            for (int i = 0; i < effects.m_StatusEffects.Count; ++i)
            {
                ref var turnsLeft = ref effects.m_StatusEffects[i].m_TurnsLeft;
                turnsLeft -= 1;
                if (turnsLeft < 0)
                {
                    t_toRemove.Add(effects.m_StatusEffects[i].m_EffectId);
                }
            }
            foreach (var i in t_toRemove)
            {
                G.Statuses.RemoveEffectFromPool(a_turnTaker, i, a_world);
            }
            return UniTask.CompletedTask;
        }
    }

    public sealed class ProcessBleedStatus : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_bleed = a_world.GetStash<BleedingStatusComponent>();
            if (stash_bleed.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<BleedingStatusComponent.Stack> t_toRemove = new(4);

            ref var t_bleed = ref stash_bleed.Get(a_turnTaker);
            for (int i = 0; i < t_bleed.m_Stacks.Count; ++i)
            {
                var stack = t_bleed.m_Stacks[i];
                ref var t_turnsLeft = ref t_bleed.m_Stacks[i].m_TurnsLeft;
                var t_damagePerTurn = stack.m_DamagePerTurn;


                int damage = t_damagePerTurn;
                //GU.ApplyResistanceToDamage<BleedResistanceModiffier>(a_turnTaker, ref damage, a_world);

                G.DealDamage(default, a_turnTaker, damage, Domain.Abilities.DamageType.BLEED_DAMAGE, a_world);
                t_turnsLeft -= 1;

                if (t_turnsLeft <= 0)
                {
                    t_toRemove.Add(stack);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemoveBleedingStack(a_turnTaker, stack, a_world);
            }

            return UniTask.CompletedTask;
        }
    }
    public sealed class ProcessPoisonStatus : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_poison = a_world.GetStash<PoisonStatusComponent>();
            if (stash_poison.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<PoisonStatusComponent.Stack> t_toRemove = new(4);

            ref var t_poison = ref stash_poison.Get(a_turnTaker);
            for (int i = 0; i < t_poison.m_Stacks.Count; ++i)
            {
                var stack = t_poison.m_Stacks[i];
                ref var t_turnsLeft = ref t_poison.m_Stacks[i].m_TurnsLeft;
                var t_damagePerTurn = stack.m_DamagePerTurn;

                t_turnsLeft -= 1;

                if (t_turnsLeft <= 0)
                {
                    t_toRemove.Add(stack);
                }
            }

            foreach (var stack in t_toRemove)
            {
                int damage = stack.m_DamagePerTurn * stack.m_Duration;
                //GU.ApplyResistanceToDamage<PoisonResistanceModiffier>(a_turnTaker, ref damage, a_world);

                G.DealDamage(default, a_turnTaker, damage, Domain.Abilities.DamageType.POISON_DAMAGE, a_world);
                G.Statuses.RemovePoisonStack(a_turnTaker, stack, a_world);
            }

            return UniTask.CompletedTask;
        }
    }
    public sealed class ProcessBurningStatus : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_burning = a_world.GetStash<BurningStatusComponent>();
            if (stash_burning.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<BurningStatusComponent.Stack> t_toRemove = new(4);

            ref var t_burn = ref stash_burning.Get(a_turnTaker);
            for (int i = 0; i < t_burn.m_Stacks.Count; ++i)
            {
                var stack = t_burn.m_Stacks[i];
                ref var t_turnsLeft = ref t_burn.m_Stacks[i].m_TurnsLeft;
                var t_damagePerTurn = stack.m_DamagePerTurn;

                int damage = stack.m_DamagePerTurn - (stack.m_Duration - stack.m_TurnsLeft);
                //GU.ApplyResistanceToDamage<BurningResistanceModiffier>(a_turnTaker, ref damage, a_world);

                G.DealDamage(default, a_turnTaker, damage, Domain.Abilities.DamageType.FIRE_DAMAGE, a_world);
                t_turnsLeft -= 1;

                if (t_turnsLeft <= 0)
                {
                    t_toRemove.Add(stack);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemoveBurningStack(a_turnTaker, stack, a_world);
            }

            return UniTask.CompletedTask;
        }
    }

}
