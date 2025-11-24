using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Game;
using Gameplay.FloatingDamage.Systems;
using Persistence.Utilities;
using Scellecs.Morpeh;
using UI.Elements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Interactions
{

    public interface IOnGameEffectApply
    {
        UniTask OnEffectApply(
            string a_EffectID,
            Entity a_Target,
            World a_world);
    }
    public interface IOnGameEffectRemove
    {
        UniTask OnEffectRemove(
            string a_EffectID,
            Entity a_Target,
            World a_world);
    }

    public sealed class NotifyAboveEnemyEffectName : BaseInteraction, IOnGameEffectApply
    {
        public override Priority m_Priority => Priority.HIGH;
        public UniTask OnEffectApply(string a_EffectID, Entity a_Target, World a_world)
        {
            var position = GU.GetTransform(a_Target, a_world).position;
            var notification = DbUtility.GetNameFromRecordWithID(a_EffectID);

            FloatingGui.Show(position, TextPool.I()
                .WarmupElement()
                .SetText(notification)
                .FontSize(T.TEXT_SIZE_H2)
                .SetColor(C.COLOR_DEFAULT));

            return UniTask.CompletedTask;
        }
    }

    public sealed class TriggerHealthBarUpdateInteraction
        : BaseInteraction, IOnGameEffectApply, IOnGameEffectRemove
    {

        public override Priority m_Priority => Priority.VERY_LOW;

        public async UniTask OnEffectApply(string a_EffectID, Entity a_Target, World a_world)
        {
            GU.UpdateHealthBarValueFor(a_Target, a_world);
            await UniTask.Yield();
        }

        public async UniTask OnEffectRemove(string a_EffectID, Entity a_Target, World a_world)
        {
            GU.UpdateHealthBarValueFor(a_Target, a_world);
            await UniTask.Yield();
        }
    }
}

