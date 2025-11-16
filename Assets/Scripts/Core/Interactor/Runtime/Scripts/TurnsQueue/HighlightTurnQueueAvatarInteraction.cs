

using Cysharp.Threading.Tasks;
using DS.Files;
using Persistence.DS;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;

namespace Interactions
{
    public sealed class HighlightTurnQueueAvatarInteraction : BaseInteraction, IOnTurnEndInteraction, IOnTurnStartInteraction
    {
        public UniTask OnTurnStart(Entity a_turnTaker, World a_world)
        {
            var t_record = DataStorage.GetRecordFromFile<BattleMeta, TurnsQueueRecord>();
            var t_view = t_record.m_QueueElementsRenderMap.TryGetValueRefByKey(a_turnTaker.Id, out var exist);
            if (exist)
            {
                t_view?.EnableHighlighting();
            }
            return UniTask.CompletedTask;
        }

        public UniTask OnTurnEnd(Entity a_turnTaker, World a_world)
        {
            var t_record = DataStorage.GetRecordFromFile<BattleMeta, TurnsQueueRecord>();
            var t_view = t_record.m_QueueElementsRenderMap.TryGetValueRefByKey(a_turnTaker.Id, out var exist);
            if (exist)
            {
                t_view?.DisableHighlighting();
            }
            return UniTask.CompletedTask;
        }
    }
}
