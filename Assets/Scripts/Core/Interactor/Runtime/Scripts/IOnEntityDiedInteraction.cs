using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Extentions;
using Domain.HealthBars.Components;
using DS.Files;
using Game;
using Persistence.DS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Interactions
{
    public interface IOnEntityDiedInteraction
    {
        UniTask OnEntityDied(
            Entity a_entity,
            Entity a_cause,
            World a_world);
    }

    public sealed class PlayDieAnimationInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public async UniTask OnEntityDied(Entity a_entity, Entity a_cause, World a_world)
        {
            var t_dieSeq = A.Die(a_entity, 4, a_world);
            t_dieSeq.Play();

            await UniTask.WaitWhile(() => t_dieSeq.IsActive());
            t_dieSeq?.Kill();
        }
    }

    public sealed class RemoveEntityFromTurnQueueInteraction : BaseInteraction, IOnEntityDiedInteraction
    {
        public override Priority m_Priority => Priority.HIGH;
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

            if (t_record.m_LastTurnTaker.Id == a_entity.Id) { t_record.m_LastTurnTaker = default; }
            if (t_record.m_CurrentTurnTaker.Id == a_entity.Id) { t_record.m_CurrentTurnTaker = default; }

            return UniTask.CompletedTask;
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
}

