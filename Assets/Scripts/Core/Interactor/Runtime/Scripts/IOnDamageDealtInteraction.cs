using System;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Domain.Stats.Components;
using Scellecs.Morpeh;

namespace Interactions
{
    public interface IOnDamageDealtInteraction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Damage dealt to the target.</returns>
        UniTask Execute(
            Entity a_Attacker,
            Entity a_Target,
            World a_world,
            int a_Damage);
    }

    public sealed class ShowFloatingDamageInteraction : BaseInteraction, IOnDamageDealtInteraction
    {
        public override Priority m_Priority => base.m_Priority;

        public async UniTask Execute(Entity a_Attacker, Entity a_Target, World a_world, int a_Damage)
        {
            await UniTask.CompletedTask;
        }
    }
}

