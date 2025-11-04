using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.GameEffects;
using Scellecs.Morpeh;
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

