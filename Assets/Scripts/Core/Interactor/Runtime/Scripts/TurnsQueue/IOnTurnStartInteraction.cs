using System.Collections.Generic;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.GameEffects;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Tags;
using Domain.UI.Mono;
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


    public sealed class BurnIfOnBurnedCell : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.HIGH;
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var cell = GU.GetOccupiedCell(a_turnTaker, a_world);
            if (F.IsBurnedCell(cell, a_world))
            {
                var damage = a_world.GetStash<TagBurnedCell>().Get(cell).m_Damage;
                await G.DealDamageAsync(cell, a_turnTaker, damage, Domain.Abilities.DamageType.FIRE_DAMAGE, a_world);
            }
        }
    }
    public sealed class PoisonIfOnPoisonedCell : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.HIGH;
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var cell = GU.GetOccupiedCell(a_turnTaker, a_world);
            if (F.IsPoisonedCell(cell, a_world))
            {
                var damage = a_world.GetStash<TagPoisonedCell>().Get(cell).m_Damage;
                await G.DealDamageAsync(cell, a_turnTaker, damage, Domain.Abilities.DamageType.POISON_DAMAGE, a_world);
            }
        }
    }
    public sealed class BleedIfOnThornsCell : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.HIGH;
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var cell = GU.GetOccupiedCell(a_turnTaker, a_world);
            if (F.IsCellWithThorns(cell, a_world))
            {
                var damage = a_world.GetStash<TagCellWithThorns>().Get(cell).m_Damage;
                await G.DealDamageAsync(cell, a_turnTaker, damage, Domain.Abilities.DamageType.BLEED_DAMAGE, a_world);
            }
        }
    }

    public sealed class EvaluateNextTurnButton : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;

        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            if (F.IsAiControlled(a_turnTaker, a_world))
            {
                BattleUiRefs.Instance.m_NextTurnButton.Hide();
            }
            else if (F.IsMonster(a_turnTaker, a_world))
            {
                BattleUiRefs.Instance.m_NextTurnButton.Show();
            }

            return UniTask.CompletedTask;
        }
    }

    public sealed class RefreshInteractions : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;

        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            G.RefreshInteractions(a_turnTaker, a_world);
            return UniTask.CompletedTask;
        }
    }

    public sealed class ShowTurnTakerStatsInBook : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            G.Battle.UpdateTurnTakerPageInBook(a_turnTaker, a_world);
            return UniTask.CompletedTask;
        }
    }

    public sealed class AddTurnTakerMarkInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;
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
        public override Priority m_Priority => Priority.VERY_HIGH;

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

    public sealed class ActivateAgentAI : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.LOW;
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            await UniTask.Yield();

            var stash_agentAI = a_world.GetStash<AgentAIComponent>();
            if (stash_agentAI.Has(a_turnTaker) == false) { return; }

            var ai = stash_agentAI.Get(a_turnTaker);
            ai.m_AIModel.Process(a_turnTaker, a_world).Forget();
        }
    }

    public sealed class ProcessTemporalEffectsInteraction : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;

        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_effectPool = a_world.GetStash<EffectsPoolComponent>();
            if (stash_effectPool.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<string> t_toRemove = new(4);

            ref var effects = ref stash_effectPool.Get(a_turnTaker);

            for (int i = 0; i < effects.m_TemporalEffects.Count; ++i)
            {
                ref var turnsLeft = ref effects.m_TemporalEffects[i].m_TurnsLeft;
                turnsLeft -= 1;
                if (turnsLeft < 0)
                {
                    t_toRemove.Add(effects.m_TemporalEffects[i].m_EffectId);
                }
            }
            foreach (var i in t_toRemove)
            {
                G.Statuses.RemoveEffectFromPool(a_turnTaker, i, a_world);
            }
            return UniTask.CompletedTask;
        }
    }

    public sealed class ProcessStunStatus : BaseInteraction, IOnTurnStartInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW; // Must be called be after ActivateAgentAI.
        public async UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            if (V.IsStuned(a_turnTaker, a_world) == false) { return; }

            if (F.IsMonster(a_turnTaker, a_world))
            {
                G.ConsumeAllInteractions(a_turnTaker, a_world); // Consumes all interactions if player's controlled.
                BattleUiRefs.Instance.m_NextTurnButton.Hide();
            }

            if (F.IsAiControlled(a_turnTaker, a_world)) // Disable ai.
            {
                await UniTask.Yield(); // wait for world updates.

                var stash_aiCancell = a_world.GetStash<AgentAICancellationToken>();
                stash_aiCancell.Get(a_turnTaker).m_TokenSource?.Cancel();
            }

            ProcessStacksDisappear(a_turnTaker, a_world);

            await UniTask.WaitForSeconds(3f); // Here may trigger visuals.

            G.NextTurn(a_world);
        }

        private void ProcessStacksDisappear(Entity a_turnTaker, World a_world)
        {
            List<IStatusEffectComponent.Stack> t_toRemove = new(4);

            ref var t_stuns = ref a_world.GetStash<StunStatusComponent>().Get(a_turnTaker);

            for (int i = 0; i < t_stuns.m_Stacks.Count; ++i)
            {
                var stack = t_stuns.m_Stacks[i];
                ref var t_turnsLeft = ref t_stuns.m_Stacks[i].m_TurnsLeft;

                t_turnsLeft -= 1;

                if (t_turnsLeft <= 0)
                {
                    t_toRemove.Add(stack);
                }
            }

            foreach (var stack in t_toRemove)
            {
                G.Statuses.RemoveStunStack(a_turnTaker, stack, a_world);
            }
        }

    }

    public sealed class ProcessBleedStatus : BaseInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var stash_bleed = a_world.GetStash<BleedingStatusComponent>();
            if (stash_bleed.Has(a_turnTaker) == false) { return UniTask.CompletedTask; }

            List<IStatusEffectComponent.Stack> t_toRemove = new(4);

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

            List<IStatusEffectComponent.Stack> t_toRemove = new(4);

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

            List<IStatusEffectComponent.Stack> t_toRemove = new(4);

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
