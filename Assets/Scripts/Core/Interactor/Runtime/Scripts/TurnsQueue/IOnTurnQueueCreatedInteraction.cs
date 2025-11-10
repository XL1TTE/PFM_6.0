

using Cysharp.Threading.Tasks;
using Domain.TurnSystem.Components;
using Domain.UI.Mono;
using Domain.UI.Widgets;
using DS.Files;
using Persistence.DS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Interactions
{
    public interface IOnTurnQueueCreatedInteraction
    {
        UniTask IOnTurnQueueCreated(World a_world);
    }


    public sealed class RenderCreatedQueueInteraction : BaseInteraction, IOnTurnQueueCreatedInteraction
    {
        public UniTask IOnTurnQueueCreated(World a_world)
        {
            var stash_avatar = a_world.GetStash<TurnQueueAvatar>();

            ref var t_queue = ref DataStorage.GetRecordFromFile<BattleMeta, TurnsQueueRecord>();

            t_queue.m_QueueElementsRenderMap = new IntHashMap<TurnQueueElementView>();

            foreach (var m in t_queue.m_Queue)
            {
                var t_view = BattleFieldUIRefs.Instance.TurnQueueWidget.AddNewInQueue();
                t_queue.m_QueueElementsRenderMap.Add(m.Id, t_view, out _);

                if (stash_avatar.Has(m) == false) { continue; }
                t_view.SetAvatar(stash_avatar.Get(m).m_Value);
            }
            return UniTask.CompletedTask;
        }
    }
}
