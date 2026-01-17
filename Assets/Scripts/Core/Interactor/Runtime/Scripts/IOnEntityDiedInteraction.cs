using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.HealthBars.Components;
using Domain.Monster.Tags;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using Domain.TurnSystem.Tags;
using Domain.UI.Mono;
using DS.Files;
using Game;
using Persistence.DS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Interactions.IOnEntityDiedInteraction
{

    public interface IOnEntityDiedInteraction
    {
        UniTask OnEntityDied(
            Entity a_entity,
            Entity a_cause,
            World a_world);
    }


    public sealed class DisableAllAbilities : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;
        public UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            G.UpdateAbilityButtonsState(a_world);
            return UniTask.CompletedTask;
        }
    }
    public sealed class DisableNextTurnButtonIfControledByPlayer : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;
        public UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            if (F.IsAiControlled(a_entity, a_world)) { return UniTask.CompletedTask; }

            BattleUiRefs.Instance.m_NextTurnButton.Hide();
            return UniTask.CompletedTask;
        }
    }

    public sealed class DisableAI : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.VERY_HIGH;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            await UniTask.Yield();

            GU.DisposeAI(a_entity, a_world);

            var stash_aiCancell = a_world.GetStash<AgentAICancellationToken>();
            if (stash_aiCancell.Has(a_entity) == false) { return; }

            stash_aiCancell.Get(a_entity).m_TokenSource?.Cancel();
            stash_aiCancell.Get(a_entity).m_TokenSource?.Dispose();
        }
    }
    public sealed class ClearHealthBarsOnDeathInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.HIGH;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            var stash_healthBarLink = a_world.GetStash<HealthBarLink>();
            if (stash_healthBarLink.Has(a_entity) == false) { return; }

            var t_healthBar = stash_healthBarLink.Get(a_entity).HealthBarEntity;
            if (a_world.TryGetComponent<HealthBarViewLink>(t_healthBar, out var healthBarView))
            {
                healthBarView.Value?.DestorySelf();
            }

            a_world.RemoveEntity(t_healthBar);

            await UniTask.CompletedTask;
        }
    }


    public sealed class PlayDieAnimationForEnemyInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            if (F.IsEnemy(a_entity, a_world) == false) { return; }

            var t_dieSeq = A.Die(a_entity, 4, a_world);
            t_dieSeq.Play();

            await UniTask.WaitWhile(() => t_dieSeq.IsActive());
            t_dieSeq?.Kill();
        }
    }
    public sealed class PlayDieAnimationForMonsterInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            if (F.IsMonster(a_entity, a_world) == false) { return; }

            await UniTask.WaitForSeconds(2.5f);
        }
    }


    public sealed class RemoveEntityFromTurnQueueInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.NORMAL;
        public UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            ref var t_record = ref DataStorage.GetRecordFromFile<BattleMeta, TurnsQueueRecord>();

            if (t_record.m_QueueElementsRenderMap.Has(a_entity.Id))
            {
                t_record.m_QueueElementsRenderMap.Remove(a_entity.Id, out var view);
                view.DestroySelf();
            }
            if (t_record.m_Queue.Contains(a_entity))
            {
                t_record.m_Queue.Remove(a_entity);
            }

            // if (t_record.m_LastTurnTaker.Id == a_entity.Id) { t_record.m_LastTurnTaker = default; }
            // if (t_record.m_CurrentTurnTaker.Id == a_entity.Id) { t_record.m_CurrentTurnTaker = default; }

            return UniTask.CompletedTask;
        }
    }

    public sealed class DisableCellPointerView : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.LOW;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            var stash_cellView = a_world.GetStash<CellViewComponent>();
            var t_cell = GU.GetOccupiedCell(a_entity, a_world);

            if (t_cell.isNullOrDisposed(a_world)) { return; }
            if (stash_cellView.Has(t_cell) == false) { return; }

            stash_cellView.Get(t_cell).m_Value.DisablePointerLayer();

            await UniTask.CompletedTask;
        }
    }

    public sealed class UnoccupyCellOnDeathInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.LOW;
        public UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            G.UnoccupyCell(GU.GetOccupiedCell(a_entity, a_world), a_world);
            return UniTask.CompletedTask;
        }
    }

    public sealed class EndTurnIfDied : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.LOW;
        public UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            var stash_curTurnTaker = a_world.GetStash<CurrentTurnTakerTag>();

            if (stash_curTurnTaker.Has(a_entity) == false) { return UniTask.CompletedTask; }

            G.NextTurn(a_world);
            return UniTask.CompletedTask;
        }
    }

    public sealed class CheckPlayerWinCondition : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            await UniTask.Yield(); // Wait for world update.

            var filter = a_world.Filter.With<TagEnemy>().Without<DiedEntityTag>().Build();

            if (filter.IsEmpty())
            {
                G.Battle.PlayerWon(a_world);
            }
        }
    }
    public sealed class CheckPlayerLostCondition : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.VERY_LOW;
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            await UniTask.Yield(); // Wait for world update.

            var filter = a_world.Filter.With<TagMonster>().Without<DiedEntityTag>().Build();

            var t_enemies = a_world.Filter.With<TagEnemy>().Build();

            if (filter.IsEmpty())
            {
                foreach (var e in t_enemies)
                {
                    GU.DisposeAI(e, a_world);
                }
                G.Battle.PlayerLost(a_world);
            }
        }
    }

}

